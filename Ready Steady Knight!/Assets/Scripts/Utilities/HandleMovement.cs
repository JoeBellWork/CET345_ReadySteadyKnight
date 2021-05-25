using System.Collections;
using UnityEngine;

public class HandleMovement : MonoBehaviour
{
    Rigidbody2D rb;
    StateManager states;
    HandleAnimations anim;
    private AudioManagerScript audioManager;

    public float acceleration = 30;
    public float airAcceleration = 15;
    public float maxSpeed = 20;
    public float jumpSpeed = 8;
    public float jumpDuration = 150;
    float actualSpeed;
    bool justJumped;
    bool canVaribleJump;
    float jumpTimer;

    void Start()
    {
        audioManager = AudioManagerScript.getInstance();
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<StateManager>();
        anim = GetComponent<HandleAnimations>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // continous checks for input
        if (!states.dontMove)
        {
            HorizontalMovement();
            Jump();
        }
    }

    void HorizontalMovement()
    {
        // function allows movement back and forth while not attacking
        actualSpeed = this.maxSpeed;

        if (states.onGround && !states.currentlyAttacking)
        {
            rb.AddForce(new Vector2((states.horizontal * actualSpeed) - rb.velocity.x * this.acceleration, 0));
        }

        if (states.horizontal == 0 && states.onGround)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Jump()
    {
        // jump script that checks if the player wishes to jump, if they can jump and controls velocity and direction
        if (states.vertical > 0)
        {
            if (!justJumped)
            {
                justJumped = true;

                if (states.onGround)
                {
                    audioManager.soundPlay("Effect_Jump");
                    anim.JumpAnim();
                    rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);
                    jumpTimer = 0;
                    canVaribleJump = true;
                }
            }
            else
            {
                if (canVaribleJump)
                {
                    jumpTimer += Time.deltaTime;
                    if (jumpTimer < this.jumpDuration / 1000)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);
                    }
                }
            }
        }
        else
        {
            justJumped = false;
        }
    }

    public void AddVelocityOnCharacter(Vector3 direction, float timer)
    {
        // adds force to character accessing the IEnumerator
        StartCoroutine(AddVelocity(timer, direction));
    }

    IEnumerator AddVelocity(float timer, Vector3 direction)
    {
        // generate velocity on impact to push player away from heavy kick or to add jump force
        float t = 0;
        while (t < timer)
        {
            t += Time.deltaTime;

            rb.AddForce(direction);
            yield return null;
        }
    }
}
