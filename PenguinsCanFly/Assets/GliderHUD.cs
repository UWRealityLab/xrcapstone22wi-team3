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

    
    // Update is called once per frame
    void Update()
    {
        speedText.text = Random.Range(3, 10).ToString("D3");
        distanceText.text = Time.timeSinceLevelLoad.ToString("D5");
    }
}
