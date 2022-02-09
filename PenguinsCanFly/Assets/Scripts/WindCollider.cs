using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCollider : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
    private Material m_Material;
    public GliderInfo gliderInfo;
    public float speedToAdd = 0.5f;
    public float pitchToAdd = -20f;  // Negative to pitch up, positive to pitch down
    
    // Start is called before the first frame update
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }
    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            Color color = m_Material.color;
            color.a = 1f;
            m_Material.color = color;
            gliderInfo.extraPitchDegree += pitchToAdd;
            gliderInfo.extraSpeed += speedToAdd;
            // Debug.Log("SAVE:gliderInfoPitchDegree:" + gliderInfo.TotalPitchDegree);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            Color color = m_Material.color;
            color.a = 0.4f;
            m_Material.color = color;
            // Debug.Log("EXITED! Wind collider");
        }
    }
}
