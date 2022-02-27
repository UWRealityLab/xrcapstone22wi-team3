using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowflakeObstacle : Obstacle
{
    private float _rotateOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().drag = Random.Range(0.5f, 2f);
        _rotateOffset = (Random.value < .5 ? 1 : -1) * Random.Range(10f, 30f);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        transform.Rotate((20f+_rotateOffset) * Vector3.forward * Time.deltaTime);
    }
    
    public override bool ShouldSpawnOnGround()
    {
        return false;
    }

    public override float GetSpawnOffsetLowerBound()
    {
        return 50f;
    }

    public override float GetSpawnOffsetUpperBound()
    {
        return 400f;
    }

    public override Quaternion GetSpawnRotation()
    {
        // Choose a random rotation to spawn in
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0,Random.Range(0, 360));
        return rotation;
    }
    
}
