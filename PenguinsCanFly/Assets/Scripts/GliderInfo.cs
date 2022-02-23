using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Audio;

public class GliderInfo : MonoBehaviour
{

    public float extraSpeed = 10f;
    public float speed = 12.5f;
    public float drag = 6;
    public AudioMixer am;

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

        // Depending on pitch, change drag so that if you are looking down, you go faster and vice versa
        // 0.05f was calculated based on -2drag / 40degrees 
        float actualPitchRotation = Vector3.SignedAngle(Vector3.up, gliderDirectionForward, gliderDirection.right);
        float modifiedDrag = -(actualPitchRotation - 90) * 0.05f + drag;
        penguinXRORigidbody.drag = modifiedDrag;
        // TODO: NEED TO ADDRESS THIS DRAG THING >> CAN BASE IT OFF OF VELOCITY INSTEAD BUT DURING LANDING, VELOCITY REACHES 0 AND IS NOT STABLE
        

        Debug.Log("SAVE:GliderSpeed:" + penguinXRORigidbody.velocity.magnitude + " ExtraSpeed " + extraSpeed);

        // Yaw camera globally
        Quaternion cameraTargetNewRotation = Quaternion.Euler(0, totalYawDegree, 0);
        penguinXROTransform.rotation = Quaternion.Slerp(penguinXROTransform.rotation, cameraTargetNewRotation, Time.deltaTime);
        
        if (userControlEnabled)
        {
            DeviceManager.Instance.rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            DeviceManager.Instance.rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
            
            if (primaryButtonValue)
            {
                Debug.Log("Hacker detected!");
                penguinXRORigidbody.AddForce(Vector3.up * 30);
            }
            else if (secondaryButtonValue)
            {
                penguinXRORigidbody.AddForce(Vector3.down * 30);

            }
        }
        
        float audioPitchIncrease = Mathf.Clamp((speed + extraSpeed) / 20, 0, 1);
        float audioVolume = Mathf.Clamp((speed + extraSpeed) / 15, 0f, 1.5f);
        if (speed < 5)
        {
            Debug.Log("CLAMPING!!!");
            audioVolume = -80f;
            StartCoroutine(StartFade(am, "Volume", 1, -80f));
        }
        else
        {
            am.SetFloat("Volume", audioVolume);
        }
        Debug.Log("SAVE:AdioPitchIncrease, Volume:" + audioPitchIncrease + " " + audioVolume);
        am.SetFloat("Pitch", 1 + audioPitchIncrease);
    }

    // Update is called once per frame

    // Disable user control of gliding, but still display the hang glider
    // and allow this script to control the glider's speed. Use for landing sequence
    public void DisableUserControlOfGlider()
    {
        Debug.Log("DISABLE USER CONTROL");
        // Glider model controller checks the userControlEnabled flag to reset to neutral
        userControlEnabled = false;
    }
    
    private void OnEnable()
    {
        penguinXRORigidbody.useGravity = true;
    }
    
    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
