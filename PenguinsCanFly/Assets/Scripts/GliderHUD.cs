using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GliderHUD : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI distanceText;
    public Transform Needle;

    public GameObject Warnings;
    public TextMeshProUGUI WarningTextCues;

    public TextMeshProUGUI fishText;

    private static GliderHUD _instance;

    public static GliderHUD Instance
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

    // Update is called once per frame
    void Update()
    {
        float gliderSpeed = GameController.Instance.gliderInfo.ActualSpeed;
        speedText.text = ((int) Math.Round(gliderSpeed)).ToString("D3"); 
        distanceText.text = ScoreCounter.Instance.GetScore().ToString("D7");
        float needleAngle = Mathf.Clamp(185 - 185 * (gliderSpeed / 50f), -12, 185);
        Needle.localRotation = Quaternion.Euler(0, 0, needleAngle);
        fishText.text = ScoreCounter.Instance.numCoins.ToString("D2");
    }
}
