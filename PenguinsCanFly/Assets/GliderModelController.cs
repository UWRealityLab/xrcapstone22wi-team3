using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderModelController : MonoBehaviour
{
    public static float MAX_ROTATION_DEGREES = 35;

    public GliderInfo gliderInfo = null;
    public HandlebarHandle rightHandlebar = null;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        // Rotate handlebar so it matches the position of hand
        Vector3 localAngles = transform.localEulerAngles;
        
        // Uncomment and use this instead to not rotate with pitch
        // Vector3 rot = new Vector3(localAngles.x, localAngles.y, goalZ);  
        
        // TODO: this might not be smooth since it pretends the axis are independent when they are not. 
        Vector3 rot = new Vector3(0.75f * (gliderInfo.totalPitchDegree - 90), 0, rightHandlebar.goalZ);  

        Debug.Log("SAVE:Is this number always 0:" + rot.y);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        
        // Below is reference code for if we want to snap handlebar to hand
        // Quaternion smoothTiltRotation = Quaternion.Slerp(thingToRotate.localRotation, Quaternion.Euler(rot), Time.deltaTime);
        // thingToRotate.localRotation = Quaternion.Euler(smoothTiltRotation.eulerAngles.x, smoothTiltRotation.eulerAngles.y, goalZ);
        
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
}
