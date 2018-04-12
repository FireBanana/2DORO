using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Player
{

    RaycastHit2D hit;

    private void Start()
    {
        Initialize();
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

        if (Input.GetKeyDown(KeyCode.W))
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

                    }
                    else
                    {
                        jump(4);
                    }
                    if (playerAction == playerState.walking)
                        rb.drag = 2;
                    else
                        rb.drag = 1;
                    playerAction = playerState.jumping;
                    anim.SetBool("WalkTrigger", false);
                    anim.SetBool("RunTrigger", false);
                    comboChainNumber = 0;
                }
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding || playerAction == playerState.jumpAttacking)
            {
                if (playerAction == playerState.sliding && sr.flipX == false)
                    anim.Play("Fighter_fall");
                moveLeftJump();
            }
            else
            {
                if (playerAction != playerState.attacking)
                {
                    if (isRunning == true)
                    {
                        playerAction = playerState.running;
                        speed = 0.03f;
                        moveLeft();
                        anim.SetBool("RunTrigger", true);
                    }
                    else
                    {
                        playerAction = playerState.walking;
                        speed = 0.01f;
                        moveLeft();
                        anim.SetBool("WalkTrigger", true);
                    }
                }
            }
            comboChainNumber = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding || playerAction == playerState.jumpAttacking)
            {
                if (playerAction == playerState.sliding && sr.flipX == true)
                    anim.Play("Fighter_fall");
                moveRightJump();
            }
            else
            {
                if (playerAction != playerState.attacking)
                {
                    if (isRunning == true)
                    {
                        playerAction = playerState.running;
                        speed = 0.03f;
                        moveRight();
                        anim.SetBool("RunTrigger", true);
                    }
                    else
                    {
                        playerAction = playerState.walking;
                        speed = 0.01f;
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
                anim.Play("Fighter_jumpAtk4");
                playerAction = playerState.jumpAttacking;
            }

        }

        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (playerAction != playerState.jumping && playerAction != playerState.sliding && playerAction != playerState.jumpAttacking)
            {
                anim.SetBool("WalkTrigger", false);
                anim.SetBool("RunTrigger", false);
                playerAction = playerState.idle;
                isRunning = false;
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (leftDash == true)
            {
                isRunning = true;
            }
            leftDashCheck();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (rightDash == true)
            {
                isRunning = true;
            }
            rightDashCheck();
        }
        else if (Input.GetKeyDown(KeyCode.V)) //V ATTACK IF NOT IN AIR
        {
            if (playerAction != playerState.attacking)
            {
                Debug.Log(comboChainNumber);
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
                            applyDamageToTrigger(1);
                        else
                            applyDamageToTrigger(0);

                    }
                    else
                    {
                        anim.Play("Fighter_punch1");
                        playerAction = playerState.attacking;
                        isRunning = false;
                        if (sr.flipX == false)
                            applyDamageToTrigger(1);
                        else
                            applyDamageToTrigger(0);
                    }
                    incrementComboChain();
                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            if (playerAction != playerState.rolling)
            {
                playerAction = playerState.idle;
                anim.Play("Figher_idle");
                rb.drag = 10;
            }
            else
            {
                rb.drag = 10;
            }
            numbOfJumps = 0;
        }
        if (collision.collider.gameObject.layer == 9)
        {
            if (playerAction == playerState.jumping)
            {
                rb.drag = 10;
                playerAction = playerState.sliding;
                anim.Play("Fighter_wallSlide");
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            if (playerAction != playerState.jumping)
            {
                anim.Play("Fighter_fall");
            }
            else
            {
                anim.Play("Fighter_jump");
            }
                rb.drag = 2;
        }
        if (collision.collider.gameObject.layer == 9)
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding)
            {
                rb.drag = 1;
            }
        }
    }
}
