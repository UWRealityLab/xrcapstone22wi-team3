using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingController : MonoBehaviour
{
    public GliderInfo gliderInfo;

    public GameObject goHomeCertificatePrefab;
    
    private bool _spawnedCertificate = false;

    // Distance from the ground where we turn off gravity and decay speed faster
    private const float LandingHeight = 2f;


    private float previousGroundWorldHeight;
    
    private Transform penguinXROTransform;
    private Rigidbody penguinXRORigidbody;

    private bool isLanded;
    
    
    // Start is called before the first frame update
    void Start()
    {
        penguinXRORigidbody = gliderInfo.penguinXRORigidbody;
        penguinXROTransform = gliderInfo.penguinXROTransform;
        isLanded = false;
    }

    private void OnEnable()
    {
        // TODO: Remove - fail safe to spawn the certificate even if get stuck and never land
        // Invoke("SpawnCertificate", 7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLanded)
        {
            return;
        }

        Debug.Log("SAVE:speed:" + gliderInfo.speed);
        
        // TODO: change this number to whatever the "ground" level is
        float distance = GetDistanceToGround();
        Debug.Log("SAVE:LandingCtronllerDistance:" + distance);
        
 
        if (distance <= LandingHeight)
        {
            // Check if speed is slow enough that we should just land and disable gravity
            Debug.Log("SAVE:gliderVelocity:" + penguinXRORigidbody.velocity.magnitude);
            if (gliderInfo.speed == 0 && penguinXRORigidbody.velocity.magnitude < 0.5)
            {
                // TODO: disable gravity but also make it so that it stops applying force up!
                penguinXRORigidbody.useGravity = false;
                isLanded = true;
                GameController.Instance.StartGroundMode();
                SpawnCertificate();
                return;
            }
            
            // Otherwise, move craft up since it is too close to ground!
            // Apply a stronger force up if you are closer to ground
            float upwardForce = 4 * Mathf.Pow(1.8f, -(distance - 3));
            Debug.Log("SAVE:UpwardForce:" + upwardForce);
            penguinXRORigidbody.AddForce(Vector3.up * Physics.gravity.magnitude + Vector3.up * upwardForce);

            // Reduce speed since too close to ground
            float speedToReduce = Math.Max(0.01f, gliderInfo.speed * 0.01f);
            Debug.Log("Reducing speed!" + speedToReduce);
            gliderInfo.speed = Math.Max(0, gliderInfo.speed - speedToReduce);
        }


        // we've hit the ground, decay faster and disable gravity
        // gliderInfo.speed *= 0.99f;
        // penguinXRORigidbody.useGravity = false;
        //
        // if (!_spawnedCertificate && penguinXRORigidbody.velocity.magnitude < 1) 
        // {
        //     GameController.Instance.StartGroundMode();
        //     SpawnCertificate();
        // }


        // Logic for starting actual landing
        // if (gliderInfo.speed < 5)
        // {
        //     GameController.Instance.StartLandingMode();
        // }
    }

    public void SpawnCertificate()
    {
        if (!_spawnedCertificate)
        {
            _spawnedCertificate = true;
            
            // Spawn the certificate w/ high score that takes the user back home
            Transform penguinTransform = penguinXRORigidbody.transform; 
            Vector3 localOffset = new Vector3(0.5f, 1, 1.5f);  // spawn in front and to the right
            Vector3 worldOffset = penguinTransform.rotation * localOffset;
            Vector3 spawnPosition = penguinTransform.position + worldOffset;
            Instantiate(goHomeCertificatePrefab, spawnPosition, penguinTransform.rotation);
        }
    }
    
    public float GetDistanceToGround()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        // Cast ahead of the glider based on the direction and go up so that it doesn't clip through ground
        Vector3 locationToRaycast = penguinXRORigidbody.velocity + penguinXROTransform.position + 50 * Vector3.up;
        if (Physics.Raycast(locationToRaycast, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            // Subtract from the earlier compensation
            return hit.distance - 50;
        }
        else  // TODO: return previous stored distance in case clip through ground
        {
            return float.PositiveInfinity;
        }
    }

}
