using UnityEngine;

public class OpenDamageCollider : StateMachineBehaviour
{
    // used to reopen damage colliders after crouching and jumping
    StateManager states;
    public HandleDamageColliders.DamageType damageType;
    public HandleDamageColliders.DCtype dcType;
    public float delay;

    // on state enter allows behaviour changes to animations and collider controls
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
        {
            states = animator.transform.GetComponentInParent<StateManager>();
        }
        states.handleDC.OpenCollider(dcType, delay, damageType);
    }


    // on state exit allows behaviour changes to animations and collier controls.
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
        {
            states = animator.transform.GetComponentInParent<StateManager>();
        }
        states.handleDC.CloseCollider();
    }
}
