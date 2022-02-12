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
    private InputDevice targetDevice;

    [SerializeField] private Wit wit;

    private bool pressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
        
        // Devices we are actually  using:
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, inputDevices);
        
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("With proper characteristics '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

        if (inputDevices.Count > 0)
        {
            targetDevice = inputDevices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (wit == null)
        {
            Debug.Log("trying to get wit");
            wit = GetComponent<Wit>();
        }
        
        Debug.Log("SAVE:wit: " + wit);
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
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
