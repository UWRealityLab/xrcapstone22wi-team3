using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;

public class CastleVoiceObstacle : VoiceObstacle
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override float GetSpawnOffsetLowerBound()
    {
        throw new System.NotImplementedException();
    }

    public override float GetSpawnOffsetUpperBound()
    {
        throw new System.NotImplementedException();
    }

    public override Quaternion GetSpawnRotation()
    {
        throw new System.NotImplementedException();
    }
    
}
