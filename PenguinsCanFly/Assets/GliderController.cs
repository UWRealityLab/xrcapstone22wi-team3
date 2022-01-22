using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GliderController : MonoBehaviour
{
    
    public float speed = 12.5f;
    public float drag = 6;
    
    public Rigidbody rb;

    // TODO: This seems like really really bad design, will probably break even if it's working
    public HandlebarHandle handlebar;
    
    
    private InputDevice targetDevice;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
        
        // Devices we are actually  using:
        InputDeviceCharacteristics rightControllerCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, inputDevices);
        
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("With proper characteristics '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

        if (inputDevices.Count > 0)
        {
            targetDevice = inputDevices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Add speed forward
        rb.drag = drag;
        Vector3 localV = transform.InverseTransformDirection(rb.velocity);
        localV.z = speed;
        rb.velocity = transform.TransformDirection(localV);

        // Match the turning of the glider to the handlebar
        Vector3 currentAngle = handlebar.thingToRotate.eulerAngles;
        Vector3 rot = transform.eulerAngles;
        float amountToRotate = currentAngle.z <= handlebar.MAX_ROTATION_DEGREES ? -currentAngle.z : 360 - currentAngle.z;
        rot.y += amountToRotate * Time.deltaTime;
        
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
        {
            Debug.Log("PrimaryButton pressed on target device!");
            Debug.Log("SAVE:xyAngle:" + transform.eulerAngles.x);
            float amountToRotateX = -10 - transform.eulerAngles.x;  
            // rot.x += -5 * Time.deltaTime;  // - goes down, + goes up
            rot.x += -5 * Time.deltaTime;
        }
        else
        {
            // TODO: return to 0 code is actually stupid hard wut LOL
            // // float amountToRotateX = 0 - transform.eulerAngles.x;
            // if (transform.eulerAngles.x < 90)
            // {
            //     rot.x += -transform.eulerAngles.x * Time.deltaTime;
            // }
            // else
            // {
            //     rot.x += 360 - transform.eulerAngles.x * Time.deltaTime;
            // }
            // Debug.Log("SAVE:OurCurrentX:" + transform.eulerAngles.x);
            rot.x = 0;
        }
        
        transform.rotation = Quaternion.Euler(rot);
    }
}
