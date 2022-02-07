using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingController : MonoBehaviour
{
    public GliderInfo gliderInfo;
    
    public Rigidbody penguinXRORigidbody;
    public GameObject goHomeCertificatePrefab;
    
    private bool _spawnedCertificate = false;

    // Distance from the ground where we turn off gravity and decay speed faster
    private const float LandingHeight = 5f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        // TODO: Remove - fail safe to spawn the certificate even if get stuck and never land
        Invoke("SpawnCertificate", 7f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SAVE:speed:" + gliderInfo.speed);
        
        // TODO: change this number to whatever the "ground" level is
        float distance = GameController.Instance.GetDistanceToGround();
        if (distance <= LandingHeight) 
        {
            // we've hit the ground, decay faster and disable gravity
            gliderInfo.speed *= 0.99f;
            penguinXRORigidbody.useGravity = false;
            
            if (!_spawnedCertificate && penguinXRORigidbody.velocity.magnitude < 1) 
            {
                GameController.Instance.StartGroundMode();
                SpawnCertificate();
            }
        }
        else
        {
            // decay speed to slow down the glider
            gliderInfo.speed *= 0.999f;
        }
    }

    public void SpawnCertificate()
    {
        if (!_spawnedCertificate)
        {
            gliderInfo.speed = 0; // TODO: remove - this is just for the fail safe
            _spawnedCertificate = true;
            
            // Spawn the certificate w/ high score that takes the user back home
            Transform penguinTransform = penguinXRORigidbody.transform; 
            Vector3 localOffset = new Vector3(0.5f, 1, 1.5f);  // spawn in front and to the right
            Vector3 worldOffset = penguinTransform.rotation * localOffset;
            Vector3 spawnPosition = penguinTransform.position + worldOffset;
            Instantiate(goHomeCertificatePrefab, spawnPosition, penguinTransform.rotation);
        }
    }
    
}
