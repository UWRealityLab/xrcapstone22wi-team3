using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperShoulderScript : MonoBehaviour
{
    public Transform controller;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controllerShoulderDiff = controller.position - transform.position;
        transform.forward = controllerShoulderDiff;
        this.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, controller.transform.eulerAngles.z);

        //transform.rotation.SetLookRotation(controllerShoulderDiff);
        //transform.forward = controllerShoulderDiff;
        //Debug.Log("Controller" + controllerShoulderDiff);
        //Debug.Log("Should Rotation" + transform.rotation);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, controller.transform.eulerAngles, )

        //transform.forward = controllerShoulderDiff;

        //// Flip back into right orientation
        //if (transform.rotation.w == 1)
        //{
        //    this.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -transform.eulerAngles.y, controller.transform.eulerAngles.z);
        //} else
        //{
        //    this.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, controller.transform.eulerAngles.z);
        //}
    }
}
