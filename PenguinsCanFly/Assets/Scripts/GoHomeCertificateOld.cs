using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GoHomeCertificateOld : XRGrabInteractable
{
    public Text display;
    
    private bool _goHomeCalled = false;
    private float _goHomeCallTime = -1f;
    
    private const float TimeBeforeReset = 3f;

    // Start is called before the first frame update
    void Start()
    {
        display.text = "Great flight! :DD\n\nScore: " + ScoreCounter.Instance.GetScore();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: add high score
        if (!_goHomeCalled && isSelected)
        {
            // Initiate go home timer
            _goHomeCalled = true;
            _goHomeCallTime = Time.time;
            Invoke("GoHome", TimeBeforeReset);
        } 
        else if (_goHomeCalled)
        {
            // Display go home count down
            int timeLeft = (int)Math.Ceiling(TimeBeforeReset - (Time.time - _goHomeCallTime));
            display.text = "Great flight! :DD\n\nReturning home in " + timeLeft;
        }
    }

    private void GoHome()
    {
        GameController.Instance.ResetToLaunch();
    }

}
