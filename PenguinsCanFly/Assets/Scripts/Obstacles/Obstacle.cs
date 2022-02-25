using System;
using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";

    private const float DestroyDistance = 1000f;
    
    private bool _obstacleHit = false;

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

    void OnDestroy()
    {
        LevelManager.NumObstaclesActiveInGame--;
    }

    IEnumerator DecreaseHeight()
    {
        float iterations = 300;
        for (int i = 0; i < iterations; i++)
        {
            GameController.Instance.gliderInfo.penguinXRORigidbody.AddForce(Vector3.down * 50);
            yield return null;
        }
        Destroy(gameObject);
    }
}
