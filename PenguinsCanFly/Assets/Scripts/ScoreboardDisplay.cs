using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardDisplay : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    
    // Start is called before the first frame update
    void Start()
    {
        textbox.text = SaveManager.Instance.GetHiScore() + "\n" + SaveManager.Instance.GetPrevScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
