using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LandingController : MonoBehaviour
{
    public GliderInfo gliderInfo;

    public GameObject goHomeCertificatePrefab;
    public GameObject goHomeTrophyPrefab;
    public GameObject goMenuPrefab;
    
    // Distance from the ground where we turn off gravity and decay speed faster
    public const float LandingHeight = 2f;
    
    private Transform penguinXROTransform;
    private Rigidbody penguinXRORigidbody;

    private bool isLanded = false;
    
    private static LandingController _instance;

    public static LandingController Instance
    {
        get
        {
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        penguinXRORigidbody = gliderInfo.penguinXRORigidbody;
        penguinXROTransform = gliderInfo.penguinXROTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLanded)
        {
            return;
        }

        // TODO: change this number to whatever the "ground" level is
        float distance = GetDistanceToGround();
        Debug.Log("SAVE:LandingCtronllerDistance:" + distance);
        
 
        if (distance <= LandingHeight)
        {
            if (gliderInfo.speed <= 4)
            {
                GameController.Instance.DisableGliderController();
                // TODO: spawn in the penguins who start chasing you!
            }
            // Check if speed is slow enough that we should just land and disable gravity
            Debug.Log("SAVE:gliderVelocity:" + penguinXRORigidbody.velocity.magnitude);
            if (gliderInfo.speed == 0 && penguinXRORigidbody.velocity.magnitude < 0.5)
            {
                // TODO: disable gravity but also make it so that it stops applying force up!
                penguinXRORigidbody.useGravity = false;
                isLanded = true;
                GameController.Instance.StartGroundMode();
                SpawnPostFlightInteractables();
                return;
            }
            
            // Otherwise, move craft up since it is too close to ground!
            // Apply a stronger force up if you are closer to ground
            float upwardForce = 4 * Mathf.Pow(1.8f, -(distance - 3));
            Debug.Log("SAVE:UpwardForce:" + upwardForce);
            penguinXRORigidbody.AddForce(Vector3.up * Physics.gravity.magnitude + Vector3.up * upwardForce);

            // Reduce speed since too close to ground
            float speedToReduce = Math.Max(0.05f, gliderInfo.speed * 0.05f);
            Debug.Log("Reducing speed!" + speedToReduce);
            gliderInfo.speed = Math.Max(0, gliderInfo.speed - speedToReduce);
        }
    }

    public void SpawnPostFlightInteractables()
    {
        SpawnCertificate();
        SpawnMenuObject();
    }

    public void SpawnCertificate()
    {
        // Spawn the certificate w/ high score that takes the user back home
        Transform penguinTransform = gliderInfo.transform;
        Vector3 worldOffset;
        GameObject certificate;
        if (ScoreCounter.Instance.GetScore() > SaveManager.Instance.GetHiScore())
        {
            Vector3 localOffset = new Vector3(0.3f, 0.4f, 0.75f);  // spawn in front and to the right
            worldOffset = penguinTransform.rotation * localOffset;
            certificate = goHomeTrophyPrefab;
        }
        else
        {
            Vector3 localOffset = new Vector3(0.3f, 1f, 0.75f);  // spawn in front and to the right
            worldOffset = penguinTransform.rotation * localOffset;
            certificate = goHomeCertificatePrefab;
        }
        Vector3 spawnPosition = penguinTransform.position + worldOffset;
        Instantiate(certificate, spawnPosition, penguinTransform.rotation);
    }

    public void SpawnMenuObject()
    {
        // Spawn the go back to menu screen
        Transform penguinTransform = gliderInfo.transform;
        Vector3 localOffset = new Vector3(-0.3f, 1f, 0.75f);  // spawn in front and to the left
        Vector3 worldOffset = penguinTransform.rotation * localOffset;
        Vector3 spawnPosition = penguinTransform.position + worldOffset;
        Instantiate(goMenuPrefab, spawnPosition, penguinTransform.rotation);
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
        else
        {
            return float.PositiveInfinity;
        }
    }

}
