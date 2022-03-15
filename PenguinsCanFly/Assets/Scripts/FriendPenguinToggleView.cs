using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendPenguinToggleView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!VoiceObstacleHandler.MultiplayerMode)
        {
            Destroy(gameObject);
        }
    }
}
