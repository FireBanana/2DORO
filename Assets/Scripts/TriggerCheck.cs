using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour {

    public bool enemyInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "")
        {
            enemyInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "")
        {
            enemyInside = false;
        }
    }

}
