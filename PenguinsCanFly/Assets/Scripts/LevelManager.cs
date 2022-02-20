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

    private int GetNumCheckpointsPerInterval(float distance)
    {
       return 2 + (int)(distance / GetCheckpointInterval());
    }

    private void GenerateObstacles(float startOfInterval)
    {
        Debug.Log("Generated obstacles for " + startOfInterval + "num: " + _numObstacleIntervalsGenerated);
        
        // Generate cosmetic obstacles

        // Generate obstacles in the danger zone
        for (int i = 0; i < GetNumCheckpointsPerInterval(startOfInterval); i++)
        {
            SpawnRandomObstacle(startOfInterval, GetPositionForObstacleInDangerZone);
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

    private Vector3 GetPositionForObstacleInDangerZone(Obstacle obstacleScript, float startOfInterval)
    {
        float x = Random.Range(-150, 150);
        float y = penguinXROTransform.position.y + 
                  Random.Range(obstacleScript.GetSpawnOffsetLowerBound(), obstacleScript.GetSpawnOffsetUpperBound());;
        float z = startOfInterval + Random.Range(0, ObstacleInterval);
        return new Vector3(x, y, z);
    }

    private float GetSpeedIncrease()
    {
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
