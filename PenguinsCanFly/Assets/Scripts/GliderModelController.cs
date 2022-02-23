using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GliderModelController : MonoBehaviour
{
    public const int MaxPitchOffsetDegree = 40;
    private const float MaxRotationDegrees = 35;

    public GliderInfo gliderInfo = null;
    public HandlebarHandle leftHandlebar = null;
    public HandlebarHandle rightHandlebar = null;

    // Update is called once per frame
    void Update()
    {
        if (!gliderInfo.userControlEnabled)
        {
            ResetGliderToNeutral();
            return;
        }
        
        float clampedRightGoalZ = clampRightHand();
        float clampedLeftGoalZ = clampLeftHand();

        // TODO: find better way to merge!!! PRETTY IMPACTFUL CHANGE
        // One alternative is that you can make a line between teh two lines and run that angle instead
        // Can also require both hands have to be grabbing for it to work
        float goalRotation = (clampedRightGoalZ + clampedLeftGoalZ) / 2;

        // Rotate handlebar so it matches the position of hand
        Vector3 localAngles = transform.localEulerAngles;

        // Tilt glider up and down based on direction of flight!
        Vector3 gliderDirectionForward = gliderInfo.penguinXRORigidbody.velocity.normalized;
        float goalPitch = Vector3.SignedAngle(Vector3.up, gliderDirectionForward, gliderInfo.gliderDirection.right) - 90;
        goalPitch = Mathf.Clamp(goalPitch, -MaxPitchOffsetDegree, MaxPitchOffsetDegree);
        
        Vector3 rot = new Vector3(goalPitch, 0, goalRotation);
        

        Quaternion finalLocalRotation =
            Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        // Snap bar quickly if BOTH hands are on it, otherwise, you don't have as much control
        if (leftHandlebar.IsBeingHeld() && rightHandlebar.IsBeingHeld())
        {
            finalLocalRotation = Quaternion.Euler(finalLocalRotation.eulerAngles.x,
                finalLocalRotation.eulerAngles.y, goalRotation);
        }

        transform.localRotation = finalLocalRotation;

        // TODO: remove magic numbers
        // Change yaw based on the local rotation so glider actually turns
        if (localAngles.z >= 5 && localAngles.z <= MaxRotationDegrees)
        {
            gliderInfo.totalYawDegree -= localAngles.z * Time.deltaTime;
        }
        else if (localAngles.z <= 355 && localAngles.z >= 360 - MaxRotationDegrees)
        {
            gliderInfo.totalYawDegree += (360 - localAngles.z) * Time.deltaTime;
        }

        // Change pitch based on local rotation so you can tilt down
        // Only allow this to work if the hand is holding on
        float rightTriggerValue = 0;
        float leftTriggerValue = 0;
        if (leftHandlebar.IsBeingHeld())
        {
            DeviceManager.Instance.rightHandDevice.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerValue);
        }

        if (rightHandlebar.IsBeingHeld())
        {
            DeviceManager.Instance.leftHandDevice.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerValue);
        }

        float averageForceDownPercent = (rightTriggerValue + leftTriggerValue) / 2;
        gliderInfo.penguinXRORigidbody.AddForce(Vector3.down * 30 * averageForceDownPercent);
    }

    private float clampRightHand()
    {
        float rotationGoal = rightHandlebar.goalZ + 90;
        // -90 is to the right since angle is measured from up -90 to right
        //     0                                     90
        // 90 -|- -90   after +90 adjustment    180 -|- 0
        //    180                                   270 -45
        // Should we turn right?
        if (rotationGoal < -MaxRotationDegrees || rotationGoal > 180)
        {
            rotationGoal = -MaxRotationDegrees;
        }
        else  // Turn left!
        {
            rotationGoal = Math.Min(MaxRotationDegrees, rotationGoal);
        }

        return rotationGoal;
    }

    private float clampLeftHand()
    {
        float rotationGoal = leftHandlebar.goalZ + 90;
        // -90 is to the right since angle is measured from up -90 to right
        //     0                                     90
        // 90 -|- -90   after +90 adjustment    180 -|- 0
        //    180                                   270 -45
        // Special case since resting position of rotation is opposite:
        if (rotationGoal == 0)
        {
            return 0;
        }
        // When should we turn right?
        if (180 > rotationGoal && rotationGoal > 0)
        {
            if (Math.Abs(180 - rotationGoal) > MaxRotationDegrees)
            {
                return -MaxRotationDegrees;
            }
            return -(180 - rotationGoal);
        }
        else  // Turn left!
        {
            if (Math.Abs(180 - rotationGoal) > MaxRotationDegrees)
            {
                return MaxRotationDegrees;
            }
            return rotationGoal - 180;
        }
    }

    private void ResetGliderToNeutral()
    {
        Vector3 rot = new Vector3(0, 0, 0);
        Quaternion finalLocalRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        transform.localRotation = finalLocalRotation;
    }
}
