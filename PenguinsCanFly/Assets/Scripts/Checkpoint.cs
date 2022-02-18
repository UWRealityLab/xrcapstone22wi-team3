using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
    public GliderInfo gliderInfo;
    public float pitchToAdd = -20f;  // Negative to pitch up, positive to pitch down
    public float heightToAdd = 50f;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            gliderInfo.penguinXRORigidbody.AddForce(Vector3.up * heightToAdd);
            gliderInfo.extraPitchDegree += pitchToAdd;
            Debug.Log("SAVE:gliderInfoPitchDegree:" + gliderInfo.TotalPitchDegree);
        }
    }
}
