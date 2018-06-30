using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Player
{
    private void Start()
    {
        Initialize();
        touchingPos = transform.position - new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y);
        PlayerAuthenticator.instance.fighterScript = this;
    }

    private void Update()
    {


        if (playerAction == playerState.rolling)
        {
            rollMovement();
            return;
        }

        if (playerAction == playerState.braking)
        {
            print(rb.velocity.x);
            if ((rb.velocity.x < 0.1f && rb.velocity.x > 0) || (rb.velocity.x > -0.1f && rb.velocity.x < 0))
            {
                playerAction = playerState.idle;
                anim.Play("Figher_idle");
            }
        }
        else if (!Input.anyKey && (playerAction == playerState.idle ||
                              playerAction == playerState.running))
        {
            if (rb.velocity.x > 1 || rb.velocity.x < -1)
            {
                anim.Play("Fighter_brake");
                playerAction = playerState.braking;
            }
        }


        if (playerAction == playerState.jumping)
        {
            if (rb.velocity.y < 0)
            {
                anim.Play("Fighter_fall");
            }
        }
        
        if(isChatting)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StopCoroutine("runCancelDelay");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (playerAction == playerState.idle || playerAction == playerState.walking || playerAction == playerState.running || playerAction == playerState.braking)
            {
                anim.Play("Fighter_block");
                playerAction = playerState.blocking;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerAction != playerState.jumping && playerAction != playerState.jumpAttacking && playerAction != playerState.blocking && playerAction != playerState.attacking)
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
                {
                    moveLeftJump();
                }
            }
            else
            {
                if (playerAction != playerState.attacking && playerAction != playerState.blocking)
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
                    if (combo1Stack.Count == 0 && combo2Stack.Count == 0)
                        comboChainNumber = 0;
                }
            }

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
                if (playerAction != playerState.attacking && playerAction != playerState.blocking)
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
                    if (combo1Stack.Count == 0 && combo2Stack.Count == 0)
                        comboChainNumber = 0;
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(isAtDoor == true)
            {
                doorScript.fadeOut();
                return;
            }
            if (playerAction == playerState.idle || playerAction == playerState.walking)
            {
                touchingPos = transform.position - new Vector3((-GetComponent<BoxCollider2D>().bounds.extents.x), (GetComponent<BoxCollider2D>().bounds.extents.y * 0.5f));
                touchingPos2 = transform.position - new Vector3((GetComponent<BoxCollider2D>().bounds.extents.x), (GetComponent<BoxCollider2D>().bounds.extents.y * 0.5f));
                
                RaycastHit2D hit = Physics2D.Raycast(touchingPos, -Vector3.up, 0.18f, 1 << 8);
                RaycastHit2D hit2 = Physics2D.Raycast(touchingPos2, -Vector3.up, 0.18f, 1 << 8);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "platform")
                    {
                        //hit.collider.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
                        hit.collider.GetComponent<Platform>().startDescendCoRoutine();
                        playerAction = playerState.jumping;
                        return;
                    }
                }else if(hit2.collider != null)
                {
                    if (hit2.collider.tag == "platform")
                    {
                        //hit2.collider.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
                        hit2.collider.GetComponent<Platform>().startDescendCoRoutine();
                        playerAction = playerState.jumping;
                        return;
                    }
                }

                if (isAllowedToFight)
                {
                    if (echipDeployable == true)
                    {
                        playerAction = playerState.attacking;
                        anim.Play("Fighter_EChip1");
                    }
                }
                else
                {
                    startEchipDelay();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.V)) //V ATTACK IF IN AIR
        {
            if (isAllowedToFight)
            {
                if (playerAction == playerState.jumping)
                {
                    anim.Play("Fighter_jumpAtk1");
                    playerAction = playerState.jumpAttacking;
                    if (sr.flipX == false)
                        performJumpAttackDamage(1, Vector2.zero, 'H'); // Does not change in mid air
                    else
                        performJumpAttackDamage(0, Vector2.zero, 'H');
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (isAllowedToFight)
            {
                if (playerAction == playerState.jumping)
                {
                    anim.Play("Fighter_jumpAtk2");
                    playerAction = playerState.jumpAttacking;
                    if (sr.flipX == false)
                    {
                        var hitDir = (2f * Vector2.right) + (-2f * Vector2.up);
                        attackMove(hitDir);
                        performJumpAttackDamage(4, Vector3.zero, 'D');
                    }
                    else
                    {
                        var hitDir = (-2f * Vector2.right) + (-2f * Vector2.up);
                        attackMove(hitDir);
                        performJumpAttackDamage(5, Vector3.zero, 'D');
                    }
                }
            }
        }

        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (playerAction != playerState.jumping && playerAction != playerState.sliding && playerAction != playerState.jumpAttacking && playerAction != playerState.blocking && playerAction != playerState.attacking)
            {
                anim.SetBool("WalkTrigger", false);
                anim.SetBool("RunTrigger", false);
                playerAction = playerState.idle;

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    StopCoroutine("runCancelDelay");
                }
                else
                {
                    StartCoroutine("runCancelDelay");
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (isAllowedToFight)
            {
                if (playerAction == playerState.blocking)
                {
                    anim.Play("Figher_idle");
                    playerAction = playerState.idle;
                }
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

        if (playerAction == playerState.jumping || playerAction == playerState.sliding || playerAction == playerState.jumpAttacking || playerAction == playerState.blocking)
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

        if (!isAllowedToFight)
            return;
        
        if (Input.GetKeyDown(KeyCode.V)) //V ATTACK IF NOT IN AIR
        {
            if (playerAction != playerState.attacking && playerAction != playerState.running)
            {
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
                        {
                            setTriggerForAttack(1, Vector3.right + Vector3.up, 'H', "kick1");
                            attackMove(Vector2.right);
                        }
                        else
                        {
                            setTriggerForAttack(0, -Vector3.right + Vector3.up, 'H', "kick1");
                            attackMove(-Vector2.right);
                        }

                    }
                    else
                    {
                        anim.Play("Fighter_punch1");
                        playerAction = playerState.attacking;
                        isRunning = false;
                        if (sr.flipX == false)
                        {
                            if (comboChainNumber == 1)
                            {
                                setTriggerForAttack(1, Vector3.right, 'B', "punch1");
                                attackMove(Vector2.right);
                            }
                            else
                                setTriggerForAttack(1, Vector3.zero, 'B', "punch1");
                        }
                        else
                        {
                            if (comboChainNumber == 1)
                            {
                                setTriggerForAttack(0, Vector3.right, 'B', "punch1");
                                attackMove(-Vector2.right);
                            }
                            else
                                setTriggerForAttack(0, Vector3.zero, 'B', "punch1");
                        }
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
                {
                    setTriggerForAttack(1, Vector3.right, 'H', "dashpunch");
                    attackMove(Vector2.right);
                }
                else
                {
                    setTriggerForAttack(0, Vector3.right, 'H', "dashpunch");
                    attackMove(-Vector2.right);
                }
            }
            else if (playerAction == playerState.attacking)
            {
                stackIDToCheck = 1;
                addToComboStack(1);
            }

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (playerAction == playerState.idle || playerAction == playerState.walking)
                {
                    playerAction = playerState.attacking;
                    dischargeCombo++;
                    anim.Play("Fighter_discharge");
                }
            }
            else if (playerAction != playerState.attacking && playerAction != playerState.running)
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
                        {
                            var hitDir = Vector3.right + (Vector3.up * 4f);
                            setTriggerForAttack(1, hitDir, 'J', "kick3");
                            attackMove(Vector2.right);
                        }
                        else
                        {
                            var hitDir = -Vector3.right + (Vector3.up * 4f);
                            setTriggerForAttack(0, hitDir, 'J', "kick3");
                            attackMove(-Vector2.right);
                        }
                    }
                    else
                    {
                        anim.Play("Fighter_punch1");
                        playerAction = playerState.attacking;
                        isRunning = false;
                        if (sr.flipX == false)
                        {
                            setTriggerForAttack(1, Vector3.right, 'C', "punch1");
                            attackMove(Vector2.right);
                        }
                        else
                        {
                            setTriggerForAttack(0, -Vector3.right, 'C', "punch1");
                            attackMove(-Vector2.right);
                        }
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
                {
                    setTriggerForAttack(1, Vector3.right, 'G', "dashkick");
                    attackMove(Vector2.right);
                }
                else
                {
                    setTriggerForAttack(0, -Vector3.right, 'G', "dashkick");
                    attackMove(-Vector2.right);
                }
            }
            else if (playerAction == playerState.attacking)
            {
                stackIDToCheck = 2;
                addToComboStack(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (playerAction == playerState.idle || playerAction == playerState.walking)
            {
                anim.Play("Fighter_special1");
                playerAction = playerState.attacking;
                isRunning = false;
                if (sr.flipX == false)
                {
                    setTriggerForAttack(1, Vector3.right, 'C', "special");
                    attackMove(Vector2.right);
                }
                else
                {
                    setTriggerForAttack(0, -Vector3.right, 'C', "special");
                    attackMove(-Vector2.right);
                }
            }
        }
    }

    //Multiplayer packet info
    private playerState previousSentState;
    private string currentSentAnim;
    private int playerFlipped;

    public IEnumerator positionSendToPacket()
    {
        while (PlayerAuthenticator.instance.connectedToSession)
        {
            switch (playerAction)
            {
                    case playerState.attacking:
                        if (previousSentState == playerState.attacking)
                        {
                            currentSentAnim = "null";
                            break;
                        }

                        previousSentState = playerState.attacking;
                 //       currentSentAnim = "Fighter_punch1";
                        break;
                    case playerState.blocking:
                        if (previousSentState == playerState.blocking)
                        {
                            currentSentAnim = "null";
                            break;
                        }
                        previousSentState = playerState.blocking;
               //         currentSentAnim = "Fighter_block";
                        break;
                    case playerState.idle:
                        if (previousSentState == playerState.idle)
                        {
                            currentSentAnim = "null";
                            break;
                        }
                        previousSentState = playerState.idle;
                        currentSentAnim = "Enemy_idle";
                        break;
                    case playerState.jumpAttacking:
                        if (previousSentState == playerState.jumpAttacking)
                        {
                            currentSentAnim = "null";
                            break;
                        }

                        previousSentState = playerState.jumpAttacking;
                   //     currentSentAnim = "Fighter_jumpAtk1";
                        break;
                    case playerState.jumping:
                        if (previousSentState == playerState.jumping)
                        {
                            currentSentAnim = "null";
                            break;
                        }
                        previousSentState = playerState.jumping;
                        currentSentAnim = "Enemy_jump";
                        break;
                    case playerState.running:
                        if (previousSentState == playerState.running)
                        {
                            currentSentAnim = "null";
                            break;
                        }

                        previousSentState = playerState.running;
                        currentSentAnim = "Enemy_run";
                        break;
                    case playerState.sliding:
                        if (previousSentState == playerState.sliding)
                        {
                            currentSentAnim = "null";
                            break;
                        }

                        previousSentState = playerState.sliding;
                        currentSentAnim = "Enemy_wallSlide";
                        break;
                    case playerState.walking:
                        if (previousSentState == playerState.walking)
                        {
                            currentSentAnim = "null";
                            break;
                        }

                        previousSentState = playerState.walking;
                        currentSentAnim = "Enemy_walk";
                        break;
                    case playerState.rolling:
                        if (previousSentState == playerState.rolling)
                        {
                            currentSentAnim = "null";
                            break;
                        }

                        previousSentState = playerState.rolling;
                        currentSentAnim = "Enemy_roll";
                        break;
            }

            switch (sr.flipX)
            {
                 case true:
                     playerFlipped = 1;
                     break;
                 case false:
                     playerFlipped = 0;
                     break;
            }
            
            yield return new WaitForSeconds(Time.deltaTime * 5);
            PlayerAuthenticator.instance.sendMovementPacket(transform.position, currentSentAnim, playerFlipped);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchingPos = transform.position - new Vector3((-GetComponent<BoxCollider2D>().bounds.extents.x), (GetComponent<BoxCollider2D>().bounds.extents.y * 0.5f));
        touchingPos2 = transform.position - new Vector3((GetComponent<BoxCollider2D>().bounds.extents.x), (GetComponent<BoxCollider2D>().bounds.extents.y * 0.5f));
        if (collision.collider.gameObject.layer == 9)
        {
            if (playerAction == playerState.rolling)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchingPos, -Vector3.up, 0.18f, 1 << 8);
                RaycastHit2D hit2 = Physics2D.Raycast(touchingPos2, -Vector3.up, 0.18f, 1 << 8);
                if (hit.collider == null && hit2.collider == null)
                {
                    StopCoroutine("waitForRoll");
                    playerAction = playerState.sliding;
                    anim.Play("Fighter_wallSlide");
                    prevState = playerState.sliding;
                    StartCoroutine("rollDelay");
                }
            }
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
                if (hurtType != 'H')
                {
                    RaycastHit2D hit = Physics2D.Raycast(touchingPos, -Vector3.up, 0.18f, 1 << 8);
                    RaycastHit2D hit2 = Physics2D.Raycast(touchingPos2, -Vector3.up, 0.18f, 1 << 8);

                    if (hit.collider != null || hit2.collider != null)
                    {
                        playerAction = playerState.idle;
                        anim.Play("Figher_idle");
                        prevState = playerState.idle;
                        anim.SetBool("WalkTrigger", false);
                        anim.SetBool("RunTrigger", false);
                    }
                }
            }
            else
            {
                prevState = playerState.idle;
            }
            numbOfJumps = 0;
            specialCombo = 0;
            dischargeCombo = 0;
            if (hurtType != 'H')
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