﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health = 100;
    public int id;
    public HealthBarHandler healthHandler;
    Animator anim;
    bool flyingHurt = false;
    public bool canBeHurt = true;

    private void Start()
    {
        if (BattleManager.Instance != null)
            healthHandler = BattleManager.Instance.assignHealthBar();
        anim = GetComponent<Animator>();
    }
    
    public bool hasDied()
    {
        if (health <= 0)
            return true;
        else
            return false;
    }

    public void depleteHealth(char hurtType, Vector3 pushDir, float damage)
    {
        healthHandler.healthDepleter(damage);
        health -= damage;
        if (hasDied())
        {
            BattleManager.Instance.gameOver();
        }
        
        switch (hurtType)
        {
            case 'B':
                anim.Play("Enemy_hurtB");
                break;
            case 'C':
            anim.Play("Enemy_hurtC");
                break;
            case 'J':
            flyingHurt = true;
            anim.Play("Enemy_hurtJ");
                break;
            case 'G':
            canBeHurt = false;
            anim.Play("Enemy_hurtG");
                break;
            case 'H':
            canBeHurt = false;
            flyingHurt = true;
            anim.Play("Enemy_hurtH");
                break;
            case 'D':
            anim.Play("Enemy_hurtD");
                break;
            case 'N':
            flyingHurt = true;
            anim.Play("Enemy_hurtN");
                break;
            case 'O':
            flyingHurt = true;
            anim.Play("Enemy_hurtO");
                break;
            case 'E':
            anim.Play("Enemy_hurtE");
            break;
            case 'F':
            anim.Play("Enemy_hurtF");
            break;
        }
        GetComponent<Rigidbody2D>().AddForce(pushDir, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == 8){
        if(flyingHurt == true){
            canBeHurt = false;
            anim.Play("Enemy_hurtFloor");
            flyingHurt = false;
        }
        }
    }

    public void canNowBeHurt(){
        canBeHurt = true;
        PlayerAuthenticator.instance.pausePackets = false;
    }
}
