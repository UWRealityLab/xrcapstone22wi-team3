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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SAVE:height:" + penguinXRORigidbody.transform.position.y);
        Debug.Log("SAVE:speed:" + gliderInfo.speed);
        
        // TODO: change this number to whatever the "ground" level is
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        Physics.Raycast(penguinXRORigidbody.transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask);
        Debug.Log("SAVE:rayDistance:" + hit.distance);
        if (hit.distance <= 2) 
        {
            // we've hit the ground, decay faster and disable gravity
            gliderInfo.speed *= 0.99f;
            penguinXRORigidbody.useGravity = false;
            
            if (!_spawnedCertificate && gliderInfo.speed < 1) 
            {
                // Stop the glider
                _spawnedCertificate = true;
                gliderInfo.speed = 0;
                GameController.Instance.DeactivateGlider();

                // Spawn the certificate w/ high score that takes the user back home
                Transform penguinTransform = penguinXRORigidbody.transform; 
                Vector3 localOffset = new Vector3(0.5f, 1, 1.5f);  // spawn in front and to the right
                Vector3 worldOffset = penguinTransform.rotation * localOffset;
                Vector3 spawnPosition = penguinTransform.position + worldOffset;
                Instantiate(goHomeCertificatePrefab, spawnPosition, penguinTransform.rotation);
            }
        }
        else
        {
            // decay speed to slow down the glider
            gliderInfo.speed *= 0.999f;
        }
    }
    
}
