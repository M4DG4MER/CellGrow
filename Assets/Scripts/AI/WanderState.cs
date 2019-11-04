using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : TargetState
{
    public float ChangeAfter = 5f;
    private float delta;
    public Vector3 range;
    private Vector3 lastTarget;

    protected override Vector3 calcultateTarget(Vector3 origen)
    {
        delta += Time.deltaTime;
        if (delta < ChangeAfter)
            return cell?.Control?.inputHandler?.Target ?? lastTarget;

        delta = 0;
        return lastTarget = new Vector3(
            Random.Range(-range.x, range.x),
            Random.Range(-range.y, range.y),
            Random.Range(-range.z, range.z)) +origen;
    }
}
