using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (_totalDistance >= startingOffset && 
            (int) ((_totalDistance - startingOffset) / CheckpointDistance) == _numCheckpointsInstantiated)
        {
            GameObject checkpointObject = (GameObject) Instantiate(Resources.Load("Checkpoint"),
                new Vector3(0, 120, _totalDistance + CheckpointDistance),
                Quaternion.identity);
            WindCollider checkpointObjectScript = checkpointObject.GetComponent<WindCollider>();
            checkpointObjectScript.gliderInfo = gliderInfo;
            _numCheckpointsInstantiated++;
        }

    }
}
