using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutateState : AIState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Hunger", true);
        cell.MutateWastePoints();
    }
}
