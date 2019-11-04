using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{
    public Slider slider;
    public TMPro.TextMeshProUGUI Text;

    public string BaseText;

    public List<string> StringValues;


    private void Awake()
    {
        slider.onValueChanged.AddListener(v =>
        {
            int i = Mathf.Clamp((int)v, 0, StringValues.Count);
            Text.text = $"{BaseText} ({StringValues[i]})";
        });
    }
}
