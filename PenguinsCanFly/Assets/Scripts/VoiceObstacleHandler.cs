using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class VoiceObstacleHandler : MonoBehaviour
{
    public GameObject voiceObstacle;

    public const float LeftRightOffset = 50;

    private GliderInfo gliderInfo;
    
    
    private Transform penguinXROTransform;

    // Start is called before the first frame update
    void Start()
    {
        gliderInfo = GameController.Instance.gliderInfo;
        penguinXROTransform = gliderInfo.penguinXROTransform;
        
        InvokeRepeating("SpawnRandom", 2.0f, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 penguinPosition = penguinXROTransform.position;
        transform.position = Vector3.forward * (penguinPosition.z + GameController.Instance.gliderInfo.speed * 10) +
                             Vector3.up * (penguinPosition.y * 0.5f - 10);
    }

    private Vector3 LeftLocation()
    {
        return transform.position + Vector3.left * LeftRightOffset;
    }

    private Vector3 RightLocation()
    {
        return transform.position + Vector3.right * LeftRightOffset;
    }

    private void SpawnRandom()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            GameObject go = GameObject.Instantiate(voiceObstacle, null, true);
            go.transform.position = LeftLocation();
        } else if (rand == 1)
        {
            GameObject go = GameObject.Instantiate(voiceObstacle, null, true);
            go.transform.position = RightLocation();
        }
        else
        {
            GameObject go = GameObject.Instantiate(voiceObstacle, null, true);
            go.transform.position = transform.position;
        }
    }

    public void SpawnVoiceObstacle(string[] values)
    {
        string location = values[0];
        Debug.Log("SPAWN ITEM!!!!!! YAYAYA " + location + " " + values);
        if (location.Equals("left"))
        {
        //     GameObject windColliderObject = (GameObject) Instantiate(Resources.Load("WindCollider"),
        //         spawnLocation.position, Quaternion.identity);
        //     WindCollider windColliderScript = windColliderObject.GetComponent<WindCollider>();
        //     windColliderScript.gliderInfo = gliderInfo;
        // } else if (location.Equals("right"))
        // {
        //     GameObject windColliderObject = (GameObject) Instantiate(Resources.Load("WindCollider"),
        //         leftLocation.position, Quaternion.identity);
        //     WindCollider windColliderScript = windColliderObject.GetComponent<WindCollider>();
        //     windColliderScript.gliderInfo = gliderInfo;
        // } else if (location.Equals("middle"))
        // {
        //     GameObject windColliderObject = (GameObject) Instantiate(Resources.Load("WindCollider"),
        //         middleLocation.position, Quaternion.identity);
        //     WindCollider windColliderScript = windColliderObject.GetComponent<WindCollider>();
        //     windColliderScript.gliderInfo = gliderInfo;
        }
        // TODO: implement spawnvoiceobstaacle callback!
    }
}
