using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
    private float pitchToAdd = -20f;  // Negative to pitch up, positive to pitch down

    private bool checkpointHit = false;

    // Start is called before the first frame update
    void Start()
    {
    }


    private void Update()
    {
        // Destroy checkpoint when the user is past it, and only if we didn't hit the checkpoint already
        if (!checkpointHit)
        {
            float gliderPositionZ = GameController.Instance.gliderInfo.penguinXROTransform.position.z;
            float selfPositionZ = transform.position.z;
            if (selfPositionZ - gliderPositionZ < -20f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            checkpointHit = true;
            StartCoroutine(IncreaseHeight());
        }
    }

    IEnumerator IncreaseHeight()
    {
        float iterations = 200;  // iterations * WaitForSeconds = length of time to apply height increase over
        for (int i = 0; i < iterations; i++)
        {
            GameController.Instance.gliderInfo.extraPitchDegree += pitchToAdd;
            yield return new WaitForSeconds(0.01f);
        }
        LevelManager.Instance.ReadyToGenerateCheckpoint();
        Destroy(gameObject);
    }
}
