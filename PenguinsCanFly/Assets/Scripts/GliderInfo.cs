using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GliderInfo : MonoBehaviour
{
    // TODO: experiment with this value.
    // After we hit this min height, gliding disables and landing starts
    // Assumes that the ground is ar y = 0
    private const float MIN_HEIGHT = 5f;
    
    public float speed = 12.5f;
    public float drag = 6;
    
    public Rigidbody penguinXRORigidbody;
    public Transform gliderDirection;

    public Transform penguinXROTransform;

    // Pitch is up/down. Looking straight ahead is pitch 90. Pitch 60 tilts up, pitch 120 tilts down 
    public float totalPitchDegree;
    public float totalYawDegree;
    
    private InputDevice targetDevice;

    
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
        // TODO: this code should maybe move to GameController
        Debug.Log("SAVE:height:" + penguinXROTransform.position.y);
        if (penguinXROTransform.position.y <= MIN_HEIGHT)
        {
            GameController.Instance.StartLandingMode();
        }
        
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
    
    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = true;
    }
}
