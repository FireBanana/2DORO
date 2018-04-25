using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;
    public HealthBarHandler healthHandler;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void depleteHealth(char hurtType, Vector3 pushDir)
    {
        // healthHandler.healthDepleter();
        switch (hurtType)
        {
            case 'B':
                anim.Play("Fighter_hurtB");
                break;
            case 'C':
            anim.Play("Fighter_hurtC");
                break;
            case 'J':
            anim.Play("Fighter_hurtJ");
                break;
            case 'G':
            anim.Play("Fighter_hurtG");
                break;
            case 'K':
                break;
            case 'O':
                break;
            case 'F':
                break;
            case 'S':
                break;
        }
        GetComponent<Rigidbody2D>().AddForce(pushDir, ForceMode2D.Impulse);
    }
}
