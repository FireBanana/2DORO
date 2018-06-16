using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{

	public GameObject currentSelection;
	public GameObject[] levels;
	public int pointer;
	

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (pointer > 0)
			{
				levels[pointer].SetActive(false);
				pointer--;
				levels[pointer].SetActive(true);
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (pointer < levels.Length - 1)
			{
				levels[pointer].SetActive(false);
				pointer++;
				levels[pointer].SetActive(true);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			switch (pointer)
			{
				case 0:
					if (SceneManager.GetActiveScene().name != "Hallway")
					{
						SceneManager.LoadScene("Hallway");
					}

					break;
				case 4:
					if (SceneManager.GetActiveScene().name != "Armory")
					{
						SceneManager.LoadScene("Armory");
					}

					break;
				case 5:
					if (SceneManager.GetActiveScene().name != "Lobby")
					{
						SceneManager.LoadScene("Lobby");
					}

					break;
			}
			
		}
	}
}
