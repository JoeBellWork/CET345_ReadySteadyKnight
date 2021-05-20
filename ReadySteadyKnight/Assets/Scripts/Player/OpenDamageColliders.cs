using UnityEngine;

public class OpenDamageColliders : StateMachineBehaviour
{
    StateManager states;
    public HandelDamageCollider.DamageType damageType;
    public HandelDamageCollider.DCtype dcType;
    public float delay;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
        {
            states = animator.transform.GetComponentInParent<StateManager>();
        }
        states.handleDC.OpenCollider(dcType, delay, damageType);
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
        {
            states = animator.transform.GetComponentInParent<StateManager>();
        }
        states.handleDC.CloseCollider();
    }
}
