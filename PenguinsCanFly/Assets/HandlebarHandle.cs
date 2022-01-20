using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandlebarHandle : XRBaseInteractable
{
    
    Transform thingToRotate = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Check that a controller is interacting with the object
    private IXRSelectInteractor selectInteractor = null;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("PullMeasurer: select entered: " + args.interactorObject);
        selectInteractor = args.interactorObject;
    }
    
    // public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    // {
    //     base.ProcessInteractable(updatePhase);
    //
    //     if (isSelected)
    //     {
    //         if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
    //         {
    //             CheckForPull();
    //         }
    //     }
    // }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("YEEEET");

    }
}
