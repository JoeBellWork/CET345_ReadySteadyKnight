using UnityEngine;

public class DoDamage : MonoBehaviour
{
    StateManager states;
    public HandelDamageCollider.DamageType damageType;


    // access state manager
    public void Start()
    {
        states = GetComponentInParent<StateManager>();
    }


    // control damage using colliders
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<StateManager>())
        {
            StateManager oState = collision.GetComponentInParent<StateManager>();
            if (oState != states)
            {
                if (!oState.currentlyAttacking)
                {
                    oState.TakeDamage(10, damageType);
                }
            }
        }
    }
}
