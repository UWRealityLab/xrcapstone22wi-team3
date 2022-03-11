using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;

    // Saved values
    private float hiScore;
    private float prevScore;
    private int totalCoins;
    private int prevNumCoins;
    
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
        
        hiScore = PlayerPrefs.GetFloat("hiScore", 0);
        prevScore = PlayerPrefs.GetFloat("prevScore", 0);
        totalCoins = PlayerPrefs.GetInt("totalCoins", 0);
        prevNumCoins = PlayerPrefs.GetInt("prevNumCoins", 0);
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public float GetHiScore()
    {
        return hiScore;
    }

    public float GetPrevScore()
    {
        return prevScore;
    }

    public float GetTotalCoins()
    {
        return totalCoins;
    }

    public int GetPrevNumCoins()
    {
        return prevNumCoins;
    }

    public void SaveScore(float score, int numCoins)
    {
        prevScore = score;
        PlayerPrefs.SetFloat("prevScore", prevScore);
        if (score > hiScore)
        {
            hiScore = score;
            PlayerPrefs.SetFloat("hiScore", hiScore);
        }

        prevNumCoins = numCoins;
        PlayerPrefs.SetInt("prevNumCoins", prevNumCoins);

        totalCoins += numCoins;
        PlayerPrefs.SetInt("totalCoins", totalCoins);
    }
}
