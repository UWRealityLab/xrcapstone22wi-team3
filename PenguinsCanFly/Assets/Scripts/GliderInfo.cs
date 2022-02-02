using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GliderInfo : MonoBehaviour
{
    public float baseSpeed = 12.5f;
    public float maxSpeed = 20f;
    public float drag = 6;

    // TODO: fix this circular dependency
    public GliderModelController gliderModelController;
    
    public Rigidbody penguinXRORigidbody;
    public Transform gliderDirection;

    public Transform penguinXROTransform;

    // Pitch is up/down. Looking straight ahead is pitch 90. Pitch 60 tilts up, pitch 120 tilts down 
    public float totalPitchDegree;
    public float totalYawDegree;

    public const float PitchRotationTolerance = 5;
    
    private InputDevice targetDevice;

    private bool _userControlEnabled = true;
    
    private float _speed;


    // Start is called before the first frame update
    void Start()
    {
        _speed = baseSpeed;
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
    void FixedUpdate()
    {
        // Add speed forward based on glider direction
        penguinXRORigidbody.drag = drag;
        // Vector3 localV = gliderDirection.InverseTransformDirection(penguinXRORigidbody.velocity);
        // localV.z = speed;
        // penguinXRORigidbody.velocity = gliderDirection.TransformDirection(localV);
        
        // penguinXRORigidbody.AddRelativeForce(Vector3.forward * (speed * 10));

        Vector3 gliderDirectionForward = gliderDirection.forward;

        // float modified_drag = -0.1f * (totalPitchDegree - 90) + drag;
        // float modified_drag = -0.1f * (gliderDirection.localEulerAngles.x - 90) + drag;
        float actualPitchRotation = Vector3.SignedAngle(Vector3.up, gliderDirectionForward, gliderDirection.right);
        // float modified_drag = -0.1f * (actualPitchRotation - 90) + drag;
        // Debug.Log("SAVE:ModifiedDrag:" + modified_drag + " " + actualPitchRotation + " " + gliderDirection.localEulerAngles.x);
        // penguinXRORigidbody.drag = modified_drag;

        // TODO: make sure this doesn't go negative tho, also when flying straight, should have speed feel more constant?

        _speed = baseSpeed;
        
        // Looking down!
        if (actualPitchRotation >  90 + PitchRotationTolerance)
        {
            // _speed += (actualPitchRotation - 90) * 0.001f;
            // _speed = Math.Min(_speed, maxSpeed);
            float modified_drag = -(actualPitchRotation - 90) * 0.05f + drag;
            penguinXRORigidbody.drag = modified_drag;
        }
        else if (actualPitchRotation < 90 - PitchRotationTolerance) // Looking up!
        {
            float modified_drag = (90 - actualPitchRotation) * 0.05f + drag;
            penguinXRORigidbody.drag = modified_drag;
            // _speed += (actualPitchRotation - 90) * 0.001f;
            // _speed = Math.Max(_speed, 5);  // Maybe have the min be something smaller than gravity force essentially
        }

        baseSpeed = _speed;
        
        
        penguinXRORigidbody.AddForce(gliderDirectionForward * (baseSpeed * 10));

        Debug.Log("SAVE:GliderSpeed:" + penguinXRORigidbody.velocity.magnitude + " " + _speed);
        // else
        // {
        //     _speed = baseSpeed + (float) (Math.Sign(_speed - baseSpeed) * Math.Pow(_speed - baseSpeed, 2) * 0.005f);
        // }
        // Decay towards base speed -> decay exponentially based on speed
        // _speed = Math.Max((float) Math.Pow(_speed - baseSpeed, 2) * -0.005f, baseSpeed);


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
                // TODO: Remove this for wind to work!!!!");
                totalPitchDegree = 90;
            }
        }

        Debug.Log("SAVE:TotalPitchDegree:" + totalPitchDegree);

    }

    // Disable user control of gliding, but still display the hang glider
    // and allow this script to control the glider's speed. Use for landing sequence
    public void DisableUserControlOfGlider()
    {
        // TODO: fix this circular dependency
        gliderModelController.transform.localRotation = 
            Quaternion.RotateTowards(gliderModelController.transform.localRotation, 
            Quaternion.identity, 0.2f);
        gliderModelController.enabled = false;
        _userControlEnabled = false;
        totalPitchDegree = 90;
    }
    
    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = true;
    }
}
