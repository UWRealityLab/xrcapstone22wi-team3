using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform penguinXROTransform;
    
    public GameObject launchController;
    public GameObject glidingController;
    public GameObject landingController;

    public Animator fadeAnimator;

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
        if (penguinXROTransform.position.y <= GLIDING_MIN_HEIGHT)
        {
            Instance.StartLandingMode();
        }
    }

    public void ResetToLaunch()
    {
        // TODO: assumes we only have one scene
        fadeAnimator.SetTrigger("FadeTransition");
        // TODO: fade out animation doesn't trigger because scene loads too soon
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartLaunchMode();;
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
        _glidingScript.speed = _launchScript.speed;
        
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