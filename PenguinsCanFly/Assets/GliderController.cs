using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderController : MonoBehaviour
{
    
    public float speed = 12.5f;
    public float drag = 6;
    
    public Rigidbody rb;

    // TODO: This seems like really really bad design, will probably break even if it's working
    public HandlebarHandle handlebar;

    public float DEGREES_PER_SECOND = 40;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.drag = drag;
        Vector3 localV = transform.InverseTransformDirection(rb.velocity);
        localV.z = speed;
        rb.velocity = transform.TransformDirection(localV);
        Debug.Log("We assdfsdafasdfsd");
        
        Debug.Log("We got the handlebar: " + handlebar.thingToRotate.eulerAngles.z);
        Vector3 currentAngle = handlebar.thingToRotate.eulerAngles;
        Vector3 rot = transform.eulerAngles;
        float amountToRotate = currentAngle.z <= handlebar.MAX_ROTATION_DEGREES ? -currentAngle.z : 360 - currentAngle.z;
        rot.y += amountToRotate * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);
    }
}
