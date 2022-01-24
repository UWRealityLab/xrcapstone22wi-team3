using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderModelController : MonoBehaviour
{
    public static float MAX_ROTATION_DEGREES = 35;

    public GliderInfo gliderInfo = null;
    public HandlebarHandle leftHandlebar = null;
    public HandlebarHandle rightHandlebar = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SAVE:RightHandGoalZ:" + rightHandlebar.goalZ);
        Debug.Log("SAVE:LeftHandGoalZ:" + leftHandlebar.goalZ);

        float clampedRightGoalZ = clampRightHand();
        float clampedLeftGoalZ = clampLeftHand();
        Debug.Log("SAVE:RightHandGoalZClamped:" + clampedRightGoalZ);
        Debug.Log("SAVE:LeftHandGoalZClamped:" + clampedLeftGoalZ);

        // TODO: find better way to merge!!! PRETTY IMPACTFUL CHANGE
        // One alternative is that you can make a line between teh two lines and run that angle instead
        // Can also require both hands have to be grabbing for it to work
        float goalRotation = (clampedRightGoalZ + clampedLeftGoalZ) / 2;

        // Rotate handlebar so it matches the position of hand
        Vector3 localAngles = transform.localEulerAngles;
        
        // Uncomment and use this instead to not rotate with pitch
        // Vector3 rot = new Vector3(localAngles.x, localAngles.y, goalZ);  
        
        // TODO: this might not be smooth since it pretends the axis are independent when they are not. 
        Vector3 rot = new Vector3(0.75f * (gliderInfo.totalPitchDegree - 90), 0, goalRotation);  

        Debug.Log("SAVE:Is this number always 0:" + rot.y);
        // 
        // transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        
        // Below is reference code for if we want to snap handlebar to hand
        Quaternion finalLocalRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rot), Time.deltaTime);

        // If we want the bar to snap faster to hand movement
        // Current setup: only snap quickly if BOTH hands are on it, otherwise, you don't have as much control
        if (leftHandlebar.IsBeingHeld() && rightHandlebar.IsBeingHeld())
        {
            finalLocalRotation = Quaternion.Euler(finalLocalRotation.eulerAngles.x, finalLocalRotation.eulerAngles.y, goalRotation);
        }

        transform.localRotation = finalLocalRotation;
        
        // TODO: remove magic numbers
        // Change yaw based on the local rotation so glider actually turns
        if (localAngles.z >= 5 && localAngles.z <= MAX_ROTATION_DEGREES)
        {
            gliderInfo.totalYawDegree -= localAngles.z * Time.deltaTime;
        } else if (localAngles.z <= 355 && localAngles.z >= 360 - MAX_ROTATION_DEGREES)
        {
            gliderInfo.totalYawDegree += (360 - localAngles.z) * Time.deltaTime;
        }
    }

    private float clampRightHand()
    {
        float rotationGoal = rightHandlebar.goalZ + 90;
        // -90 is to the right since angle is measured from up -90 to right
        //     0                                     90
        // 90 -|- -90   after +90 adjustment    180 -|- 0
        //    180                                   270 -45
        // Should we turn right?
        if (rotationGoal < -MAX_ROTATION_DEGREES || rotationGoal > 180)
        {
            rotationGoal = -MAX_ROTATION_DEGREES;
        }
        else  // Turn left!
        {
            rotationGoal = Math.Min(MAX_ROTATION_DEGREES, rotationGoal);
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
            if (Math.Abs(180 - rotationGoal) > MAX_ROTATION_DEGREES)
            {
                return -MAX_ROTATION_DEGREES;
            }
            return -(180 - rotationGoal);
        }
        else  // Turn left!
        {
            if (Math.Abs(180 - rotationGoal) > MAX_ROTATION_DEGREES)
            {
                return MAX_ROTATION_DEGREES;
            }
            return rotationGoal - 180;
        }
    }
}
