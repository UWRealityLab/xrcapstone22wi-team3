using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Audio;

public class TutorialGliderInfo : MonoBehaviour
{
    // TODO: fix this circular dependency
    public GliderModelController gliderModelController;

    public Rigidbody penguinXRORigidbody;
    public Transform gliderDirection;

    public Transform penguinXROTransform;

    public float ActualSpeed
    {
        get { return penguinXRORigidbody.velocity.magnitude; }
    }

    public float totalYawDegree;

    // when userControlEnabled = false, the user cannot control the glider
    public bool userControlEnabled = true;

    void FixedUpdate()
    {        

        // Depending on pitch, change drag so that if you are looking down, you go faster and vice versa
        // 0.05f was calculated based on -2drag / 40degrees 
        float actualPitchRotation = Vector3.SignedAngle(Vector3.up, penguinXRORigidbody.velocity.normalized, gliderDirection.right);

        // Yaw camera globally
        //Quaternion cameraTargetNewRotation = Quaternion.Euler(0, totalYawDegree, 0);
        //penguinXROTransform.rotation = Quaternion.Slerp(penguinXROTransform.rotation, cameraTargetNewRotation, Time.deltaTime);

        if (userControlEnabled)
        {
            DeviceManager.Instance.rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            DeviceManager.Instance.rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

            if (primaryButtonValue)
            {
                penguinXRORigidbody.AddForce(Vector3.up * 0.75f);
            }
            else if (secondaryButtonValue)
            {
                penguinXRORigidbody.AddForce(Vector3.down * 0.3f);

            }
        }

      
    }

    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = false;
    }
}
