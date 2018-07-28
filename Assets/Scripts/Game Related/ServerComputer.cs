using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerComputer : MonoBehaviour
{
    private bool playerInside;
    private bool isCreator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (playerInside)
            {
                openBrowser();
                DataHolder.Instance.fighter.isChatting = true;
                DataHolder.Instance.fighter.completeAttack();
                playerInside = false;
            }
        }
    }

    public void closeWindows()
    {
        if (DataHolder.Instance.roomWaitingScreen.activeInHierarchy)
        {
            DataHolder.Instance.roomWaitingScreen.SetActive(false);
            DataHolder.Instance.fighter.isChatting = false;
            PlayerAuthenticator.instance.leaveBattleMatch();
        }

        if (DataHolder.Instance.serverBrowserScreen.activeInHierarchy)
        {
            DataHolder.Instance.serverBrowserScreen.SetActive(false);
            DataHolder.Instance.fighter.isChatting = false;
        }

        playerInside = true;
        isCreator = false;
    }
    
    public void createRoom()
    {
        isCreator = true;
            DataHolder.Instance.serverBrowserScreen.SetActive(false);
            DataHolder.Instance.roomWaitingScreen.SetActive(true);
            DataHolder.Instance.roomWaitingScreen.GetComponent<CreateBattleMenuManager>().cleanSlots();
            PlayerAuthenticator.instance.createBattleMatch();
    }
		    
    public void openBrowser()
    {
        DataHolder.Instance.serverBrowserScreen.SetActive(true);
        DataHolder.Instance.serverBrowserScreen.GetComponent<ServerBrowserManager>().cleanSlots();
        PlayerAuthenticator.instance.searchBattleMatches();
    }

    public void initiateMatch()
    {
        if(isCreator)
        PlayerAuthenticator.instance.initiateBattleMatch();
    }

    public void closeMatchOpenBrowser()
    {
        isCreator = false;
        PlayerAuthenticator.instance.leaveBattleMatch();
        DataHolder.Instance.roomWaitingScreen.SetActive(false);
        openBrowser();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerInside = false;
    }
}