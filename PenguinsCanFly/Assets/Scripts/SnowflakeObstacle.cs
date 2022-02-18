using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowflakeObstacle : Obstacle
{
    private float _rotateOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().drag = 1f + Random.Range(-0.5f, 1f);
        _rotateOffset = Random.Range(-10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate((20f+_rotateOffset) * Vector3.forward * Time.deltaTime);
    }

    public override float GetSpawnOffsetLowerBound()
    {
        return 200f;
    }

    public override float GetSpawnOffsetUpperBound()
    {
        return 0f;
    }
}
