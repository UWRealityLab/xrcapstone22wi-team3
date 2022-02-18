using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonObstacle : Obstacle
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override float GetSpawnOffsetLowerBound()
    {
        return -20f;
    }

    public override float GetSpawnOffsetUpperBound()
    {
        return 50f;
    }

    public override Quaternion GetSpawnRotation()
    {
        return Quaternion.identity;
    }
}
