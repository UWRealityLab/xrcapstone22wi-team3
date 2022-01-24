using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureBox : MonoBehaviour
{
    private bool _isTouched;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public bool IsTouched()
    {
        return _isTouched;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: check if touched by a left or right hand controller
        _isTouched = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // TODO: check if touched by a left or right hand controller
        _isTouched = false;
    }
}
