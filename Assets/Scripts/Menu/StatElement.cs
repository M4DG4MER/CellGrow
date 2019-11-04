using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatElement : MonoBehaviour
{
    public CellADNValue cellADNValue;
    public TMPro.TextMeshProUGUI Text;
    public NumericUpDown upDown;
    public Button All;

    public Cell cell;

    private void Awake()
    {
        upDown.onBeforeChange += UpDown_onBeforeChange;
        upDown.onAfterChange += UpDown_onAfterChange;
        All.onClick.AddListener(() =>
        {
            cellADNValue.Mutate(cell.MutatePoints, cell, true);
            cell.MutatePoints = 0;
        });
    }

    private bool UpDown_onAfterChange(NumericUpDown num)
    {
        cellADNValue.Mutate(num.value, cell, true);
        return --cell.MutatePoints > 0;
    }

    internal void SetUp(Cell c, CellADNValue adnVal)
    {
        this.cell = c;
        cellADNValue = adnVal;
        Text.text = cellADNValue.ADN.Description;
    }

    private void Update()
    {
        upDown.value = cellADNValue.value;
    }

    private bool UpDown_onBeforeChange(NumericUpDown num)
    {
        return cell.MutatePoints > 0;
    }
}

