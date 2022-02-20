using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
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
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator IncreaseHeight()
    {
        for (int i = 0; i < 100; i++)
        {
            GameController.Instance.gliderInfo.penguinXRORigidbody.AddForce(Vector3.up * 20);
            GameController.Instance.gliderInfo.extraPitchDegree += pitchToAdd;
            yield return null;
        }
    }
}
