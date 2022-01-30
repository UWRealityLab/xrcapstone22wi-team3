using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private float lastPositionZ;
    private float totalDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        lastPositionZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionZ = transform.position.z;
        totalDistance += (newPositionZ - lastPositionZ);
        lastPositionZ = newPositionZ;
        
        Debug.Log("SAVE:score:" + Math.Round(totalDistance));
    }
}
