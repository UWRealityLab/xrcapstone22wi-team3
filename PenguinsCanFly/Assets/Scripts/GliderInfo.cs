using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GliderInfo : MonoBehaviour
{
    public float speed = 12.5f;
    public float drag = 6;

    // TODO: fix this circular dependency
    public GliderModelController gliderModelController;
    
    public Rigidbody penguinXRORigidbody;
    public Transform gliderDirection;

    public Transform penguinXROTransform;

    // Pitch is up/down. Looking straight ahead is pitch 90. Pitch 60 tilts up, pitch 120 tilts down 
    public float totalPitchDegree;
    public float totalYawDegree;
    
    private InputDevice targetDevice;

    private bool _userControlEnabled = true;

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
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
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
        // Add speed forward based on glider direction
        penguinXRORigidbody.drag = drag;
        Vector3 localV = gliderDirection.InverseTransformDirection(penguinXRORigidbody.velocity);
        localV.z = speed;
        penguinXRORigidbody.velocity = gliderDirection.TransformDirection(localV);
        
        // Yaw camera globally
        Quaternion cameraTargetNewRotation = Quaternion.Euler(0, totalYawDegree, 0);
        penguinXROTransform.rotation = Quaternion.Slerp(penguinXROTransform.rotation, cameraTargetNewRotation, Time.deltaTime);
        
        // Pitch locally off of the camera yaw
        Quaternion gliderTargetNewRotation = Quaternion.Euler(totalPitchDegree - 90, 0, 0);  // Subtract 90 since looking forward is 90
        gliderDirection.localRotation = Quaternion.Slerp(gliderDirection.localRotation, gliderTargetNewRotation, Time.deltaTime);

        if (_userControlEnabled)
        {
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
            if (primaryButtonValue)
            {
                totalPitchDegree = 60;
            }
            else if (secondaryButtonValue)
            {
                totalPitchDegree = 120;
            }
            else
            {
                totalPitchDegree = 90;
            }
        }

    }

    // Disable user control of gliding, but still display the hang glider
    // and allow this script to control the glider's speed. Use for landing sequence
    public void DisableUserControlOfGlider()
    {
        // TODO: fix this circular dependency
        gliderModelController.transform.localRotation = 
            Quaternion.RotateTowards(gliderModelController.transform.localRotation, 
            Quaternion.identity, 0.1f);
        gliderModelController.enabled = false;
        _userControlEnabled = false;
        totalPitchDegree = 90;
    }
    
    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = true;
    }
}
