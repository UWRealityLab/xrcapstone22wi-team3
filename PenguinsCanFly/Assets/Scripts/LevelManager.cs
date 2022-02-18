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
    
    public float obstacleCheckRadius = 10f;
    public int maxSpawnAttemptsPerObstacle = 10;  // to prevent infinite loop

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
        int numSegments = (int)((_totalDistance - startingOffset) / CheckpointDistance);
        if (_totalDistance >= startingOffset && 
            numSegments == _numCheckpointsInstantiated)
        {
            int checkpointX = Random.Range(-75, 75); 
            GameObject checkpointObject = (GameObject) Instantiate(Resources.Load("Checkpoint"),
                new Vector3(checkpointX, 120, _totalDistance + CheckpointDistance),
                Quaternion.identity);
            Checkpoint checkpointObjectScript = checkpointObject.GetComponent<Checkpoint>();
            checkpointObjectScript.gliderInfo = gliderInfo;
            _numCheckpointsInstantiated++;

            int numObstaclesSpawned = 0;
            for (int i = 0; i < 10 + numSegments*5; i++)
            {
                Vector3 position = Vector3.zero;
                bool validPosition = false;
                int spawnAttempts = 0;
 
                // While we don't have a valid position and we haven't tried spawning this obstacle too many times
                while(!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
                {
                    spawnAttempts++;
 
                    // Pick a random position
                    float x;
                    if (i < 2)
                    {
                        // Make sure we get enough in the middleish
                        x = Random.Range(-50, 50);
                    }
                    else
                    {
                        x = Random.Range(-150, 150);
                    }
                    float y = penguinXROTransform.position.y;
                    float z = _totalDistance + Random.Range(5, CheckpointDistance - 5);
                    position = new Vector3(x, y, z);

                    // Collect all colliders within our Obstacle Check Radius
                    Collider[] colliders = Physics.OverlapSphere(position, obstacleCheckRadius);

                    validPosition = colliders.Length == 0;
                }
                
                if(validPosition)
                {
                    // Spawn the obstacle here
                    numObstaclesSpawned++;
                    GameObject obstacle = (GameObject) Instantiate(Resources.Load("Obstacle"),
                        position,
                        Quaternion.identity);
                    WindCollider obstacleScript = obstacle.GetComponent<WindCollider>();
                    obstacleScript.gliderInfo = gliderInfo;
                }
            }
            Debug.Log("Num obstacles spawned:" + numObstaclesSpawned);
        }

    }
}
