using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    public GameObject fadeScreen;
    public Transform exitPoint;
    public bool isLevelChangeDoor;
    public GameObject levelMenu;
    public string levelName;
    Fighter fighterScript;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            fighterScript = coll.gameObject.GetComponent<Fighter>();
            fighterScript.isAtDoor = true;
            fighterScript.doorScript = this;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            fighterScript.isAtDoor = false;
            //fighterScript.doorScript = null;
        }
    }

    public void fadeOut()
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Image>().DOFade(1f, 1f).OnComplete(fadeIn);
        fighterScript.completeAttack();
        fighterScript.GetComponent<Animator>().SetBool("WalkTrigger", false);
        fighterScript.GetComponent<Animator>().SetBool("RunTrigger", false);
        fighterScript.enabled = false;
    }

    public void fadeIn()
    {
        if (isLevelChangeDoor)
        {
            var lvlMan = GameObject.Find("LevelManager");
            if(lvlMan != null)
                lvlMan.GetComponent<SetPlayerPositionOnDoor>().setLastPos(fighterScript.transform.position);
            if (levelMenu != null)
            {
                levelMenu.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(levelName);
            }

            return;
        }
        fadeScreen.GetComponent<Image>().DOFade(0f, 1f).OnComplete(fadeInComplete);
        fighterScript.gameObject.transform.position = exitPoint.position;
        Camera.main.GetComponent<ProCamera2D>().MoveCameraInstantlyToPosition(exitPoint.position);
    }

    void fadeInComplete()
    {
        fadeScreen.SetActive(false);
        fighterScript.enabled = true;
    }
}
