using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingController : MonoBehaviour
{
    public Rigidbody penguinXRORigidbody;
    public GameObject goHomeCertificatePrefab;
    
    private float _currForce = 2f;
    private bool spawnedCertificate = false;
    
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
        if (_currForce > 0.02)
        {
            _currForce *= 0.99f;
            penguinXRORigidbody.AddRelativeForce(0, 0, _currForce, ForceMode.VelocityChange);
        }
        else if (!spawnedCertificate) {
            spawnedCertificate = true;
            //Vector3 spawnPosition = penguinXRORigidbody.transform.TransformDirection(Vector3.forward);
            Instantiate(goHomeCertificatePrefab, penguinXRORigidbody.transform.position, penguinXRORigidbody.transform.rotation);
            Debug.Log("SPAWNED");
        }
    }

    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = false;
    }
}
