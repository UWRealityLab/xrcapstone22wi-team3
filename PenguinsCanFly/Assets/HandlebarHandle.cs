using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class HandlebarHandle : XRBaseInteractable
{
    
    public Transform thingToRotate = null;
    public float MAX_ROTATION_DEGREES = 35;

    public GliderController gliderController = null;

    public float goalZ = 0;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Check that a controller is interacting with the object
    private IXRSelectInteractor selectInteractor = null;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("HandlebarHandle: select entered: " + args.interactorObject);
        selectInteractor = args.interactorObject;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
    
        if (isSelected)
        {
            // We move hand position and right to the same plane defined by the local forward vector and then compute angle
            // to get the angle we want to be rotated
            Vector3 relativePos = selectInteractor.transform.position - thingToRotate.position;
            Vector3 projectedVector = Vector3.ProjectOnPlane(relativePos, thingToRotate.forward);
            // Note that because it is signed, it will be -180 < val < 180
            float rotationGoal = Vector3.SignedAngle(Vector3.up, projectedVector, thingToRotate.forward) + 90;

            // -90 is to the right since angle is measured from up -90 to right
            //     0                                     90
            // 90 -|- -90   after +90 adjustment    180 -|- 0
            //    180                                   270 -45
            if (rotationGoal < -MAX_ROTATION_DEGREES || rotationGoal > 180)  // Need to custom write for balance
            {
                rotationGoal = -MAX_ROTATION_DEGREES;
            }
            else
            {
                rotationGoal = Math.Min(MAX_ROTATION_DEGREES, rotationGoal);
            }

            goalZ = rotationGoal;
            Debug.Log("SAVE:goalZ:" + goalZ);
        }
    }
    
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        Debug.Log("HandlebarHandle: select exited: " + args.interactorObject);
        selectInteractor = null;
        goalZ = 0;
    }


    // Update is called once per frame
    void Update()
    {
        // Rotate handlebar so it matches the position of hand
        Vector3 localAngles = thingToRotate.localEulerAngles;
        
        // Uncomment and use this instead to not rotate with pitch
        // Vector3 rot = new Vector3(localAngles.x, localAngles.y, goalZ);  
        
        // TODO: this might not be smooth since it pretends the axis are independent when they are not. 
        Vector3 rot = new Vector3(0.75f * (gliderController.totalPitchDegree - 90), 0, goalZ);  

        Debug.Log("SAVE:Is this number always 0:" + rot.y);
        thingToRotate.localRotation = Quaternion.Slerp(thingToRotate.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        
        // Below is reference code for if we want to snap handlebar to hand
        // Quaternion smoothTiltRotation = Quaternion.Slerp(thingToRotate.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        // thingToRotate.localRotation = Quaternion.Euler(smoothTiltRotation.eulerAngles.x, smoothTiltRotation.eulerAngles.y, goalZ);
        
        // TODO: remove magic numbers
        // Change yaw based on the local rotation so glider actually turns
        if (localAngles.z >= 5 && localAngles.z <= MAX_ROTATION_DEGREES)
        {
            gliderController.totalYawDegree -= localAngles.z * Time.deltaTime;
        } else if (localAngles.z <= 355 && localAngles.z >= 360 - MAX_ROTATION_DEGREES)
        {
            gliderController.totalYawDegree += (360 - localAngles.z) * Time.deltaTime;
        }
    }
}
