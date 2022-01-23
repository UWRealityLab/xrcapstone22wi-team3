using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapDetector : MonoBehaviour
{
    public FlapGroup top;

    public FlapGroup bottom;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: do something interesting with this count
        int topCount = top.GetHitCount();
        int bottomCount = bottom.GetHitCount();
        Debug.Log("SAVE:flaps: top-" + topCount + " bottom-" + bottomCount + " min-" + Math.Min(topCount, bottomCount));
    }
}
