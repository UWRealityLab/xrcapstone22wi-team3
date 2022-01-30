using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GoHomeCertificate : XRGrabInteractable
{
    public Text display;
    
    private bool _goHomeCalled = false;
    private float _goHomeCallTime = -1f;
    
    private const float TimeBeforeReset = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: add high score
        display.text = "Great flight! :DD";
        if (!_goHomeCalled && isSelected)
        {
            _goHomeCalled = true;
            _goHomeCallTime = Time.time;
            Invoke("GoHome", TimeBeforeReset);
            Debug.Log("Time to go home! " + Time.time);
        } 
        else if (_goHomeCalled)
        {
            int timeLeft = (int)Math.Ceiling(TimeBeforeReset - (Time.time - _goHomeCallTime));
            display.text = "Great flight! :DD\n\nReturning home in " + timeLeft;
        }
    }

    private void GoHome()
    {
        GameController.Instance.ResetToLaunch();
    }

}
