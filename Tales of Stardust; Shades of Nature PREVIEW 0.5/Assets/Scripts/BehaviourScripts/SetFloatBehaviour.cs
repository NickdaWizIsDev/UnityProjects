using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{
    public string floatName;
    public bool updateOnStateMachine;
    public bool updateOnState;
    public float valueOnEnter, valueOnExit;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetFloat(floatName, valueOnEnter);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.SetFloat(floatName, valueOnExit);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(floatName, valueOnEnter);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(floatName, valueOnExit);
    }
}
