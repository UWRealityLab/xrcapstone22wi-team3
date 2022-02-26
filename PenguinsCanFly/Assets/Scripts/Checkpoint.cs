using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";

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
                LevelManager.Instance.PlayerMissedCheckpoint();
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
        float iterations = 200;
        for (int i = 0; i < iterations; i++)
        {
            GameController.Instance.gliderInfo.penguinXRORigidbody.AddForce(Vector3.up * 50);
            yield return null;
        }
        LevelManager.Instance.PlayerPassedCheckpoint();
        Destroy(gameObject);
    }
}
