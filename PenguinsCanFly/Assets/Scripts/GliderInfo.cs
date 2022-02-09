using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GliderInfo : MonoBehaviour
{
    // Note, 0 is up, 90 is forward, 180 is down
    public const int MaxPitchOffsetDegree = 40;
    
    public float extraSpeed = 10f;
    public float speed = 12.5f;
    public float drag = 6;

    // TODO: fix this circular dependency
    public GliderModelController gliderModelController;
    
    public Rigidbody penguinXRORigidbody;
    public Transform gliderDirection;

    public Transform penguinXROTransform;

    // Pitch is up/down. Looking straight ahead is pitch 90. Pitch 60 tilts up, pitch 120 tilts down 
    public float pitchDegree = 90f; // This should only be modified by the hand controller, changes "base" pitch
    public float extraPitchDegree = 0f;
    public float TotalPitchDegree
    {
        get { return pitchDegree + extraPitchDegree; }
    }

    public float totalYawDegree;
    
    private InputDevice targetDevice;

    // when userControlEnabled = false, the user cannot control the glider
    public bool userControlEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            // Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
        
        // Devices we are actually  using:
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, inputDevices);
        
        foreach (var device in inputDevices)
        {
            // Debug.Log(string.Format("With proper characteristics '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

        if (inputDevices.Count > 0)
        {
            targetDevice = inputDevices[0];
        }
    }

    void FixedUpdate()
    {
        // Add speed forward based on glider direction
        Vector3 gliderDirectionForward = gliderDirection.forward;
        float modifiedSpeed = speed + extraSpeed;
        penguinXRORigidbody.AddForce(gliderDirectionForward * (modifiedSpeed * 10));
        
        // Reduce extra speed towards 0
        if (extraSpeed > 0)
        {
            extraSpeed *= 0.99f;
        }
        else if (extraSpeed < 0)
        {
            extraSpeed += .05f;
        }
        extraSpeed = Mathf.Clamp(extraSpeed, -0.95f * speed, 3 * speed);
        
        // Reduce extra pitch towards 0
        extraPitchDegree *= 0.9f;
        extraPitchDegree = Mathf.Clamp(extraPitchDegree,-MaxPitchOffsetDegree, MaxPitchOffsetDegree);

        // Depending on pitch, change drag so that if you are looking down, you go faster and vice versa
        // 0.05f was calculated based on -2drag / 40degrees 
        float actualPitchRotation = Vector3.SignedAngle(Vector3.up, gliderDirectionForward, gliderDirection.right);
        float modifiedDrag = -(actualPitchRotation - 90) * 0.05f + drag;
        penguinXRORigidbody.drag = modifiedDrag;
        

        // Debug.Log("SAVE:GliderSpeed:" + penguinXRORigidbody.velocity.magnitude + " ExtraSpeed " + extraSpeed);

        // Yaw camera globally
        Quaternion cameraTargetNewRotation = Quaternion.Euler(0, totalYawDegree, 0);
        penguinXROTransform.rotation = Quaternion.Slerp(penguinXROTransform.rotation, cameraTargetNewRotation, Time.deltaTime);
        
        // Pitch locally off of the camera yaw
        Quaternion gliderTargetNewRotation = Quaternion.Euler(TotalPitchDegree - 90, 0, 0);  // Subtract 90 since looking forward is 90
        gliderDirection.localRotation = Quaternion.Slerp(gliderDirection.localRotation, gliderTargetNewRotation, Time.deltaTime);

        if (userControlEnabled)
        {
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
            if (primaryButtonValue)
            {
                // Debug.Log("Hacker detected!");
                extraPitchDegree = -30;
            }
            else if (secondaryButtonValue)
            {
                extraPitchDegree = 30;
            }
        }
        


        // Debug.Log("SAVE:TotalPitchDegree:" + TotalPitchDegree + " extraPitchDegree " + extraPitchDegree);

    }

    // Update is called once per frame

    // Disable user control of gliding, but still display the hang glider
    // and allow this script to control the glider's speed. Use for landing sequence
    public void DisableUserControlOfGlider()
    {
        // Debug.Log("DISABLE USER CONTROL");
        // Glider model controller checks the userControlEnabled flag to reset to neutral
        userControlEnabled = false;
        extraPitchDegree = 0;
        pitchDegree = 90;
    }
    
    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = true;
    }
}
