using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudObstacle : Obstacle
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public override bool ShouldSpawnOnGround()
    {
        return false;
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
