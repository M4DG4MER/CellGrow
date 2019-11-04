using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class DetectState : AIState
{
    public string DetectName;

    public List<DetectorStats> Definition;

    protected List<Detector> detectors = new List<Detector>();
    protected CellDetector cellDetector;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (cellDetector == null)
        {
            cellDetector = cell.GetComponent<CellDetector>();
            detectors = cellDetector.detectors
                .Where(dtc => dtc.Definition.In(Definition)).ToList();
        }

        animator.SetBool(DetectName, detectors.Any(d => d.foundedOne));
    }

}
