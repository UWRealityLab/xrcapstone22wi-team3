using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingController : MonoBehaviour
{
    public Rigidbody penguinXRORigidbody;
    public GameObject goHomeCertificatePrefab;
    
    private float _currForce = 2f;
    private bool _spawnedCertificate = false;
    
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
        // TODO: this is a super jank landing
        if (_currForce > 0.4)
        {
            _currForce *= 0.99f;
            penguinXRORigidbody.AddRelativeForce(0, 0, _currForce, ForceMode.VelocityChange);
        }
        else if (!_spawnedCertificate) 
        {
            // Spawn the certificate w/ high score that takes the user back home
            _spawnedCertificate = true;
            Transform penguinTransform = penguinXRORigidbody.transform; 
            Vector3 localOffset = new Vector3(0.5f, 1, 3);  // spawn in front and to the right
            Vector3 worldOffset = penguinTransform.rotation * localOffset;
            Vector3 spawnPosition = penguinTransform.position + worldOffset;
            Instantiate(goHomeCertificatePrefab, spawnPosition, penguinTransform.rotation);
            GameController.Instance.DisableGlider();
        }
    }

    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = false;
    }
}
