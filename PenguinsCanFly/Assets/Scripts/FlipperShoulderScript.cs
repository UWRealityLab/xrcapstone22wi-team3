using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperShoulderScript : MonoBehaviour
{
    public Transform controller;
    public GameObject flipper;

    public float flipperHandWidth = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controllerShoulderDiff = controller.position - transform.position;

        // Point shoulder / flipper in controllers direction
        transform.forward = controllerShoulderDiff;

        // Add rotation for some mobility
        this.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, controller.transform.eulerAngles.z);


        // Change Penguin Flipper length
        float flipperLength = controllerShoulderDiff.magnitude + flipperHandWidth;
        flipper.transform.localScale = new Vector3(flipper.transform.localScale.x, flipper.transform.localScale.y, flipperLength);
        flipper.transform.localPosition = new Vector3(0f, 0f, flipperLength / 2);
        Debug.Log(flipper.transform.localPosition);

    }
}
