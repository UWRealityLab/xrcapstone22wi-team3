using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreCounter : MonoBehaviour
{
    private float lastPositionZ;
    private float totalDistance;
    
    private static ScoreCounter _instance;

    public static ScoreCounter Instance
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
        lastPositionZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionZ = transform.position.z;
        totalDistance += (newPositionZ - lastPositionZ);
        lastPositionZ = newPositionZ;
        
        Debug.Log("SAVE:score:" + GetScore());
    }

    public int GetScore()
    {
        return (int)Math.Round(totalDistance);
    }
}
