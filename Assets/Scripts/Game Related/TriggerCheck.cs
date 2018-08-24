using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour {

    public bool enemyInside = false;
    public GameObject enemyObject;
    private int totalEnemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemy")
        {
            totalEnemies++;
            enemyInside = true;
            enemyObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            totalEnemies--;
            if(totalEnemies == 0)
                enemyInside = false;
            enemyObject = null;
        }
    }

}
