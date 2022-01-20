using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderController : MonoBehaviour
{
    
    public float speed = 12.5f;
    public float drag = 6;
    
    public Rigidbody rb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.drag = drag;
        Vector3 localV = transform.InverseTransformDirection(rb.velocity);
        localV.z = speed;
        rb.velocity = transform.TransformDirection(localV);
    }
}
