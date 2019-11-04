using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IReseteable : MonoBehaviour
{

    public virtual void ResetThis(Vector3 prandom)
    {
        this.gameObject.SetActive(true);
        this.transform.position = prandom;
    }
}
