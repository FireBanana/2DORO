using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Com.LuisPedroFonseca.ProCamera2D;

public class TestingRestartScript : MonoBehaviour {

    public GameObject inventoryMenu;

	void Update () {
      /*  if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }*/
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
