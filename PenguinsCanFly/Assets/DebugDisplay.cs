using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    private List<string> debugLogs = new List<string>();
    public Text Display;
    
    protected void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        Debug.Log("Hey there dork!");
    }
    
    protected void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            debugLogs.Add(logString);

            string displayText = "";
            for (int i = debugLogs.Count - 1; i >= 0 && debugLogs.Count - i < 12; i--)
            {
                displayText = debugLogs[i] + "\n" + displayText;;

            }
            Display.text = displayText;
        }
    }
}