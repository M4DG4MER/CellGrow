using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "Evolucion/Color")]
public class CellColor : CellProperty
{
    public Material material;
    public Gradient gradient;

    public Color Multiplier;


}


/*
 * Body => comida       
 * Locomotion =>        //velocidad y salto
 * Armor => X        
 * Canion =>    //veneno y fuego
 * Cola =>      //velocidad y salto
 * Cuernos => X
 * eyes => X
 * 
 * 
 */
