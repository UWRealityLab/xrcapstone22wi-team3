using System;
using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour
{
    private const string HangGliderTag = "HangGlider";

    private const float DestroyDistance = 1000f;
    
    private bool _obstacleHit = false;
    
    // should this obstacle always be spawned on the ground? if so, Lower and UpperBound will be ignored
    public abstract bool ShouldSpawnOnGround();
    public abstract float GetSpawnOffsetLowerBound();
    public abstract float GetSpawnOffsetUpperBound();

    public GameObject mesh;
    public GameObject destroyedPieces;
    
    
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
        if (other.gameObject.CompareTag(HangGliderTag) && !_obstacleHit) // for obstacles that have multiple colliders
        {
            _obstacleHit = true;
            DeviceManager.Instance.SendCollisionHaptics();
            LevelManager.Instance.collisionAudioSource.Play();
            CustomCollisionEffects(other);
            StartCoroutine(DecreaseHeight());
        }
    }

    public void CustomCollisionEffects(Collider other)
    {
        if (mesh == null)
        {
            return;
        }
        
        mesh.SetActive(false);
        
        Vector3 collisionLocation = other.transform.position;
        destroyedPieces.SetActive(true);
        for (int i = 0; i < destroyedPieces.transform.childCount; i++)
        {
            Transform destroyedPiece = destroyedPieces.transform.GetChild(i);
            Rigidbody destroyedPieceRb = destroyedPiece.GetComponent<Rigidbody>();
            destroyedPieceRb.AddExplosionForce(1000, collisionLocation, 50, 15);
        }
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
