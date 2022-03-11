using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Facebook.WitAi;
using Facebook.WitAi.Lib;


public class VoiceObstacleHandler : MonoBehaviour
{
    
    enum VoiceObstacleState
    {
        IDLE,
        ATTACK_SEQUENCE_STARTED,
        MIC_ACTIVATED,
        MIC_DEACTIVATED,
        WAITING_FOR_WIT,
        FAILED,
        EXECUTING,
    }

    public static bool MultiplayerMode = true;
    
    [SerializeField] private Wit wit;

    public GameObject voiceObstacle;

    public const float LeftRightOffset = 50;
    
    private bool spawningEnabled = false;
    private VoiceObstacleState currentState = VoiceObstacleState.IDLE;
    private int numObstaclesToSpawn = 0;
    private int attemptId = 0;
    
    private static VoiceObstacleHandler _instance;

    public static VoiceObstacleHandler Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Add callback to mic from Wit
        Mic.Instance.OnStopRecording += MicDeactivatedCallback;
    }

    private void AddObstacles()
    {
        Debug.Log("Added an obstacle!");
        if (spawningEnabled)
        {
            numObstaclesToSpawn += 1;
            Invoke("AddObstacles", Random.Range(10, 30));
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!MultiplayerMode)
        {
            return;
        }
        
        // Get wit
        if (wit == null)
        {
            Debug.Log("trying to get wit");
            wit = GetComponent<Wit>();
        }

        // TODO: temporary location and also need to determine if in multiplayer mode or not
        if (GameController.Instance.gliderInfo.transform.position.y > 20 && wit != null)
        {
            EnableVoiceObstaclesSpawn();
        }
        else
        {
            DisableVoiceObstaclesSpawn();
        }
        
        Vector3 penguinPosition = GameController.Instance.gliderInfo.penguinXROTransform.position;
        transform.position = Vector3.forward * (penguinPosition.z + GameController.Instance.gliderInfo.speed * 8) +
                             Vector3.up * (penguinPosition.y * 0.75f - 15);

        if (currentState == VoiceObstacleState.IDLE)
        {
            if (numObstaclesToSpawn > 0 && spawningEnabled)
            {
                currentState = VoiceObstacleState.ATTACK_SEQUENCE_STARTED;
                StartAttackSequence();
            }
            else
            {
                GliderHUD.Instance.Warnings.SetActive(false);
            }
        }
        
        Debug.Log("SAVE:VoiceObstacleData: numObs " + numObstaclesToSpawn 
                                                    + " attemptId " + attemptId 
                                                    + " state" + Enum.GetName(typeof(VoiceObstacleState), currentState)
                                                    + " spawnEnabled " + spawningEnabled);
    }

    public void DisableVoiceObstaclesSpawn()
    {
        spawningEnabled = false;
        CancelInvoke("AddObstacles");
        numObstaclesToSpawn = 0;
    }

    public bool IsVoiceObstaclesSpawningEnabled()
    {
        return spawningEnabled;
    }

    public void EnableVoiceObstaclesSpawn()
    {
        if (!spawningEnabled)
        {
            spawningEnabled = true;
            numObstaclesToSpawn = 0;
            Invoke("AddObstacles", Random.Range(10, 30));
        }
    }

    private void StartAttackSequence()
    {
        attemptId += 1;
        StartCoroutine(AttackSequenceCoroutine());
    }
    
    IEnumerator AttackSequenceCoroutine()
    {
        yield return new WaitForSeconds(1);
        GliderHUD.Instance.Warnings.SetActive(true);

        for (int i = 3; i >= 1; i--)
        {
            Debug.Log("Attacking in .... " + i);
            GliderHUD.Instance.WarningTextCues.text = "Attacking in " + i + "...";
            yield return new WaitForSeconds(1);
        }
        currentState = VoiceObstacleState.MIC_ACTIVATED;
        wit.Activate();
        GliderHUD.Instance.WarningTextCues.text = "!!!";

    }

    public void MicDeactivatedCallback()
    {
        currentState = VoiceObstacleState.MIC_DEACTIVATED;
        GliderHUD.Instance.WarningTextCues.text = "... deciphering message ...";
        currentState = VoiceObstacleState.WAITING_FOR_WIT;
        StartCoroutine(WaitingTimeoutCoroutine(attemptId));
    }

    IEnumerator WaitingTimeoutCoroutine(int attemptId)
    {
        // Wait for a few seconds for Wit to get a response
        yield return new WaitForSeconds(3);
        
        if (currentState == VoiceObstacleState.WAITING_FOR_WIT && this.attemptId == attemptId)
        {
            currentState = VoiceObstacleState.FAILED;
            GliderHUD.Instance.WarningTextCues.text = "... attack failed ...";
            currentState = VoiceObstacleState.IDLE;
        }
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
            GameObject go = Instantiate(voiceObstacle, null, true);
            go.transform.position = LeftLocation();
        } else if (rand == 1)
        {
            GameObject go = Instantiate(voiceObstacle, null, true);
            go.transform.position = RightLocation();
        }
        else
        {
            GameObject go = Instantiate(voiceObstacle, null, true);
            go.transform.position = transform.position;
        }
    }

    public void SpawnVoiceObstacle(string[] values)
    {
        numObstaclesToSpawn -= 1;
        if (currentState == VoiceObstacleState.WAITING_FOR_WIT)
        {
            currentState = VoiceObstacleState.EXECUTING;
        }
        
        string location = values[0];
        Debug.Log("SPAWNING ITEM: " + location + " " + values);
        if (location.Equals("left"))
        {
            GameObject go = Instantiate(voiceObstacle, null, true);
            go.transform.position = LeftLocation();
            GliderHUD.Instance.WarningTextCues.text = "Attacking left!";
        } else if (location.Equals("right"))
        {
            GameObject go = Instantiate(voiceObstacle, null, true);
            go.transform.position = RightLocation();
            GliderHUD.Instance.WarningTextCues.text = "Attacking right!";
        }
        else if (location.Equals("middle"))
        {
            GameObject go = Instantiate(voiceObstacle, null, true);
            go.transform.position = transform.position;
            GliderHUD.Instance.WarningTextCues.text = "Attacking middle!";
        }
        else
        {
            SpawnRandom();
            GliderHUD.Instance.WarningTextCues.text = "Uncertain: attacking random";
        }

        if (currentState == VoiceObstacleState.EXECUTING)
        {
            StartCoroutine(ChangeToIdleAfterDelay());
        }
    }

    IEnumerator ChangeToIdleAfterDelay()
    {
        yield return new WaitForSeconds(1);
        currentState = VoiceObstacleState.IDLE;
    }

    
}
