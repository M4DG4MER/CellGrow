using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FleeState : TargetState
{
    protected override Vector3 calcultateTarget(Vector3 origen)
    {
        Vector3 fullNearest = Vector3.zero;
        detectors?.ForEach(dtc => fullNearest +=
            dtc != null && dtc.nearests != null ? (dtc.nearests.Count() > 1 ?
            dtc.nearests.Aggregate((a, b) => a - b) :
            (dtc.Closest - origen)) : 
            Vector3.zero
        );

        return origen - fullNearest;
    }
}
