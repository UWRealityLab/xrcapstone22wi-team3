using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonObstacle : Obstacle
{
    public override bool ShouldSpawnOnGround()
    {
        return false;
    }

    public override float GetSpawnOffsetLowerBound()
    {
        return -30f;
    }

    public override float GetSpawnOffsetUpperBound()
    {
        return 70f;
    }

    public override Quaternion GetSpawnRotation()
    {
        return Quaternion.identity;
    }
}
