using System;
using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";
    private float pitchToAdd = 3f;  // Negative to pitch up, positive to pitch down

    private const float DestroyDistance = 1000f;

    public const float ObstacleHeightDecrease = 3f;
    
    public abstract float GetSpawnOffsetLowerBound();
    public abstract float GetSpawnOffsetUpperBound();
    public abstract Quaternion GetSpawnRotation();

    public void Update()
    {
        // Destroy obstacle if it falls below the ground
        if (transform.position.y < -20)
        {
            Destroy(gameObject);
        }
        
        // Destroy obstacle when the user is too far past it
        float gliderPositionZ = GameController.Instance.gliderInfo.penguinXROTransform.position.z;
        float selfPositionZ = transform.position.z;
        if (selfPositionZ - gliderPositionZ < -DestroyDistance)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            DeviceManager.Instance.SendCollisionHaptics();
            StartCoroutine(DecreaseHeight());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(HangGliderTag))
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        LevelManager.NumObstaclesActiveInGame--;
    }

    IEnumerator DecreaseHeight()
    {
        float iterations = 200;  // iterations * WaitForSeconds = length of time to apply height increase over
        float heightIncDecreasePerIteration = ObstacleHeightDecrease / iterations;
        for (int i = 0; i < iterations; i++)
        {
            GameController.Instance.gliderInfo.penguinXROTransform.position += Vector3.down * heightIncDecreasePerIteration;
            GameController.Instance.gliderInfo.extraPitchDegree += pitchToAdd;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
