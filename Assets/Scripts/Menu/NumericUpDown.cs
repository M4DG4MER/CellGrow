using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumericUpDown : MonoBehaviour
{


    public Button buttonAdd;
    public Button buttonRemove;
    public TMPro.TextMeshProUGUI Text;

    public float value = 0;
    public float MinValue = 0;
    public float MaxValue = 0;

    public event ValueChange onBeforeChange;
    public event ValueChange onAfterChange;


    public float Value
    {
        get => value;
        set
        {
            if (onBeforeChange?.Invoke(this) ?? false)
            {
                float v = Mathf.Clamp(value, MinValue, MaxValue);
                if (this.value != v)
                {
                    this.value = v;
                    onAfterChange?.Invoke(this);
                }
            }
        }
    }


    private void Awake()
    {
        Value = value;
        buttonAdd.onClick.AddListener(new UnityEngine.Events.UnityAction(() => Value++));
        buttonRemove.onClick.AddListener(new UnityEngine.Events.UnityAction(() => Value--));
    }

    private void Update()
    {
        Text.text = this.value.ToString("0");
    }


}

public delegate bool ValueChange(NumericUpDown num);

