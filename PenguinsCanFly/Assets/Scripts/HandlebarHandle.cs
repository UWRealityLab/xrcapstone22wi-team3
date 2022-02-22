using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class HandlebarHandle : XRBaseInteractable
{
    
    public Transform hangGliderRotatePoint = null;
    
    public float goalZ;

    public FlipperShoulderScript leftShoulder;
    public FlipperShoulderScript rightShoulder;

    // Used for indicating where the flipper should be locked in
    public Transform grabLockPosition;

    // Start is called before the first frame update
    void Start()
    {
        goalZ = -90;
    }

    // Check that a controller is interacting with the object
    private IXRSelectInteractor selectInteractor = null;

    public bool IsBeingHeld()
    {
        return selectInteractor != null;
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("HandlebarHandle: select entered: " + args.interactorObject);
        selectInteractor = args.interactorObject;
        if (selectInteractor.ToString().Contains("Left"))
        {
            leftShoulder.gliderHandlePos = grabLockPosition;
        }
        else
        {
            rightShoulder.gliderHandlePos = grabLockPosition;
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
    
        if (isSelected)
        {
            // We move hand position and right to the same plane defined by the local forward vector and then compute angle
            // to get the angle we want to be rotated
            Vector3 relativePos = selectInteractor.transform.position - hangGliderRotatePoint.position;
            Vector3 projectedVector = Vector3.ProjectOnPlane(relativePos, hangGliderRotatePoint.forward);
            // Note that because it is signed, it will be -180 < val < 180
            float rotationGoal = Vector3.SignedAngle(Vector3.up, projectedVector, hangGliderRotatePoint.forward);
            goalZ = rotationGoal;
        }
    }
    
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        Debug.Log("HandlebarHandle: select exited: " + args.interactorObject);
        if (selectInteractor.ToString().Contains("Left"))
        {
            leftShoulder.gliderHandlePos = null;
        }
        else
        {
            rightShoulder.gliderHandlePos = null;
        }
        selectInteractor = null;
        goalZ = -90;
        
    }
    
}
