using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GliderInfo gliderInfo;  // Used for winds
    public Transform penguinXROTransform;
    
    private float lastPositionZ;
    private float totalDistance;

    private GameObject terrainPrefab;
    private float terrainZ = 250;
    private float startingOffset = 550;

    private int numInstantiated;
    
    // Start is called before the first frame update
    void Start()
    {
        lastPositionZ = penguinXROTransform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionZ = penguinXROTransform.position.z;
        totalDistance += (newPositionZ - lastPositionZ);
        lastPositionZ = newPositionZ;

        // TODO: Remove hard coding of instantitation, destroy past tiles, clean up overall
        if (totalDistance >= startingOffset && (int) ((totalDistance - startingOffset) / terrainZ) == numInstantiated)
        {
            // Debug.Log("New terrain tile instantiated " + numInstantiated);
            Instantiate(Resources.Load("Snow Tile 1"),
                new Vector3(-75, 0, totalDistance + terrainZ),
                Quaternion.identity);
            
            GameObject windColliderObject = (GameObject) Instantiate(Resources.Load("WindCollider"),
                new Vector3(0, 50, totalDistance + terrainZ),
                Quaternion.identity);
            WindCollider windColliderScript = windColliderObject.GetComponent<WindCollider>();
            windColliderScript.gliderInfo = gliderInfo;

            numInstantiated++;
            Debug.Log("SAVE:NUMInstantiated:" + numInstantiated);
        }
    }
}
