using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowflakeObstacle : MonoBehaviour, IObstacle
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

    public float GetSpawnOffsetLowerBound()
    {
        return 150f;
    }

    public float GetSpawnOffsetUpperBound()
    {
        return 0f;
    }
}
