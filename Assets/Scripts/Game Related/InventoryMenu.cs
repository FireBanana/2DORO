using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {

    public GameObject[] menuImages;
    int currMenu;
	public void changeSubMenu(int newMenu)
    {
        menuImages[currMenu].SetActive(false);
        currMenu = newMenu;
        menuImages[currMenu].SetActive(true);
    }

    public void closeMenu()
    {
        gameObject.SetActive(false);
    }
}
