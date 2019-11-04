using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSize : MonoBehaviour
{
    private Cell cell;

    public float size = 1;
    public float MaxSize = 100;

    public float Multiplier = 0.05f;

    private void Awake()
    {
        cell = GetComponent<Cell>();
        cell.onEat += new CellEvent(RecalculateSizeOnEat);
    }

    void RecalculateSizeOnEat(Cell c, float v)
    {

        size += v;
        size = Mathf.Clamp(size, 1, MaxSize);
        this.transform.localScale = Vector3.one * (1 + size * Multiplier);
    }


}
