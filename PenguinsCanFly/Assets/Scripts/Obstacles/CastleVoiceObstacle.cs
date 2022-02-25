using System;
using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;

public class CastleVoiceObstacle : VoiceObstacle
{
    public GameObject offset;
    // Start is called before the first frame update
    void Start()
    {
        offset.transform.localPosition += Vector3.down * 50;
    }
    

    public void Update()
    {
        base.Update();
        Vector3 localOffset = offset.transform.localPosition;
        offset.transform.localPosition = Vector3.Lerp(localOffset, Vector3.zero, Time.deltaTime);
    }
    
    public void OnEnable()
    {
        throw new NotImplementedException();
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
