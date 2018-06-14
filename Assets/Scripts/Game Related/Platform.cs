using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public void startDescendCoRoutine()
    {
        GetComponent<EdgeCollider2D>().enabled = false;
        StartCoroutine(descend());
    }
    
    IEnumerator descend()
    {
        yield return new WaitForSeconds(0.4f);
        //GetComponent<PlatformEffector2D>().rotationalOffset = 0;
        GetComponent<EdgeCollider2D>().enabled = true;
    }
}
