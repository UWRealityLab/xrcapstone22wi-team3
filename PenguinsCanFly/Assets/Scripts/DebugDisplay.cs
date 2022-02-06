using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DebugDisplay : MonoBehaviour
{
    private List<string> debugLogs = new List<string>();
    private Dictionary<string, string> saveValues = new Dictionary<string, string>();
    public Text Display;
    public int NUM_LOG_ITEMS = 22;
    
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
            if (logString.StartsWith("SAVE:"))
            {
                string[] components = logString.Split(':');
                if (components.Length != 3)
                {
                    debugLogs.Add("ERROR: BAD DEBUG LOG!!!!: " + logString);
                }

                saveValues[components[1]] = components[2];
            }
            else
            {
                debugLogs.Add(logString);
            }
                        
            string displayText = "";
            for (int i = debugLogs.Count - 1; i >= 0 && debugLogs.Count - i < NUM_LOG_ITEMS - saveValues.Count; i--)
            {
                displayText = debugLogs[i] + "\n" + displayText;

            }

            displayText += "---\n";
            foreach (string key in saveValues.Keys)
            {
                displayText += key + ": " + saveValues[key] + "\n";
            }
            Display.text = displayText;
        }
    }
}