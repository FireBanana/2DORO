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
            if (playerAction != playerState.jumping )
            {
                if (playerAction == playerState.sliding)
                {
                    Debug.Log("runu");
                    anim.Play("Fighter_jump");
                }
                playerAction = playerState.jumping;
                anim.SetBool("WalkTrigger", false);
                anim.SetBool("RunTrigger", false);
                rb.drag = 1;
                jump(4);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding)
            {
                if (playerAction == playerState.sliding && sr.flipX == false)
                    anim.Play("Fighter_fall");
                moveLeftJump();
            }
            else
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
        else if (Input.GetKey(KeyCode.D))
        {
            if (playerAction == playerState.jumping || playerAction == playerState.sliding)
            {
                if (playerAction == playerState.sliding && sr.flipX == true)
                    anim.Play("Fighter_fall");
                moveRightJump();
            }
            else
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
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (playerAction != playerState.jumping && playerAction != playerState.sliding)
            {
                anim.SetBool("WalkTrigger", false);
                anim.SetBool("RunTrigger", false);
                playerAction = playerState.idle;
                isRunning = false;
            }
        }
        if (playerAction == playerState.jumping || playerAction == playerState.sliding)
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            speed = 0.05f;
            anim.SetBool("WalkTrigger", false);
            anim.SetBool("RunTrigger", false);
            playerAction = playerState.rolling;
            anim.SetBool("RollTrigger", true);
            roll("Fighter_roll", 0.01f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            playerAction = playerState.idle;
            anim.Play("Figher_idle");
            rb.drag = 10;
        }
        if(collision.collider.gameObject.layer == 9)
        {
            if(playerAction == playerState.jumping)
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
            rb.drag = 1;
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
