using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
    public string boolName;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if(updateOnStateMachine)
            animator.SetBool(boolName, valueOnEnter);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnExit);
    }
}