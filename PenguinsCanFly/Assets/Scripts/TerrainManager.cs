using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private float lastPositionZ;
    private float totalDistance;

    private GameObject terrainPrefab;
    private float terrainZ = 250;
    private float startingOffset = 550;

    private int numInstantiated;
    
    // Start is called before the first frame update
    void Start()
    {
        lastPositionZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionZ = transform.position.z;
        totalDistance += (newPositionZ - lastPositionZ);
        lastPositionZ = newPositionZ;

        // TODO: Remove hard coding of instantitation, destroy past tiles, clean up overall
        if (Math.Round(totalDistance) >= startingOffset && (Math.Round(totalDistance) - startingOffset) % terrainZ == 0)
        {
            // Debug.Log("New terrain tile instantiated " + numInstantiated);
            Instantiate(Resources.Load("Snow Tile 1"),
                new Vector3(-75, 0, totalDistance + terrainZ),
                Quaternion.identity);

            numInstantiated++;
        }
    }
}
