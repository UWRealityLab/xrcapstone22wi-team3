using System;
using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;

public class CastleVoiceObstacle : VoiceObstacle
{
    public GameObject offset;

    // Start is called before the first frame update
    void Start()
    {
        offset.transform.localPosition += Vector3.down * 50;
        mesh.SetActive(true);
        destroyedPieces.SetActive(false);
    }
    public void Update()
    {
        base.Update();
        Vector3 localOffset = offset.transform.localPosition;
        offset.transform.localPosition = Vector3.Lerp(localOffset, Vector3.zero, Time.deltaTime);
    }

    public override float GetCollisionForce()
    {
        return 50;
    }

    public override void CustomCollisionEffects(Collider other)
    {
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
}
