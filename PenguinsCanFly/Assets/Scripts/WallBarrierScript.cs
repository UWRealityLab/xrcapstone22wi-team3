using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBarrierScript : MonoBehaviour
{
    public GameObject wall1;
    public GameObject wall2;

    private Renderer wall1Renderer;
    private Renderer wall2Renderer;

    void Awake()
    {
        wall1.transform.position = new Vector3(-TerrainManager.gameWidth / 2f - 5, 0, 0);
        wall2.transform.position = new Vector3(TerrainManager.gameWidth / 2f + 5, 0, 0);
        
        wall1Renderer = wall1.GetComponent<MeshRenderer>();
        wall2Renderer = wall2.GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody penguinXRORigidBody = GameController.Instance.gliderInfo.penguinXRORigidbody;
        Vector3 gliderPos = penguinXRORigidBody.transform.position;
        transform.position = new Vector3(0, 50, gliderPos.z);

        penguinXRORigidBody.position =
            new Vector3(Mathf.Clamp(gliderPos.x, -TerrainManager.gameWidth / 2f, TerrainManager.gameWidth / 2f),
                gliderPos.y, gliderPos.z);

        Color color = wall1Renderer.material.color;
        float leftOpacity = 1 - Mathf.Abs(gliderPos.x - (-TerrainManager.gameWidth / 2f)) / 10;
        color.a = Mathf.Clamp(leftOpacity, 0, 1);
        wall1Renderer.material.color = color;
        
        Color color2 = wall2Renderer.material.color;
        float rightOpacity = 1 - Mathf.Abs(gliderPos.x - (TerrainManager.gameWidth / 2f)) / 10;
        Debug.Log("SAVE:rightOpacity: " + rightOpacity);
        color2.a = Mathf.Clamp(rightOpacity, 0, 1);
        wall2Renderer.material.color = color2;

    }
}
