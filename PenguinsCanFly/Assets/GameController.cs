using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject launchController;
    public GameObject glidingController;
    
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
        glidingController.SetActive(false);
        launchController.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGlidingMode()
    {
        Debug.Log("GLIDING STARTED!!");
        glidingController.SetActive(true);
        launchController.SetActive(false);
    }
}