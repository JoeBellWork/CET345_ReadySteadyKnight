using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// scripted used to control player states
public class StateManager : MonoBehaviour
{
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

    public Slider healthSlider;
    SpriteRenderer sRenderer;

    [HideInInspector]
    public HandelDamageCollider handleDC;
    [HideInInspector]
    public HandelAnimation handleAnim;
    [HideInInspector]
    public HandleMovement handleMovement;

    public GameObject[] movementColliders;

    // grab scripts, animators and sprites to control fighter
    public void Start()
    {
        handleDC = GetComponent<HandelDamageCollider>();
        handleAnim = GetComponent<HandelAnimation>();
        handleMovement = GetComponent<HandleMovement>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    // controls health and input sections
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

    // bool check function to figure out when the fighters are on the ground
    public bool isOnGround()
    {
        bool retVal = false;
        LayerMask layer = ~(1 << gameObject.layer | 1 << 3);
        retVal = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, layer);
        return retVal;
    }

    // function to reset player state inputs
    public void ResetStateInputs()
    {
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

    // a means of controlling what damage colliders open and close at different times
    public void CloseMovementCollider(int index)
    {
        movementColliders[index].SetActive(false);
    }


    // a means of controlling what damage colliders open and close at different times
    public void OpenMovementCollider(int index)
    {
        movementColliders[index].SetActive(true);
    }

    //taking damage script
    public void TakeDamage(int damage, HandelDamageCollider.DamageType damageType)
    {
        if (!gettingHit) // ensures can only be hit 1 per damage animation
        {
            switch (damageType)
            {
                case HandelDamageCollider.DamageType.light:
                    StartCoroutine(CloseImmortality(0.3f));
                    break;
                case HandelDamageCollider.DamageType.heavy:
                    handleMovement.AddVelocityOnCharacter(((!lookRight) ? Vector3.right * 1 : Vector3.right * -1) + Vector3.up, 0.25f);
                    StartCoroutine(CloseImmortality(1));
                    break;
            }

            health -= damage;
            gettingHit = true;
        }
    }

    IEnumerator CloseImmortality(float timer) // invincibility frames on hit
    {
        yield return new WaitForSeconds(timer);
        gettingHit = false;
    }
}