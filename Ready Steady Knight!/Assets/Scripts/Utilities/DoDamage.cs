using UnityEngine;

public class DoDamage : MonoBehaviour
{
    private AudioManagerScript audioManager;
    StateManager states;
    public HandleDamageColliders.DamageType damageType;

    public void Start()
    {
        audioManager = AudioManagerScript.getInstance();
        states = GetComponentInParent<StateManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // on colision with collision zones, deal damage to player, play sounds and change health.
        if(collision.GetComponentInParent<StateManager>())
        {
            StateManager oState = collision.GetComponentInParent<StateManager>();
            if(oState != states)
            {
                if(!oState.currentlyAttacking)
                {
                    audioManager.soundPlay("Effect_Hit");
                    oState.TakeDamage(10, damageType);
                }                
            }
        }
    }
}
