using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Com.LuisPedroFonseca.ProCamera2D;

public class TestingRestartScript : MonoBehaviour {

    public GameObject inventoryMenu;

	void Update ()
	{

	    if (PlayerAuthenticator.instance == null)
	    {
	        print("Player Authenticator does not exist");
	        return;
	    }

	    if (PlayerAuthenticator.instance.fighterScript.isChatting)
	        return;

		if (Input.GetKeyDown(KeyCode.P)) //Search matches for fighting
		{
			if (DataHolder.Instance.roomWaitingScreen.activeInHierarchy)
			{
				DataHolder.Instance.roomWaitingScreen.SetActive(false);
				return;
			}
			else
			{
				DataHolder.Instance.roomWaitingScreen.SetActive(true);
				PlayerAuthenticator.instance.createBattleMatch();
			}
		}
		    
		if (Input.GetKeyDown(KeyCode.L))
		{
			if (DataHolder.Instance.serverBrowserScreen.activeInHierarchy)
			{
				DataHolder.Instance.serverBrowserScreen.SetActive(false);
				return;
			}
			else
			{
				DataHolder.Instance.serverBrowserScreen.SetActive(true);
			}
			PlayerAuthenticator.instance.searchBattleMatches();
		}
			
	    
        if(Input.GetKeyDown(KeyCode.T)){
            GetComponent<ProCamera2DShake>().Shake();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryMenu.activeInHierarchy)
                inventoryMenu.SetActive(false);
            else
            {
                if(PlayerAuthenticator.instance.menuNameText.text == " ")
                {
                    PlayerAuthenticator.instance.setInventoryInfo();
                }
                inventoryMenu.SetActive(true);
            }
        }
    }
}
