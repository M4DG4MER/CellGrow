using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollRotator : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        CellControl c = collision.gameObject.GetComponent<CellControl>();
        if (c != null && c.inputHandler.isAI)
        {
            c?.ChangeDirection();
            collision.rigidbody.AddRelativeTorque(0, 90, 0);
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    CellControl c = collision.gameObject.GetComponent<CellControl>();
    //    if (c != null && c.inputHandler.isAI)
    //    {
    //        collision.rigidbody.AddRelativeTorque(0, 15, 0);
    //    }
    //}
}
