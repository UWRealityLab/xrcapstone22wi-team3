using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GliderController : MonoBehaviour
{
    
    public float speed = 12.5f;
    public float drag = 6;

    public float DEGREE_ROTATION_SPEED = 5;
    
    public Rigidbody rb;
    public Transform gliderDirection;

    // TODO: This seems like really really bad design, will probably break even if it's working
    public HandlebarHandle handlebar;

    public float totalPitchDegree;
    public float totalYawDegree;
    
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
        // Add speed forward based on glider direction
        rb.drag = drag;
        Vector3 localV = gliderDirection.InverseTransformDirection(rb.velocity);
        localV.z = speed;
        rb.velocity = gliderDirection.TransformDirection(localV);
        
        // Yaw camera globally
        float totalYawRadian = (float) (totalYawDegree * Math.PI / 180);
        Vector3 cameraTarget = new Vector3(Mathf.Sin(totalYawRadian), 0, Mathf.Cos(totalYawRadian));
        Quaternion cameraTargetNewRotation = Quaternion.LookRotation(cameraTarget, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraTargetNewRotation, Time.deltaTime);
        
        // Pitch locally off of the camera yaw
        float totalPitchRadian = (float) (totalPitchDegree * Math.PI / 180);
        Vector3 gliderTarget = new Vector3(0, Mathf.Cos(totalPitchRadian), Mathf.Sin(totalPitchRadian));
        Quaternion gliderTargetNewRotation = Quaternion.LookRotation(gliderTarget, Vector3.up);
        gliderDirection.localRotation = Quaternion.Slerp(gliderDirection.localRotation, gliderTargetNewRotation, Time.deltaTime);



        // Quaternion newRotation = Quaternion.LookRotation(target, Vector3.up);
        // gliderDirection.rotation = Quaternion.Slerp(gliderDirection.rotation, newRotation, Time.deltaTime);
            
        // Vector3 target = new Vector3(Mathf.Sin(totalPitchRadian) * Mathf.Sin(totalYawRadian),
        //                              Mathf.Cos(totalPitchRadian),
        //                              Mathf.Sin(totalPitchRadian) * Mathf.Cos(totalYawRadian));
        // Debug.Log("SAVE:Penguin Origin Aiming:" + target);
        // Quaternion newRotation = Quaternion.LookRotation(target, Vector3.up);
        // gliderDirection.rotation = Quaternion.Slerp(gliderDirection.rotation, newRotation, Time.deltaTime);
        // Debug.Log("SAVE:CameraRotation:" + gliderDirection.rotation.eulerAngles + " z should ALWAYS be 0!" );

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
        {
            totalPitchDegree = 60;
        }
        else
        {
            totalPitchDegree = 90;
        }

        return;

        // Match the turning of the glider to the handlebar
        Vector3 currentAngle = handlebar.thingToRotate.eulerAngles;
        Vector3 rot = transform.eulerAngles;
        // float amountToRotate = currentAngle.z <= handlebar.MAX_ROTATION_DEGREES ? -currentAngle.z : 360 - currentAngle.z;
        float amountToRotate = handlebar.goalZ;
        // rot.y += amountToRotate * Time.deltaTime;
        rot.y += amountToRotate;
        
        // targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
        {
            Debug.Log("PrimaryButton pressed on target device!");
            Debug.Log("SAVE:xyAngle:" + transform.eulerAngles.x);
            float amountToRotateX = -10 - transform.eulerAngles.x;  
            // rot.x += -5 * Time.deltaTime;  // - goes down, + goes up
            // rot.x += -5 * Time.deltaTime;
            // rot.x += -5;
            rot.x = 330;
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
        Debug.Log("SAVE:XYZ:" + transform.eulerAngles);

        // transform.rotation = Quaternion.Euler(rot);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime);
    }
}
