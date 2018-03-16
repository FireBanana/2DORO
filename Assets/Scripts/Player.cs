using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    string Class;
    GameObject weapon;
    GameObject eChip;
    GameObject discharger;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;

    protected enum playerState { idle, running, rolling, jumping, walking, sliding}
    protected playerState playerAction;

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
        rb.AddForce(new Vector2(-100, 0) * speed);
        sr.flipX = true;
    }
    public void moveRightJump()
    {
        //transform.Translate(new Vector2(1, 0) * speed);
        rb.AddForce(new Vector2(100, 0) * speed);
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

    public void roll(string rollAnimName, float normSpeed)
    {
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

    IEnumerator waitForRoll()
    {
        yield return new WaitForSeconds(0.5f);
        speed = defaultSpeed;
        anim.SetBool("RollTrigger", false);
        playerAction = playerState.idle;
    }

    IEnumerator dashDelay(int i)
    {
        yield return new WaitForSeconds(dashDuration);
        if(i == 1)
        {
            leftDash = false;
        }
        if(i == 2)
        {
            rightDash = false;
        }
    }

}
