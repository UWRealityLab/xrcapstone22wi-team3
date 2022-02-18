using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonObstacle : Obstacle
{
    private float _speed;
    private float _height;

    // Start is called before the first frame update
    void Start()
    {
        _speed = (Random.value < .5 ? 1 : -1) * Random.Range(0.3f, 0.6f);
        _height = Random.Range(0.03f, 0.05f);
    }

    // Update is called once per frame
    void Update() {
        float y = Mathf.Sin(Time.time * _speed) * _height;
        transform.position += Vector3.up * y;
    }
    
    public override float GetSpawnOffsetLowerBound()
    {
        return -20f;
    }

    public override float GetSpawnOffsetUpperBound()
    {
        return 100f;
    }

    public override Quaternion GetSpawnRotation()
    {
        return Quaternion.identity;
    }
}
