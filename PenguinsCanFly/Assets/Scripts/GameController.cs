using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public ResetManager resetManager;
    
    public GameObject launchController;
    public GameObject glidingController;
    public GameObject landingController;
    public GameObject locomotionSystem;

    private LaunchController _launchScript;
    public GliderInfo gliderInfo;
    
    private static GameController _instance;
    


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
        gliderInfo = glidingController.GetComponent<GliderInfo>();
        StartLaunchMode();
    }

    public void ResetToLaunch()
    {
        resetManager.FadeResetToLaunch();
    }

    public void ResetToMenu()
    {
        resetManager.FadeResetToMenu();
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
        gliderInfo.extraSpeed = _launchScript.speed - gliderInfo.speed;
        
        launchController.SetActive(false);
        glidingController.SetActive(true);
        landingController.SetActive(true);
        locomotionSystem.SetActive(false);
    }

    public void StartLandingMode()
    {
        Debug.Log("Landing sequence initiated!!");
        launchController.SetActive(false);
        // Don't deactivate glidingController yet since we want the glider to still be visible
        // Don't disable the _glidingScript since we still want to control the speed using it
        gliderInfo.DisableUserControlOfGlider();
        landingController.SetActive(true);
        locomotionSystem.SetActive(false);
    }

    public void DisableGliderController()
    {
        Debug.Log("Disabling gliderModelScontrolle!" + gliderInfo.gliderModelController);
        gliderInfo.DisableUserControlOfGlider();
    }
    
    public void StartGroundMode()
    {
        // Hides the glider and enables locomotion
        glidingController.SetActive(false);
        locomotionSystem.SetActive(false);
    }
    
}