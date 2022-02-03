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

    private LaunchController _launchScript;
    private GliderInfo _glidingScript;
    
    private static GameController _instance;
    
    // TODO: experiment with this value.
    // After we hit this min height, the landing sequence starts
    // Assumes that the ground is ar y = 0
    private const float GLIDING_MIN_HEIGHT = 10f;

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
        if (distance <= GLIDING_MIN_HEIGHT)
        {
            Instance.StartLandingMode();
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
    }

    public void StartGlidingMode()
    {
        // Transfer launch speed to gliding mode
        _glidingScript.extraSpeed = _launchScript.speed - _glidingScript.speed;
        
        launchController.SetActive(false);
        glidingController.SetActive(true);
        landingController.SetActive(false);
    }

    public void StartLandingMode()
    {        
        launchController.SetActive(false);
        // Don't deactivate glidingController yet since we want the glider to still be visible
        // Don't disable the _glidingScript since we still want to control the speed using it
        _glidingScript.DisableUserControlOfGlider();
        landingController.SetActive(true);
    }
    public void DeactivateGlider()
    {
        // Hides the glider
        glidingController.SetActive(false);
    }
}