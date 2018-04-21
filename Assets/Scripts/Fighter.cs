using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Player
{

    private void Start()
    {
        Initialize();
        touchingPos = transform.position - new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y);
    }

    private void Update()
    {


        if (playerAction == playerState.rolling)
        {
            rollMovement();
            return;
        }


        if (playerAction == playerState.jumping)
        {
            if (rb.velocity.y < 0)
            {
                anim.Play("Fighter_fall");
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StopCoroutine("runCancelDelay");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerAction != playerState.jumping && playerAction != playerState.jumpAttacking)
            {
                if (numbOfJumps < 5)
                {
                    if (playerAction == playerState.sliding)
                    {
                        anim.Play("Fighter_jump");
                        if (sr.flipX == true)
                        {
                            jumpRight(4);
                            numbOfJumps++;
                        }
                        else
                        {
                            jumpLeft(4);
                            numbOfJumps++;
                        }
                        prevState = playerState.sliding;
                    }
                    else
                    {
                        jump(4);
                    }
                    playerAction = playerState.jumping;
                    anim.SetBool("WalkTrigger", false);
                    anim.SetBool("RunTrigger", false);
                    comboChainNumber = 0;
                }
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding || playerAction == playerState.jumpAttacking)
            {
                if (playerAction == playerState.sliding && sr.flipX == false)
                    anim.Play("Fighter_fall");
                if (prevState == playerState.sliding)
                {
                    moveSlideJumpLeft();
                    sr.flipX = true;
                }
                else
                    moveLeftJump();
            }
            else
            {
                if (playerAction != playerState.attacking)
                {
                    if (isRunning == true)
                    {
                        playerAction = playerState.running;
                        speed = 2f;
                        moveLeft();
                        anim.SetBool("RunTrigger", true);
                    }
                    else
                    {
                        playerAction = playerState.walking;
                        speed = 0.75f;
                        moveLeft();
                        anim.SetBool("WalkTrigger", true);
                    }
                }
            }
            comboChainNumber = 0;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding || playerAction == playerState.jumpAttacking)
            {
                if (playerAction == playerState.sliding && sr.flipX == true)
                    anim.Play("Fighter_fall");
                if (prevState == playerState.sliding)
                {
                    moveSlideJumpRight();
                    sr.flipX = false;
                }
                else
                    moveRightJump();
            }
            else
            {
                if (playerAction != playerState.attacking)
                {
                    if (isRunning == true)
                    {
                        playerAction = playerState.running;
                        speed = 2f;
                        moveRight();
                        anim.SetBool("RunTrigger", true);
                    }
                    else
                    {
                        playerAction = playerState.walking;
                        speed = 0.75f;
                        moveRight();
                        anim.SetBool("WalkTrigger", true);
                    }
                }
            }
            comboChainNumber = 0;
        }

        if (Input.GetKeyDown(KeyCode.V)) //V ATTACK IF IN AIR
        {
            if (playerAction == playerState.jumping)
            {
                anim.Play("Fighter_jumpAtk1");
                playerAction = playerState.jumpAttacking;
                if (sr.flipX == false)
                    performJumpAttackDamage(1); // Does not change in mid air
                else
                    performJumpAttackDamage(0);
            }

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerAction == playerState.jumping)
            {
                anim.Play("Fighter_jumpAtk2");
                playerAction = playerState.jumpAttacking;
                if (sr.flipX == false)
                    performJumpAttackDamage(1);
                else
                    performJumpAttackDamage(0);
            }

        }

        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (playerAction != playerState.jumping && playerAction != playerState.sliding && playerAction != playerState.jumpAttacking)
            {
                anim.SetBool("WalkTrigger", false);
                anim.SetBool("RunTrigger", false);
                playerAction = playerState.idle;
                StartCoroutine("runCancelDelay");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerAction != playerState.sliding)
            {
                if (canRollAgain == true)
                {
                    if (playerAction == playerState.jumping)
                    {
                        var ps = playerAction;
                        speed = 0.02f;
                        anim.SetBool("WalkTrigger", false);
                        anim.SetBool("RunTrigger", false);
                        playerAction = playerState.rolling;
                        anim.SetBool("RollTrigger", true);
                        roll("Fighter_roll", 0.01f, ps);
                    }
                    else
                    {
                        var ps = playerAction;
                        speed = 0.05f;
                        anim.SetBool("WalkTrigger", false);
                        anim.SetBool("RunTrigger", false);
                        playerAction = playerState.rolling;
                        anim.SetBool("RollTrigger", true);
                        roll("Fighter_roll", 0.01f, ps);
                    }
                    comboChainNumber = 0;
                }
            }
        }

        if (playerAction == playerState.jumping || playerAction == playerState.sliding || playerAction == playerState.jumpAttacking)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (leftDash == true)
            {
                isRunning = true;
            }
            leftDashCheck();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (rightDash == true)
            {
                isRunning = true;
            }
            rightDashCheck();
        }
        else if (Input.GetKeyDown(KeyCode.V)) //V ATTACK IF NOT IN AIR
        {
            if (playerAction != playerState.attacking && playerAction != playerState.running)
            {
                // Debug.Log(comboChainNumber);
                if (comboDelay == false)
                {
                    anim.SetBool("WalkTrigger", false);
                    anim.SetBool("RunTrigger", false);
                    if (comboChainNumber == 2)
                    {
                        anim.Play("Fighter_kick1");
                        playerAction = playerState.attacking;
                        isRunning = false;
                        if (sr.flipX == false)
                            setTriggerForAttack(1);
                        else
                            setTriggerForAttack(0);

                    }
                    else
                    {
                        anim.Play("Fighter_punch1");
                        playerAction = playerState.attacking;
                        isRunning = false;
                        if (sr.flipX == false)
                            setTriggerForAttack(1);
                        else
                            setTriggerForAttack(0);
                    }
                    incrementComboChain();
                }
            }
            else if (playerAction == playerState.running)
            {
                anim.SetBool("WalkTrigger", false);
                anim.SetBool("RunTrigger", false);
                anim.Play("Fighter_dashPunch");
                playerAction = playerState.attacking;
                isRunning = false;
                if (sr.flipX == false)
                    setTriggerForAttack(1);
                else
                    setTriggerForAttack(0);
            }

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerAction != playerState.attacking && playerAction != playerState.running)
            {
                if (comboDelay == false)
                {
                    anim.SetBool("WalkTrigger", false);
                    anim.SetBool("RunTrigger", false);
                    if (comboChainNumber2 == 1)
                    {
                        anim.Play("Fighter_kick3");
                        playerAction = playerState.attacking;
                        isRunning = false;
                        if (sr.flipX == false)
                            setTriggerForAttack(1);
                        else
                            setTriggerForAttack(0);

                    }
                    else
                    {
                        anim.Play("Fighter_punch1");
                        playerAction = playerState.attacking;
                        isRunning = false;
                        if (sr.flipX == false)
                            setTriggerForAttack(1);
                        else
                            setTriggerForAttack(0);
                    }
                    incrementComboChain2();
                }
            }
            else if (playerAction == playerState.running)
            {
                anim.SetBool("WalkTrigger", false);
                anim.SetBool("RunTrigger", false);
                anim.Play("Fighter_dashKick");
                playerAction = playerState.attacking;
                isRunning = false;
                if (sr.flipX == false)
                    setTriggerForAttack(1);
                else
                    setTriggerForAttack(0);
            }
        }
        //  Debug.DrawRay(transform.position - new Vector3((GetComponent<BoxCollider2D>().bounds.extents.x), (GetComponent<BoxCollider2D>().bounds.extents.y * 0.5f)), -Vector3.up, Color.red);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchingPos = transform.position - new Vector3((-GetComponent<BoxCollider2D>().bounds.extents.x), (GetComponent<BoxCollider2D>().bounds.extents.y * 0.5f));
        touchingPos2 = transform.position - new Vector3((GetComponent<BoxCollider2D>().bounds.extents.x), (GetComponent<BoxCollider2D>().bounds.extents.y * 0.5f));
        if (collision.collider.gameObject.layer == 9)
        {
            if (playerAction == playerState.jumping)
            {
                playerAction = playerState.sliding;
                anim.Play("Fighter_wallSlide");
                prevState = playerState.sliding;
            }
        }
        else if (collision.collider.gameObject.layer == 8)
        {
            if (playerAction != playerState.rolling)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchingPos, -Vector3.up, 1f, 1 << 8);
                RaycastHit2D hit2 = Physics2D.Raycast(touchingPos2, -Vector3.up, 1f, 1 << 8);

                if (hit.collider != null || hit2.collider != null)
                {
                    playerAction = playerState.idle;
                    anim.Play("Figher_idle");
                    prevState = playerState.idle;
                    anim.SetBool("WalkTrigger", false);
                    anim.SetBool("RunTrigger", false);
                }
            }
            else
            {
                prevState = playerState.idle;
            }
            numbOfJumps = 0;
            stopJumpAttack();

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            if (playerAction != playerState.rolling)
            {
                if (playerAction != playerState.jumping)
                {
                    anim.Play("Fighter_fall");
                }
                else
                {
                    anim.Play("Fighter_jump");
                }
                playerAction = playerState.jumping;
                //rb.drag = 2;
            }
        }
        else if (collision.collider.gameObject.layer == 9)
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding)
            {
                //rb.drag = 1;
            }
        }
    }
}
