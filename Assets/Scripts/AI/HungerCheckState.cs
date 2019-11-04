using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HungerCheckState : AIState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        animator.SetBool("Hunger", (int)cell.NextMutatePoints > (int)cell.mutatePoints);
    }

}
