using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject launchController;
    public GameObject glidingController;

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

    public void StartLaunchMode()
    {
        glidingController.SetActive(false);
        launchController.SetActive(true);
    }

    public void StartGlidingMode()
    {
        _glidingScript.speed = _launchScript.speed;
        glidingController.SetActive(true);
        launchController.SetActive(false);
    }
}