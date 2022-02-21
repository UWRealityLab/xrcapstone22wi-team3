using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;

    // Saved values
    private float hiScore;
    private float prevScore;
    
    public static SaveManager Instance
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
        hiScore = PlayerPrefs.GetFloat("hiScore", 0);
        prevScore = PlayerPrefs.GetFloat("prevScore", 0);
    }

    public float GetHiScore()
    {
        return hiScore;
    }

    public float GetPrevScore()
    {
        return prevScore;
    }

    public void SaveScore(float score)
    {
        prevScore = score;
        PlayerPrefs.SetFloat("prevScore", prevScore);
        if (score > hiScore)
        {
            hiScore = score;
            PlayerPrefs.SetFloat("hiScore", hiScore);
        }
    }
}
