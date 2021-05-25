using System.Collections;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    // full AI behaviour
    // list of varibles that control audio, animation, access state manager to replicate input.
    private AudioManagerScript audioManager;
    StateManager states;
    public StateManager enStates;
    public float changeStateTolerance = 3;

    // floats and timers for opening and closing behaviour/ animation
    public float normalRate = 1;
    float nrmTimer;

    public float closeRate = 0.5f;
    float clTimer;

    public float aiStateLife = 1;
    float aidTimer;

    bool initiateAI;
    bool closeCombat;

    bool gotRandom;
    float storeRandom;

    bool randomizeAttacks;
    int numberOfAttacks;
    int curNumberAttacks;

    public float JumpRate = 1;
    float jRate;
    bool jump;
    float jTimer;

    public AttackPatterns[] attackPatterns;

    public enum AIState
    {
        closeState,
        normalState,
        resetAI
    }

    public AIState aiState;

    void Start()
    {
        audioManager = AudioManagerScript.getInstance();
        states = GetComponent<StateManager>();
    }

    void Update()
    {
        CheckDistance();
        States();
        AIAgent();
    }

    void States()
    {
        switch (aiState)
        {
            case AIState.closeState:
                CloseState();
                break;
            case AIState.normalState:
                NormalState();
                break;
            case AIState.resetAI:
                ResetAI();
                break;
        }
        Jumping();
    }

    void AIAgent()
    {
        if (initiateAI)
        {
            aiState = AIState.resetAI;
            float multiplier = 0;

            if (!gotRandom)
            {
                storeRandom = ReturnRandom();
                gotRandom = true;
            }

            if (!closeCombat)
            {
                multiplier += 30;
            }
            else
            {
                multiplier -= 50;
            }

            if (storeRandom + multiplier < 50)
            {
                Attack();
            }
            else
            {
                Movement();
            }
        }
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, enStates.transform.position);
        if (distance < changeStateTolerance)
        {
            if (aiState != AIState.resetAI)
            {
                aiState = AIState.closeState;
            }
            closeCombat = true;
        }
        else
        {
            if (aiState != AIState.resetAI)
            {
                aiState = AIState.normalState;
            }

            if (closeCombat)
            {
                if (!gotRandom)
                {
                    storeRandom = ReturnRandom();
                    gotRandom = true;
                }

                if (storeRandom < 90)
                {
                    Movement();
                }
            }

            closeCombat = false;
        }
    }

    void Attack()
    {
        if (!gotRandom)
        {
            storeRandom = ReturnRandom();
            gotRandom = true;
        }

        if (!randomizeAttacks)
        {
            numberOfAttacks = (int)Random.Range(1, 4);
            randomizeAttacks = true;
        }

        if (curNumberAttacks < numberOfAttacks)
        {
            int attackNumber = Random.Range(0, attackPatterns.Length);
            StartCoroutine(OpenAttack(attackPatterns[attackNumber], 0));
            curNumberAttacks++;
        }

    }

    void Movement()
    {
        if (!gotRandom)
        {
            storeRandom = ReturnRandom(); //use randomiser to see probability of moving towards player
            gotRandom = true;
        }

        if (storeRandom < 90) //90% chance using Return random function to move towards player
        {
            if (enStates.transform.position.x < transform.position.x)
            {
                states.horizontal = -1;
            }
            else
            {
                states.horizontal = 1;
            }
        }
        else // or 10 % to move away from player
        {
            if (enStates.transform.position.x < transform.position.x)
            {
                states.horizontal = 1;
            }
            else
            {
                states.horizontal = -1;
            }
        }
    }

    void Jumping()
    {
        // controls the percentage change of the AI jumping as well as attacking mid jump
        if (!enStates.onGround)
        {
            float r = ReturnRandom();
            if (r < 50)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
        }



        if (jump)
        {
            audioManager.soundPlay("Effect_Jump");
            states.vertical = 1;
            jRate = ReturnRandom();
            jump = false;
        }
        else
        {
            states.vertical = 0;
        }

        jTimer += Time.deltaTime;

        if (jTimer > JumpRate * 10)
        {
            if (jRate < 50)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
            jTimer = 0;
        }
    }

    void ResetAI()
    {
        // reset AI conditionals to recieve new direction and orders
        aidTimer += Time.deltaTime;
        if (aidTimer > aiStateLife)
        {
            initiateAI = false;
            states.horizontal = 0;
            states.vertical = 0;
            aidTimer = 0;

            gotRandom = false;
            // switch state from normal to close
            storeRandom = ReturnRandom();
            if (storeRandom < 50)
            {
                aiState = AIState.normalState;
            }
            else
            {
                aiState = AIState.closeState;
            }
            curNumberAttacks = 1;
            randomizeAttacks = false;
        }
    }

    IEnumerator OpenAttack(AttackPatterns a, int i)
    {
        // ienumerator which counts delay and attack given to output attack type for AI as well as combo counter and hen they can attack
        int index = i;
        float delay = a.attacks[index].delay;
        states.attack1 = a.attacks[index].attack1;
        states.attack2 = a.attacks[index].attack2;
        yield return new WaitForSeconds(delay);

        states.attack1 = false;
        states.attack2 = false;

        if (index < a.attacks.Length - 1)
        {
            index++;
            StartCoroutine(OpenAttack(a, index));
        }
    }

    float ReturnRandom()
    {
        float retVal = (Random.Range(0, 101));
        return retVal;
    }

    void CloseState()
    {
        // set close state at end of moment, used to trigger new AI behaviour with initiate AI bool
        clTimer += Time.deltaTime;
        if (clTimer > closeRate)
        {
            clTimer = 0;
            initiateAI = true;
        }
    }

    void NormalState()
    {
        // set normal state at start, used to trigger new AI behaviour with initiate AI bool
        nrmTimer += Time.deltaTime;
        if (nrmTimer > normalRate)
        {
            nrmTimer = 0;
            initiateAI = true;
        }
    }

    [System.Serializable]
    public class AttackPatterns
    {
        // attack pattern array for combo move which allows kicking while on the ground
        public AttackBase[] attacks;
    }

    [System.Serializable]
    public class AttackBase
    {
        // inherited class for attack styles and delay time
        public bool attack1;
        public bool attack2;
        public float delay;
    }
}
