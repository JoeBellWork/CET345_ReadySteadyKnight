using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{

    private ParticleExplosion particleVar;
    // fighter varibles from health, input controls through bool switches, heigt tracker and direction facing.
    public int health = 100;
    public float horizontal;
    public float vertical;
    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool crouch;
    public bool canAttack;
    public bool gettingHit;
    public bool currentlyAttacking;
    public bool dontMove;
    public bool onGround;
    public bool lookRight;

    // control health slider
    public Slider healthSlider;
    SpriteRenderer sRenderer;


    //access other scripts for collider manipulation and animations
    [HideInInspector]
    public HandleDamageColliders handleDC;
    [HideInInspector]
    public HandleAnimations handleAnim;
    [HideInInspector]
    public HandleMovement handleMovement;

    public GameObject[] movementColliders;


    public void Start()
    {
        // get other scripts
        handleDC = GetComponent<HandleDamageColliders>();
        handleAnim = GetComponent<HandleAnimations>();
        handleMovement = GetComponent<HandleMovement>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
        particleVar = GetComponent<ParticleExplosion>();
    }


    public void FixedUpdate()
    {
        sRenderer.flipX = lookRight;
        onGround = isOnGround();

        if (healthSlider != null)
        {
            healthSlider.value = health * 0.01f;
        }

        if (health <= 0)
        {
            if (LevelManager.getInstance().countdown)
            {
                LevelManager.getInstance().EndTurnFunction();
                handleAnim.anim.Play("Dead");
            }
        }
    }

    public bool isOnGround()
    {
        // bool check using ray cast to check for layer of ground
        bool retVal = false;
        LayerMask layer = ~(1 << gameObject.layer | 1 << 3);
        retVal = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, layer);
        return retVal;
    }

    public void ResetStateInputs()
    {
        // used after a fight round to reset health, position and logic
        horizontal = 0;
        vertical = 0;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        crouch = false;
        gettingHit = false;
        currentlyAttacking = false;
        dontMove = false;
    }

    // close movement colliders to allow jumping and crouching
    public void CloseMovementCollider(int index)
    {
        movementColliders[index].SetActive(false);
    }

    // close movement colliders to allow jumping and crouching
    public void OpenMovementCollider(int index)
    {
        movementColliders[index].SetActive(true);
    }

    // damage function called by other scripts that registers damage typ for force knock back, ammount of damage taken and imortality.
    public void TakeDamage(int damage, HandleDamageColliders.DamageType damageType)
    {
        if (!gettingHit)
        {
            switch (damageType)
            {
                case HandleDamageColliders.DamageType.light:
                    StartCoroutine(CloseImmortality(0.3f));
                    break;
                case HandleDamageColliders.DamageType.heavy:
                    handleMovement.AddVelocityOnCharacter(((!lookRight) ? Vector3.right * 1 : Vector3.right * -1) + Vector3.up, 0.25f);
                    StartCoroutine(CloseImmortality(1));
                    break;
            }

            health -= damage;
            particleVar.ExplodePlay();
            gettingHit = true;
        }
    }

    IEnumerator CloseImmortality(float timer)
    {
        yield return new WaitForSeconds(timer);
        gettingHit = false;
    }
}
