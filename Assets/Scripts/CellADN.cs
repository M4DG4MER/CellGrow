using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DNA", menuName ="Evolucion/DNA")]
public class CellADN : CellProperty
{
    public Mesh _Mesh;
    public string Blendshape;
}



public class CellProperty : ScriptableObject
{
    public ADNstat Stat;

    public string Description;
}