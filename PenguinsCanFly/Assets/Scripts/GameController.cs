using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public ResetManager resetManager;
    
    public Transform penguinXROTransform;
    
    public GameObject launchController;
    public GameObject glidingController;
    public GameObject landingController;
    public GameObject locomotionSystem;

    private LaunchController _launchScript;
    private GliderInfo _glidingScript;
    
    private static GameController _instance;
    
    // TODO: experiment with this value.
    // After we hit this min height from the ground, the landing sequence starts
    private const float GlidingMinHeight = 10f;

    public static GameController Instance
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

        // Disable everything on awake so that enabling works later
        glidingController.SetActive(false);
        launchController.SetActive(false);
        landingController.SetActive(false);
        locomotionSystem.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _launchScript = launchController.GetComponent<LaunchController>();
        _glidingScript = glidingController.GetComponent<GliderInfo>();
        StartLaunchMode();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = GetDistanceToGround();
        Debug.Log("SAVE:rayDistance:" + distance);
        // TODO: only call this method once
        if (distance <= GlidingMinHeight && !landingController.activeSelf)
        {
            StartLandingMode();
        }
    }

    public float GetDistanceToGround()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        if (Physics.Raycast(penguinXROTransform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            return hit.distance;
        }
        else
        {
            return float.PositiveInfinity;
        }
    }

    public void ResetToLaunch()
    {
        resetManager.FadeAndReset();
    }

    public void StartLaunchMode()
    {
        launchController.SetActive(true);
        glidingController.SetActive(false);
        landingController.SetActive(false);
        locomotionSystem.SetActive(false);
    }

    public void StartGlidingMode()
    {
        // Transfer launch speed to gliding mode
        _glidingScript.extraSpeed = _launchScript.speed - _glidingScript.speed;
        
        launchController.SetActive(false);
        glidingController.SetActive(true);
        landingController.SetActive(false);
        locomotionSystem.SetActive(false);
    }

    public void StartLandingMode()
    {
        Debug.Log("Landing sequence initiated!!");
        launchController.SetActive(false);
        // Don't deactivate glidingController yet since we want the glider to still be visible
        // Don't disable the _glidingScript since we still want to control the speed using it
        _glidingScript.DisableUserControlOfGlider();
        landingController.SetActive(true);
        locomotionSystem.SetActive(false);
    }
    
    public void StartGroundMode()
    {
        // Hides the glider and enables locomotion
        glidingController.SetActive(false);
        locomotionSystem.SetActive(true);
    }
    
}