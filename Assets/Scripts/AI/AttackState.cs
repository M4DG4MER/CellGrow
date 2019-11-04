using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIState
{
    protected InputHandler AiInput;
    public string AttackAxis = "Fire1";


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (AiInput == null)
            AiInput = cell.GetComponent<InputHandler>();

        AiInput.SetAxis(AttackAxis);

    }


}
