using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoHomeCertificate : XRGrabInteractable
{

    private bool _goHomeCalled = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_goHomeCalled && isSelected)
        {
            _goHomeCalled = true;
            Invoke("GoHome", 5);
            Debug.Log("Time to go home! " + Time.time);
        }
        Debug.Log("SAVE:time:" + Time.time);
    }

    private void GoHome()
    {
        GameController.Instance.ResetToLaunch();
    }

}
