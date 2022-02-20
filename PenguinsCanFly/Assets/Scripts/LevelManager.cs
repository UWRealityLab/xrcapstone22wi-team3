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
    private int _numCheckpointsInstantiated = 0;
    
    private const float ObstacleCheckRadius = 10f;
    private int _maxSpawnAttemptsPerObstacle = 10;  // to prevent infinite loop

    private const float MaxObstacleHeight = 500f;

    public GameObject[] obstacleTypes;

    // Start is called before the first frame update
    void Start()
    {
        _lastPositionZ = penguinXROTransform.position.z;

        for (int i = 1; i < GenerateDistance / GetCheckpointInterval(); i++)
        {
            GenerateCheckpoint(i * GetCheckpointInterval());
        }
        
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

        // Spawn checkpoints
        if ((int)(_totalDistance / GetCheckpointInterval()) == _numCheckpointsInstantiated)
        {
            StartCoroutine(IncreaseSpeed(GetSpeedIncrease()));
            
            GenerateCheckpoint(_totalDistance + GenerateDistance);
            _numCheckpointsInstantiated++;
        }

        if ((int)(_totalDistance / ObstacleInterval) == _numObstacleIntervalsGenerated)
        {
            GenerateObstacles(_totalDistance + GenerateDistance);
            _numObstacleIntervalsGenerated++;
        }

    }

    private float GetCheckpointInterval()
    {
        // TODO: change this to depend on speed
        return 500f;
    }

    private void GenerateCheckpoint(float location)
    {
        int checkpointX = Random.Range(-75, 75); 
        Instantiate(Resources.Load("Checkpoint"),
            new Vector3(checkpointX, 175, location),
            Quaternion.identity);
    }

    private void GenerateObstacles(float startOfInterval)
    {
        // Generate obstacles in the danger zone
        int numObstaclesPerInterval = 2 + (int)(startOfInterval / GetCheckpointInterval());
        for (int i = 0; i < numObstaclesPerInterval; i++)
        {
            SpawnRandomObstacle(startOfInterval, GetPositionForObstacleInDangerZone);
        }

        // Generate cosmetic obstacles
        int numCosmeticLower = Math.Max(2, (int) penguinXROTransform.position.y / 50);
        for (int i = 0; i < numCosmeticLower; i++)
        {
            SpawnRandomObstacle(startOfInterval, GetPositionForCosmeticObstacleLower);
        }
        
        int numCosmeticHigher = Math.Max(2, (int)(MaxObstacleHeight - penguinXROTransform.position.y) / 50);
        for (int i = 0; i < numCosmeticHigher; i++)
        {
            SpawnRandomObstacle(startOfInterval, GetPositionForCosmeticObstacleHigher);
        }
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
        }
    }

    // Obstacles that the user will probably player
    private Vector3 GetPositionForObstacleInDangerZone(Obstacle obstacleScript, float startOfInterval)
    {
        float x = Random.Range(-150, 150);
        float y = penguinXROTransform.position.y + 
                  Random.Range(obstacleScript.GetSpawnOffsetLowerBound(), obstacleScript.GetSpawnOffsetUpperBound());;
        float z = startOfInterval + Random.Range(0, ObstacleInterval);
        return new Vector3(x, y, z);
    }
    
    // Obstacles that are lower than the player
    private Vector3 GetPositionForCosmeticObstacleLower(Obstacle obstacleScript, float startOfInterval)
    {
        // TODO: maybe this x range is too wide
        float x = Random.Range(-200, 200);
        float y = Random.Range(0, obstacleScript.GetSpawnOffsetLowerBound() + penguinXROTransform.position.y);
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
        return 0.5f;
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
