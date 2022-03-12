using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GoMenuTutorialScript : XRGrabInteractable
{
    public TextMeshProUGUI returnHomeText;

    public AudioSource successAudioSource;

    private bool _goMenuCalled = false;
    private float _goMenuCallTime = -1f;

    private const float TimeBeforeReset = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_goMenuCalled && isSelected)
        {
            if (SceneManager.GetSceneByName("GliderTutorialScene").isLoaded)
            {
                successAudioSource.Play();
            }

            // Initiate go menu timer
            _goMenuCalled = true;
            _goMenuCallTime = Time.time;
            Invoke("GoMenu", TimeBeforeReset);
        }
        else if (_goMenuCalled)
        {
            // Display go home count down
            int timeLeft = (int)Math.Ceiling(TimeBeforeReset - (Time.time - _goMenuCallTime));
            returnHomeText.text = "Returning home in " + timeLeft;
        }
    }

    private void GoMenu()
    {
        TutorialGameController.Instance.ResetToMenu();
    }

}
