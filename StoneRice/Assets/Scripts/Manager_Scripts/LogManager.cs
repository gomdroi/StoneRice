using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoSingleton<LogManager>
{
    private Text LogText = null;

    private ScrollRect scrollRect = null;

    private void Awake()
    {
        LogText = GameObject.Find("Log_Text").GetComponent<Text>();
        scrollRect = GameObject.Find("Log_Window").GetComponent<ScrollRect>();
    }

    private void Start()
    {       
        if (LogText != null)
        {
            LogText.text += "LogManager On" + "\n";
        }
    }

    public void SimpleLog(string _log)
    {
        LogText.text += _log + "\n";

        scrollRect.verticalNormalizedPosition = 0.0f;
    }
}
