using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

using Facebook.WitAi;
using Oculus.Voice;

public class VoiceSystemActivator : MonoBehaviour
{

    [SerializeField] private Wit wit;

    private bool pressed = false;

    // Update is called once per frame
    void Update()
    {
        if (wit == null)
        {
            Debug.Log("trying to get wit");
            wit = GetComponent<Wit>();
        }
        
        Debug.Log("SAVE:wit: " + wit);
        DeviceManager.Instance.leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue != pressed)
        {
            pressed = primaryButtonValue;
            if (pressed)
            {
                Debug.Log("Activating wit");
                wit.Activate();
                Debug.Log("Activated wit");
            }
        }
        
    }
}
