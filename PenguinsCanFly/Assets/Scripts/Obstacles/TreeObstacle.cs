using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObstacle : Obstacle
{
    void Start()
    {
        float randomScale = Random.Range(30, 50);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
    
    public override bool ShouldSpawnOnGround()
    {
        return true;
    }
    
    public override float GetSpawnOffsetLowerBound()
    {
        throw new System.NotSupportedException();
    }

    public override float GetSpawnOffsetUpperBound()
    {
        throw new System.NotSupportedException();
    }

    public override Quaternion GetSpawnRotation()
    {
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        return rotation;
    }
}
