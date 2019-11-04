using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellEditor : MonoBehaviour
{


    public StatElement Prefab;

    public List<ADNstat> Stats;
    private List<CellADNValue> values;

    Cell cell;

    public void SetUp(Cell cell)
    {
        this.cell = cell;

        StatElement[] children = GetComponentsInChildren<StatElement>();
        foreach (var st in children)
            Destroy(st.gameObject);

        values = this.cell.ADNs.Where(adn => adn.ADN.Stat.In(Stats)).ToList();
        values.ForEach(adn =>  Instantiate(Prefab, transform).SetUp(cell, adn));
    }

}



public static class CollectionExtension
{

    public static bool In<T>(this T element, IEnumerable<T> collection2)
    {
        return collection2.Contains(element);
    }

}