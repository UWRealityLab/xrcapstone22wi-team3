using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGameController : MonoBehaviour
{
    public ResetManager resetManager;

    public GameObject glidingController;
    public GameObject locomotionSystem;

    private LaunchController _launchScript;
    public TutorialGliderInfo gliderInfo;

    private static TutorialGameController _instance;

    public const string MASTER_SCENE_NAME = "MasterScene";
    public const string MENU_SCENE_NAME = "MenuScene";

    public static TutorialGameController Instance
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
        locomotionSystem.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        glidingController.SetActive(false);
        gliderInfo = glidingController.GetComponent<TutorialGliderInfo>();
        StartStandingMode();
    }

    public void ResetToLaunch()
    {
        resetManager.FadeResetToScene(MASTER_SCENE_NAME);
    }

    public void ResetToMenu()
    {
        resetManager.FadeResetToScene(MENU_SCENE_NAME);
    }

    public void StartStandingMode()
    {
        glidingController.SetActive(true);
        locomotionSystem.SetActive(false);
    }
}