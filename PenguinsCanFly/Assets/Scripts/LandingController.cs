using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingController : MonoBehaviour
{
    public Rigidbody penguinXRORigidbody;

    private float _currForce = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.Log("SAVE:landing:" + penguinXRORigidbody.velocity + " " + _currForce);
        if (_currForce > 0.01)
        {
            _currForce *= 0.99f;
            penguinXRORigidbody.AddRelativeForce(0, 0, _currForce, ForceMode.VelocityChange);
        }
        else
        {
            Debug.Log("done");
        }
    }

    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = false;
    }
}
