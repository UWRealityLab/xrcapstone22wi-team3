using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
    private float pitchToAdd = 3f;  // Negative to pitch up, positive to pitch down
    
    public abstract float GetSpawnOffsetLowerBound();
    public abstract float GetSpawnOffsetUpperBound();
    public abstract Quaternion GetSpawnRotation();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            StartCoroutine(DecreaseHeight());
        }
    }

    IEnumerator DecreaseHeight()
    {
        for (int i = 0; i < 100; i++)
        {
            GameController.Instance.gliderInfo.penguinXRORigidbody.AddForce(Vector3.down * 5);
            GameController.Instance.gliderInfo.extraPitchDegree += pitchToAdd;
            yield return null;
        }
    }
}
