using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandlebarHandle : XRBaseInteractable
{
    
    public Transform thingToRotate = null;
    public float MAX_ROTATION_DEGREES = 35;
    
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
            float amountToRotateZ = - Vector3.SignedAngle(projectedVector, thingToRotate.right, thingToRotate.forward);
            // Check that xyAngle is within bounds
            Vector3 currentAngle = thingToRotate.eulerAngles;
            if (currentAngle.z + amountToRotateZ <= MAX_ROTATION_DEGREES
                || currentAngle.z + amountToRotateZ >= 360 - MAX_ROTATION_DEGREES)
            {
                thingToRotate.Rotate(new Vector3(0, 0, amountToRotateZ));
            }
            // Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, relativeYPos);
            
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                Debug.Log("SAVE:xyAngle:" + amountToRotateZ);
                Debug.Log("SAVE:objectRotation:" + thingToRotate.eulerAngles);

                // Debug.Log("Rotation goal: " + targetRotation.eulerAngles + " Relative pos: " + relativePos);
            }
            // thingToRotate.rotation = Quaternion.Lerp(thingToRotate.rotation, targetRotation, 0.3);
            // Do some smoothing
            // thingToRotate.rotation *= targetRotation;
        }
        
    }
    
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            Debug.Log("HandlebarHandle: select entered: " + args.interactorObject);
            selectInteractor = args.interactorObject;
        }


    // Update is called once per frame
    void Update()
    {
        
    }
}
