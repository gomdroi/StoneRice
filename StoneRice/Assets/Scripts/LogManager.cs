using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoSingleton<LogManager>
{
    private Text LogText = null;

    private ScrollRect scrollRect = null;

    private void Start()
    {
        LogText = GameObject.Find("Log_Text").GetComponent<Text>();
        scrollRect = GameObject.Find("Log_Window").GetComponent<ScrollRect>();

        if (LogText != null)
        {
            LogText.text += "Hello Log Window!" + "\n";
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LogText.text += "Mouse Down Position (" + "X : " + Input.mousePosition.x + " Y : " + Input.mousePosition.y + ")\n";
        }

        scrollRect.verticalNormalizedPosition = 0.0f;
    }

    public void SimpleLog(string _log)
    {
        LogText.text += _log + "\n";

        scrollRect.verticalNormalizedPosition = 0.0f;
    }
}
