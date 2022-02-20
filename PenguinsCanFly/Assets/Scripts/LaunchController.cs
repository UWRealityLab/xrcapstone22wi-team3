using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchController : MonoBehaviour
{
    public GestureGroup top;
    public GestureGroup bottom;
    
    public Rigidbody penguinXRORigidbody;
    public Transform gliderDirection;

    public float speed = 0f;
    
    // TODO: different way of changing speed
    private const float FLAP_SPEED_MULTIPLIER = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int topCount = top.GetHitCount();
        int bottomCount = bottom.GetHitCount();

        // TODO: if you stop flapping, should your speed increase
        speed = Math.Min(topCount, bottomCount) * FLAP_SPEED_MULTIPLIER;
        
        Vector3 localV = gliderDirection.InverseTransformDirection(penguinXRORigidbody.velocity);
        localV.z = speed;
        penguinXRORigidbody.velocity = gliderDirection.TransformDirection(localV);
    }

    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = false;
    }
}
