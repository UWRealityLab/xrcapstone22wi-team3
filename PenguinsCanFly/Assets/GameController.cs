using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject launchController;
    public GameObject glidingController;

    private MonoBehaviour _launchScript;
    private MonoBehaviour _glidingScript;

    private static GameController _instance;

    public static GameController Instance
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
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _launchScript = launchController.GetComponent<FlapDetector>();
        _glidingScript = glidingController.GetComponent<GliderInfo>();

        _launchScript.enabled = true;
        _glidingScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: remove
        Debug.Log("SAVE:enabled:launch-" + _launchScript.enabled + "...gliding-" + _glidingScript.enabled);
    }

    public void StartGlidingMode()
    {
        Debug.Log("GLIDING STARTED!!");
        // TODO: refactor gldiing stuff
        _glidingScript.enabled = true;
        launchController.SetActive(false);
    }
}