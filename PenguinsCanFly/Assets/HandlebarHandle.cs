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
            Vector3 relativePos = selectInteractor.transform.position - hangGliderRotatePoint.position;
            Vector3 projectedVector = Vector3.ProjectOnPlane(relativePos, hangGliderRotatePoint.forward);
            // Note that because it is signed, it will be -180 < val < 180
            float rotationGoal = Vector3.SignedAngle(Vector3.up, projectedVector, hangGliderRotatePoint.forward) + 90;

            // -90 is to the right since angle is measured from up -90 to right
            //     0                                     90
            // 90 -|- -90   after +90 adjustment    180 -|- 0
            //    180                                   270 -45
            if (rotationGoal < -GliderModelController.MAX_ROTATION_DEGREES || rotationGoal > 180)  // Need to custom write for balance
            {
                rotationGoal = -GliderModelController.MAX_ROTATION_DEGREES;
            }
            else
            {
                rotationGoal = Math.Min(GliderModelController.MAX_ROTATION_DEGREES, rotationGoal);
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
    }
}
