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
            CustomCollisionEffects(other);
            StartCoroutine(DecreaseHeight());
        }
    }

    public virtual void CustomCollisionEffects(Collider other)
    {
        // TODO: find out what code can be shared so we can use base.CustomCollisionEffects
    }

    void OnDestroy()
    {
        LevelManager.NumObstaclesActiveInGame--;
    }

    public virtual float GetCollisionForce()
    {
        return 50;
    }

    IEnumerator DecreaseHeight()
    {
        float iterations = 300;
        for (int i = 0; i < iterations; i++)
        {
            if (LandingController.Instance.GetDistanceToGround() <= LandingController.LandingHeight)
            {
                // Don't apply force if we are too close to the ground
                break;
            }
            
            GameController.Instance.gliderInfo.penguinXRORigidbody.AddForce(Vector3.down * GetCollisionForce());
            yield return null;
        }
        Destroy(gameObject);
    }
}
