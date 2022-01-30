using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject launchController;
    public GameObject glidingController;
    public GameObject landingController;

    private LaunchController _launchScript;
    private GliderInfo _glidingScript;
    
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

    }

    public void ResetToLaunch()
    {
        // TODO: assumes we only have one scene
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
        glidingController.SetActive(false);
        landingController.SetActive(true);
    }
}