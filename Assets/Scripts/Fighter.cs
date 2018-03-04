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
            if (Physics2D.Raycast(transform.position, -transform.up, 0.19f, 1 << 8))
            {
                playerAction = playerState.idle;
                Debug.Log("asd");
            }

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (playerAction != playerState.jumping)
            {
                playerAction = playerState.jumping;
                jump(4);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (playerAction == playerState.jumping)
            {
                moveLeft();
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
            if (playerAction == playerState.jumping)
            {
                moveRight();
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
            anim.SetBool("WalkTrigger", false);
            anim.SetBool("RunTrigger", false);
            playerAction = playerState.idle;
            isRunning = false;
        }
        if (playerAction == playerState.jumping)
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
}
