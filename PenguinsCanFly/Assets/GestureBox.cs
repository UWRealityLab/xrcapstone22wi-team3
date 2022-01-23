using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureBox : MonoBehaviour
{
    public bool isTouched;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: check if touched by a left or right hand controller
        isTouched = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // TODO: check if touched by a left or right hand controller
        isTouched = false;
    }
}
