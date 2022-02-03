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

    // Note, 0 is up, 90 is forward, 180 is down
    public const int MAX_PITCH_OFFSET_DEGREE = 40;

    
    // Start is called before the first frame update
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("WHAATTT" + gameObject.name + " " + gameObject.tag);
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            Debug.Log("Please");
            Color color = m_Material.color;
            color.a = 1f;
            m_Material.color = color;
            gliderInfo.totalPitchDegree = Mathf.Clamp(gliderInfo.totalPitchDegree + pitchToAdd,
                                                    90 - MAX_PITCH_OFFSET_DEGREE,
                                                    90 + MAX_PITCH_OFFSET_DEGREE);
            // gliderInfo.penguinXRORigidbody.AddForce(Vector3.up * 500f);
            gliderInfo.extraSpeed += speedToAdd;
            Debug.Log("SAVE:gliderInfoPitchDegree:" + gliderInfo.totalPitchDegree);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            Color color = m_Material.color;
            color.a = 0.4f;
            m_Material.color = color;
            // gliderInfo.totalPitchDegree = 90;
            Debug.Log("EXITED! Wind collider");
        }
    }
}
