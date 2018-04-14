using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    string Class;
    GameObject weapon;
    GameObject eChip;
    GameObject discharger;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;

    protected enum playerState { idle, running, rolling, jumping, walking, sliding, attacking, jumpAttacking }
    protected playerState playerAction;
    public TriggerCheck[] triggerChecks;

    protected float health = 100;
    [SerializeField]
    float power;
    [SerializeField]
    float intelligence;
    [SerializeField]
    protected float speed;
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    float defense;
    [SerializeField]
    float defaultSpeed;
    float dashDuration = 0.2f;
    protected bool leftDash, rightDash, isRunning;
    protected int numbOfJumps;
    protected bool canRollAgain = true;
    protected bool comboDelay = false;
    protected int comboChainNumber, comboChainNumber2;

    public void Initialize()
    {
        playerAction = playerState.idle;
        rb = GetComponent<Rigidbody2D>();
        speed = 0.01f;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void jump(float jumpPow)
    {
        rb.AddForce(new Vector2(0, jumpPow), ForceMode2D.Impulse);
    }
    public void jumpLeft(float jumpPow)
    {
        rb.AddForce(new Vector2(-0.5f, jumpPow), ForceMode2D.Impulse);
    }
    public void jumpRight(float jumpPow)
    {
        rb.AddForce(new Vector2(0.5f, jumpPow), ForceMode2D.Impulse);
    }

    public void moveLeft()
    {
        //transform.Translate(new Vector2(-1, 0) * speed);
        rb.AddForce(new Vector2(-500, 0) * speed);
        sr.flipX = true;
    }
    public void moveRight()
    {
        //transform.Translate(new Vector2(1, 0) * speed);
        rb.AddForce(new Vector2(500, 0) * speed);
        sr.flipX = false;
    }
    public void moveLeftJump()
    {
        //transform.Translate(new Vector2(-1, 0) * speed);
        rb.AddForce(new Vector2(-100, 0) * 0.05f);
        sr.flipX = true;
    }
    public void moveRightJump()
    {
        //transform.Translate(new Vector2(1, 0) * speed);
        rb.AddForce(new Vector2(100, 0) * 0.05f);
        sr.flipX = false;
    }

    public void rollMovement()
    {
        if (sr.flipX == true)
        {
            //transform.Translate(new Vector2(-1, 0) * speed);
            rb.AddForce(new Vector2(-500, 0) * speed);
        }
        else
        {
            //transform.Translate(new Vector2(1, 0) * speed);
            rb.AddForce(new Vector2(500, 0) * speed);
        }
    }

    playerState prevState;

    protected void roll(string rollAnimName, float normSpeed, playerState ps)
    {
        prevState = ps;
        canRollAgain = false;
        defaultSpeed = normSpeed;
        anim.Play(rollAnimName);
        StartCoroutine("waitForRoll");
    }
    public void leftDashCheck()
    {
        if (leftDash == false)
        {
            StartCoroutine(dashDelay(1));
            leftDash = true;
        }
    }
    public void rightDashCheck()
    {
        if (rightDash == false)
        {
            StartCoroutine(dashDelay(2));
            rightDash = true;
        }
    }

    public void completeAttack()
    {
        playerAction = playerState.idle;
    }
    public void incrementComboChain()
    {
        StopCoroutine("resetComboChain");
        if (comboChainNumber < 2)
        {
            comboChainNumber++;
        }
        else
        {
            comboDelay = true; //used in fighter
        }
        StartCoroutine("resetComboChain");
    }
    public void incrementComboChain2()
    {
        StopCoroutine("resetComboChain");
        if (comboChainNumber2 < 1)
        {
            comboChainNumber2++;
        }
        else
        {
            comboDelay = true; //used in fighter
        }
        StartCoroutine("resetComboChain");
    }
    public bool applyDamageToTrigger(int x)
    {
        //0 = left
        //1 = right
        //2 = top
        //3 = bottom
        //4 = bottom right
        //5 = bottom left
        //6 = top right
        //7 = top left

        //punch - left/right
        //kick - bottom left/ bottom right

        switch (x)
        {
            case 0:
                if (triggerChecks[0].enemyInside == true)
                {
                    triggerChecks[0].enemyObject.GetComponent<Enemy>().depleteHealth();
                    return true;
                }
                break;
            case 1:
                if (triggerChecks[1].enemyInside == true)
                {
                    triggerChecks[1].enemyObject.GetComponent<Enemy>().depleteHealth();
                    return true;
                }
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
        }
        return false;

    }
    int triggerForJump;
    public void performJumpAttackDamage(int x)
    {
        triggerForJump = x;
        StartCoroutine("checkJumpAttacks");
    }

    public void stopJumpAttack()
    {
        StopCoroutine("checkJumpAttacks");
    }

    IEnumerator checkJumpAttacks()
    {
        bool check = true;
        while (check)
        {
            if (applyDamageToTrigger(triggerForJump))
            {
                check = false;
            }
            yield return null;
        }
    }
    IEnumerator resetComboChain()
    {
        yield return new WaitForSeconds(1.5f);
        comboChainNumber = 0;
        comboChainNumber2 = 0;
        comboDelay = false;
    }

    IEnumerator waitForRoll()
    {
        yield return new WaitForSeconds(0.5f);
        speed = defaultSpeed;
        if (prevState == playerState.jumping)
            playerAction = playerState.jumping;
        else
            playerAction = playerState.idle;
        anim.SetBool("RollTrigger", false);
        StartCoroutine("rollDelay");
    }

    IEnumerator rollDelay()
    {
        yield return new WaitForSeconds(2);
        canRollAgain = true;
    }

    IEnumerator dashDelay(int i)
    {
        yield return new WaitForSeconds(dashDuration);
        if (i == 1)
        {
            leftDash = false;
        }
        if (i == 2)
        {
            rightDash = false;
        }
    }

}
