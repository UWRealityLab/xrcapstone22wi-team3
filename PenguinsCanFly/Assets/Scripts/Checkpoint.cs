using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
    public GliderInfo gliderInfo;
    private float pitchToAdd = -20f;  // Negative to pitch up, positive to pitch down

    // Start is called before the first frame update
    void Start()
    {
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            StartCoroutine(IncreaseHeight());
        }
    }

    IEnumerator IncreaseHeight()
    {
        for (int i = 0; i < 100; i++)
        {
            gliderInfo.penguinXRORigidbody.AddForce(Vector3.up * 20);
            gliderInfo.extraPitchDegree += pitchToAdd;
            yield return null;
        }
    }
}
