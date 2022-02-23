using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public Transform penguinXROTransform;
    
    private float _lastPositionZ;
    private float _totalDistance;

    private const float GenerateDistance = 1000;  // generate this far in advance
    private const float ObstacleInterval = 100;  // generate obstacles in this interval
    
    private int _numObstacleIntervalsGenerated = 0;
    private float _locationOfLastCheckpoint;
    private int _numCheckpointsInstantiated = 0;
    
    private const float ObstacleCheckRadius = 15f;
    private int _maxSpawnAttemptsPerObstacle = 10;  // to prevent infinite loop

    private const float MaxObstacleHeight = 500f;

    public GameObject[] obstacleTypes;

    // TODO: remove, this is for debugging purposes
    public static int NumObstaclesActiveInGame = 0;
    
    private static LevelManager _instance;

    public static LevelManager Instance
    {
        get
        {
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        _lastPositionZ = penguinXROTransform.position.z;

        // Hardcode spawn first checkpoint
        float checkpointX = Random.Range(-75f, 75f);
        float checkpointZ = 200f;
        Instantiate(Resources.Load("Checkpoint"),
            new Vector3(checkpointX, 20, checkpointZ),
            Quaternion.identity);
        _locationOfLastCheckpoint = checkpointZ;
        
        // Don't generate obstacles in the first interval
        for (int i = 1; i < GenerateDistance / ObstacleInterval; i++)
        {
            GenerateObstacles(i * ObstacleInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionZ = penguinXROTransform.position.z;
        _totalDistance += (newPositionZ - _lastPositionZ);
        _lastPositionZ = newPositionZ;

        if ((int)(_totalDistance / ObstacleInterval) == _numObstacleIntervalsGenerated)
        {
            GenerateObstacles(_totalDistance + GenerateDistance);
            _numObstacleIntervalsGenerated++;
        }
        
        Debug.Log("SAVE:numObstaclesActive:" + NumObstaclesActiveInGame);
    }

    public void ReadyToGenerateCheckpoint()
    {
        StartCoroutine(IncreaseSpeed(GetSpeedIncrease()));
            
        _locationOfLastCheckpoint = Single.MaxValue;
        GenerateCheckpoint();
        _numCheckpointsInstantiated++;
        Debug.Log("num checkpoints passed:" + _numCheckpointsInstantiated);
    }
    
    private void GenerateCheckpoint()
    {
        GliderInfo gliderInfo = GameController.Instance.gliderInfo;

        float predictedPositionY = gliderInfo.penguinXROTransform.position.y;
        // TODO: physics says this should be sqrt, but removing it gives a much better approximation
        float targetHeightToHitCheckpoint = 50;
        float timeUntilWeHitGround = -2 * (predictedPositionY - targetHeightToHitCheckpoint)/ (Physics.gravity.y + gliderInfo.drag); // sqrt

        float projectedSpeed = gliderInfo.ActualSpeed + GetSpeedIncrease();
        float distanceWhereWeWillLand = projectedSpeed * timeUntilWeHitGround;
            
        Debug.Log("time until land:" + timeUntilWeHitGround);
        Debug.Log("distance until land:" + distanceWhereWeWillLand);

        float minSpawnOffset = 200f;
        float spawnZ = gliderInfo.penguinXROTransform.position.z + Math.Max(minSpawnOffset, distanceWhereWeWillLand);
        _locationOfLastCheckpoint = spawnZ;
        
        float spawnX = Random.Range(-50f, 50f);
        Instantiate(Resources.Load("Checkpoint"), new Vector3(spawnX, 20, spawnZ), Quaternion.identity);
    }

    private void GenerateObstacles(float startOfInterval)
    {
        // Generate obstacles in the danger zone
        int numObstaclesPerInterval = 2 + _numCheckpointsInstantiated / 3;
        for (int i = 0; i < numObstaclesPerInterval; i++)
        {
            SpawnRandomObstacle(startOfInterval, GetPositionForObstacleInDangerZone);
        }

        // Generate cosmetic obstacles
        // int numCosmeticLower = Math.Max(2, (int) penguinXROTransform.position.y / 100);
        // for (int i = 0; i < numCosmeticLower; i++)
        // {
        //     SpawnRandomObstacle(startOfInterval, GetPositionForCosmeticObstacleLower);
        // }
        //
        // int numCosmeticHigher = Math.Max(2, (int)(MaxObstacleHeight - penguinXROTransform.position.y) / 100);
        // for (int i = 0; i < numCosmeticHigher; i++)
        // {
        //     SpawnRandomObstacle(startOfInterval, GetPositionForCosmeticObstacleHigher);
        // }
    }

    private void SpawnRandomObstacle(float startOfInterval, Func<Obstacle, float, Vector3> getPositionForObstacle)
    {
        Vector3 position = Vector3.zero;
        bool validPosition = false;
        int spawnAttempts = 0;
            
        // Choose a random obstacle
        GameObject obstacle = obstacleTypes[Random.Range(0, obstacleTypes.Length)];
        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();

        // While we don't have a valid position and we haven't tried spawning this obstacle too many times
        while(!validPosition && spawnAttempts < _maxSpawnAttemptsPerObstacle)
        {
            spawnAttempts++;

            // Pick a random position
            position = getPositionForObstacle(obstacleScript, startOfInterval);

            // Collect all colliders within our Obstacle Check Radius
            Collider[] colliders = Physics.OverlapSphere(position, ObstacleCheckRadius);

            validPosition = colliders.Length == 0;
        }
            
        if(validPosition)
        {
            // Spawn the obstacle here
            Instantiate(obstacle, position, obstacleScript.GetSpawnRotation());
            NumObstaclesActiveInGame++;
        }
    }

    // Obstacles that the user will probably player
    private Vector3 GetPositionForObstacleInDangerZone(Obstacle obstacleScript, float startOfInterval)
    {
        float x = Random.Range(-150, 150);
        float y = penguinXROTransform.position.y + 
                  Random.Range(obstacleScript.GetSpawnOffsetLowerBound(), obstacleScript.GetSpawnOffsetUpperBound());
        y = Math.Max(15, y);  // min spawn height of 15
        float z = startOfInterval + Random.Range(0, ObstacleInterval);
        return new Vector3(x, y, z);
    }
    
    // Obstacles that are lower than the player
    private Vector3 GetPositionForCosmeticObstacleLower(Obstacle obstacleScript, float startOfInterval)
    {
        // TODO: maybe this x range is too wide
        float x = Random.Range(-200, 200);
        float y = Random.Range(10, obstacleScript.GetSpawnOffsetLowerBound() + penguinXROTransform.position.y);
        float z = startOfInterval + Random.Range(0, ObstacleInterval);
        return new Vector3(x, y, z);
    }
    
    // Obstacles that are higher than the player
    private Vector3 GetPositionForCosmeticObstacleHigher(Obstacle obstacleScript, float startOfInterval)
    {
        float x = Random.Range(-200, 200);
        float y = Random.Range(penguinXROTransform.position.y + obstacleScript.GetSpawnOffsetUpperBound(), MaxObstacleHeight);
        float z = startOfInterval + Random.Range(0, ObstacleInterval);
        return new Vector3(x, y, z);
    }

    private float GetSpeedIncrease()
    {
        // Increase speed depending on number of checkpoints we've passed
        if (_numCheckpointsInstantiated == 0)
        {
            return 0f;
        }
        if (_numCheckpointsInstantiated < 5)
        {
            return 2f;
        }
        if (_numCheckpointsInstantiated < 10)
        {
            return 1f;
        }
        if (_numCheckpointsInstantiated < 20)
        {
            return 0.5f;
        }
        if (_numCheckpointsInstantiated < 30)
        {
            return 0.1f;
        }
        return 0;
    }
    
    IEnumerator IncreaseSpeed(float speedIncrease)
    {
        // Apply speed over 10 seconds
        for (int i = 0; i < 100; i++)
        {
            GameController.Instance.gliderInfo.speed += 0.01f * speedIncrease;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
