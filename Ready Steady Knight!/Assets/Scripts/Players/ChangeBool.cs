using UnityEngine;
using System.Collections;

public class ChangeBool : StateMachineBehaviour {

    public string boolName;
    public bool status;
    public bool resetOnExit;

     // state enter allows a between state machine behaviour when a transition state enters
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        animator.SetBool(boolName, status);
	}
	
	// state exit allows a transition between state machine behaviours when a transition state ends
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if(resetOnExit)
        {
            animator.SetBool(boolName, !status);
        }            
	}
}
