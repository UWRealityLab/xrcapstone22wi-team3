using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public GliderInfo gliderInfo;  // Used for winds
    public Transform penguinXROTransform;
    
    private float _lastPositionZ;
    private float _totalDistance;
    
    // TODO: reference value from terrain manager?
    private float startingOffset = 550;

    private const float CheckpointDistance = 500;
    private int _numCheckpointsInstantiated;
    
    private const float ObstacleCheckRadius = 10f;
    private int _maxSpawnAttemptsPerObstacle = 10;  // to prevent infinite loop

    public GameObject[] obstacleTypes;

    // Start is called before the first frame update
    void Start()
    {
        _lastPositionZ = penguinXROTransform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionZ = penguinXROTransform.position.z;
        _totalDistance += (newPositionZ - _lastPositionZ);
        _lastPositionZ = newPositionZ;
        
        // Spawn checkpoints
        //int numSegments = (int)((_totalDistance - startingOffset) / CheckpointDistance);
        int numSegments = (int)((_totalDistance) / CheckpointDistance);
        if (// _totalDistance >= startingOffset &&
            numSegments == _numCheckpointsInstantiated)
        {
            StartCoroutine(IncreaseSpeed(getSpeedIncrease()));
            
            int checkpointX = Random.Range(-75, 75); 
            GameObject checkpointObject = (GameObject) Instantiate(Resources.Load("Checkpoint"),
                new Vector3(checkpointX, 175, _totalDistance + CheckpointDistance),
                Quaternion.identity);
            Checkpoint checkpointObjectScript = checkpointObject.GetComponent<Checkpoint>();
            checkpointObjectScript.gliderInfo = gliderInfo;
            _numCheckpointsInstantiated++;

            int numObstaclesSpawned = 0;
            for (int i = 0; i < 10 + numSegments*2; i++)
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
                    float x;
                    if (i < 2)
                    {
                        // Make sure we get enough in the middleish
                        x = Random.Range(-25, 25);
                    }
                    else
                    {
                        x = Random.Range(-150, 150);
                    }
                    float y = penguinXROTransform.position.y + Random.Range(obstacleScript.GetSpawnOffsetLowerBound(), obstacleScript.GetSpawnOffsetUpperBound());;
                    float z = _totalDistance + Random.Range(5, CheckpointDistance - 5);
                    position = new Vector3(x, y, z);

                    // Collect all colliders within our Obstacle Check Radius
                    Collider[] colliders = Physics.OverlapSphere(position, ObstacleCheckRadius);

                    validPosition = colliders.Length == 0;
                }
                
                if(validPosition)
                {
                    // Spawn the obstacle here
                    numObstaclesSpawned++;
                    
                    Instantiate(obstacle,
                        position,
                        obstacleScript.GetSpawnRotation());
                    // WindCollider obstacleScript = obstacle.GetComponent<WindCollider>();
                    // obstacleScript.gliderInfo = gliderInfo;
                }
            }
            Debug.Log("Num obstacles spawned:" + numObstaclesSpawned);
        }

    }

    private float getSpeedIncrease()
    {
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
            gliderInfo.speed += 0.01f * speedIncrease;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
