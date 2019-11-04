using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logger : MonoBehaviour
{
    static logger l;
    static public logger Logger { get => l ?? (l = FindObjectOfType<logger>()); }
    static public void Log(string t)
    {
        Logger.text.text = t;
    }

    public TMPro.TextMeshProUGUI text;
}
