using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodType Type;

    public Cell thrower;

    MeshRenderer _renderer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cell")
        {
            var cell = other.gameObject.GetComponent<Cell>();

            if (cell != null && cell != thrower)
            {
                cell.ThrowEatEvent(Type.Nutricion);
                this.gameObject.SetActive(false);
            }
        }
        
    }

}
