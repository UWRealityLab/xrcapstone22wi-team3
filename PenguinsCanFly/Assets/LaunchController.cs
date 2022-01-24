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
    
    // TODO: different way of changing speed
    private static float FLAP_SPEED_MULTIPLIER = 3f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int topCount = top.GetHitCount();
        int bottomCount = bottom.GetHitCount();
        Debug.Log("SAVE:flaps: top-" + topCount + " bottom-" + bottomCount + " min-" + Math.Min(topCount, bottomCount));

        // TODO: if you stop flapping, should your speed increase
        Vector3 localV = gliderDirection.InverseTransformDirection(penguinXRORigidbody.velocity);
        localV.z = Math.Min(topCount, bottomCount) * FLAP_SPEED_MULTIPLIER;
        penguinXRORigidbody.velocity = gliderDirection.TransformDirection(localV);
        Debug.Log("SAVE:speed:" + localV.z);
    }

    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = false;
    }
}
