using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerObstacle : Obstacle
{
    
    void Start()
    {
        float randomScale = Random.Range(60, 90);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
    
    public override float GetSpawnOffsetLowerBound()
    {
        return -40f;
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
