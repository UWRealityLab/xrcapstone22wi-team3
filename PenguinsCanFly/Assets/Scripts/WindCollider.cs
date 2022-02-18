using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCollider : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
    public GliderInfo gliderInfo;
    public float speedToAdd = 0.5f;
    public float pitchToAdd = -20f;  // Negative to pitch up, positive to pitch down
    
    // Start is called before the first frame update
    void Start()
    {
        gliderInfo = GameController.Instance.gliderInfo;
    }
    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            gliderInfo.extraPitchDegree += pitchToAdd;
            gliderInfo.extraSpeed += speedToAdd;
        }
    }
}
