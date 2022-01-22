using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

            
            // thingToRotate.RotateAround(rotationPivot.position, thingToRotate.forward, 1);
            // thingToRotate.RotateAround(rotationPivot.position, ); 
            // Quaternion targetRotation = Quaternion.Euler(thingToRotate.rotation.eulerAngles.x,
            //                                              thingToRotate.rotation.eulerAngles.y,
            //                                              selectInteractor.transform.rotation.eulerAngles.z);
            // thingToRotate.rotation = Quaternion.RotateTowards(thingToRotate.rotation, targetRotation, 5);
            //
            // if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            // {
            //     Debug.Log("Degree " + targetRotation.eulerAngles);
            // }

            Vector3 relativePos = selectInteractor.transform.position - thingToRotate.position;
            // Vector3 relativePos = thingToRotate.InverseTransformDirection(selectInteractor.transform.position);
            // Vector3 relativeYPos = new Vector3(relativePos.x, relativePos.y, 0);
            // Vector3 

            Vector3 projectedVector = Vector3.ProjectOnPlane(relativePos, thingToRotate.forward);
            
            // Note that because it is signed, it will be -180 < val < 180
            float angleDifferenceTurn = Vector3.SignedAngle(thingToRotate.right, projectedVector, thingToRotate.forward);
            goalZ = thingToRotate.localEulerAngles.z + angleDifferenceTurn;
            Debug.Log("SAVE:angleDifferenceTurn:" + angleDifferenceTurn);
            Debug.Log("SAVE:goalZValue:" + goalZ);
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
        // Make sure that goalZ is within bounds (TODO: this solution is buggy sadly bc of euler angles...)
        if (goalZ <= 180)
        {
            goalZ = Math.Min(MAX_ROTATION_DEGREES, goalZ);
        }
        else
        {
            goalZ = Math.Max(360 - MAX_ROTATION_DEGREES, goalZ);
        }

        // Rotate handlebar so it matches the position of hand
        Vector3 localAngles = thingToRotate.localEulerAngles;
        Vector3 rot = new Vector3(localAngles.x, localAngles.y, goalZ);
        thingToRotate.localRotation = Quaternion.Slerp(thingToRotate.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        
        // TODO: remove magic numbers
        // Change yaw based on the local rotation so glider actually turns
        if (localAngles.z >= 5 && localAngles.z < MAX_ROTATION_DEGREES)
        {
            gliderController.totalYawDegree -= localAngles.z * Time.deltaTime;
        } else if (localAngles.z <= 355 && localAngles.z >= 360 - MAX_ROTATION_DEGREES)
        {
            gliderController.totalYawDegree += (360 - localAngles.z) * Time.deltaTime;
        }
    }
}
