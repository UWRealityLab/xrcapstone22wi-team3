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
        _height = Random.Range(0.04f, 0.07f);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        float y = Mathf.Sin(Time.time * _speed) * _height;
        transform.position += Vector3.up * y;
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
