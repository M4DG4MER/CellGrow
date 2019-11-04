using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class TargetState : AIState
{
    protected InputHandler AiInput;
    protected Vector3 last;

    public List<DetectorStats> Definition;
    protected CellDetector cellDetector;
    protected List<Detector> detectors = new List<Detector>();

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (cellDetector == null)
        {
            cellDetector = cell.GetComponent<CellDetector>();
            detectors = cellDetector?.detectors.Where(dtc => dtc.Definition.In(Definition)).ToList();
        }
        if (AiInput == null)
            AiInput = cell.GetComponent<InputHandler>();

        AiInput.Target = calcultateTarget(cell.mtransform.position);
    }
    

    protected virtual Vector3 calcultateTarget(Vector3 origen)
    {
       return -(detectors?.Where(dtc => dtc.foundedOne).Select(dtc => dtc.Closest -origen).OrderBy(v => v.magnitude).FirstOrDefault() ?? Vector3.zero) + origen ;
    }

}
