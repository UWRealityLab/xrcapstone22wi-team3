using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class DeviceManager : MonoBehaviour
{
    
    public InputDevice rightHandDevice;
    public InputDevice leftHandDevice;

    private static DeviceManager _instance;
    public static DeviceManager Instance
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
    }
    

    void Start()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
        
        // Devices we are actually  using:
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, inputDevices);
        
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("With proper characteristics '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

        if (inputDevices.Count > 0)
        {
            rightHandDevice = inputDevices[0];
        }
        
        InputDeviceCharacteristics leftControllerCharacteristics  =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, inputDevices);
        if (inputDevices.Count > 0)
        {
            leftHandDevice = inputDevices[0];
        }
    }

    public void SendCollisionHaptics()
    {
        leftHandDevice.SendHapticImpulse(0, 1, 0.5f);
        rightHandDevice.SendHapticImpulse(0, 1, 0.5f);
    }
}
