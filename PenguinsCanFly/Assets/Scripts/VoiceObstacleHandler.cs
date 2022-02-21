using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceObstacleHandler : MonoBehaviour
{
    public Transform leftLocation;

    public Transform middleLocation;
    public Transform rightLocation;

    public GliderInfo gliderInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnVoiceObstacle(string[] values)
    {
        string location = values[0];
        Debug.Log("SPAWN ITEM!!!!!! YAYAYA " + location + " " + values);
        if (location.Equals("left"))
        {
            GameObject windColliderObject = (GameObject) Instantiate(Resources.Load("WindCollider"),
                leftLocation.position, Quaternion.identity);
            WindCollider windColliderScript = windColliderObject.GetComponent<WindCollider>();
            windColliderScript.gliderInfo = gliderInfo;
        } else if (location.Equals("right"))
        {
            GameObject windColliderObject = (GameObject) Instantiate(Resources.Load("WindCollider"),
                leftLocation.position, Quaternion.identity);
            WindCollider windColliderScript = windColliderObject.GetComponent<WindCollider>();
            windColliderScript.gliderInfo = gliderInfo;
        } else if (location.Equals("middle"))
        {
            GameObject windColliderObject = (GameObject) Instantiate(Resources.Load("WindCollider"),
                middleLocation.position, Quaternion.identity);
            WindCollider windColliderScript = windColliderObject.GetComponent<WindCollider>();
            windColliderScript.gliderInfo = gliderInfo;
        }
    }
}
