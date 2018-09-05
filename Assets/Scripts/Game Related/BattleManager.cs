using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

	public static BattleManager Instance;

	public HealthBarHandler[] healthBars;
	public GameObject exitMenu;
	private int time = 300;

	public HealthBarHandler assignHealthBar()
	{
		for (int i = 0; i < healthBars.Length; i++)
		{
			if (!healthBars[i].isUsed)
			{
				healthBars[i].transform.parent.gameObject.SetActive(true);
				healthBars[i].isUsed = true;
				return healthBars[i];
			}
		}

		return null;
	}

	public void returnToLobby()
	{
		SceneManager.LoadScene("Lobby");
	}

	public void gameOver()
	{
		exitMenu.SetActive(true);
		PlayerAuthenticator.instance.fighterScript.isAllowedToFight = false;
	}

	private void Start()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	IEnumerator battleCountDown()
	{
		while (time > 0)
		{
			yield return new WaitForSeconds(1);
			time--;
		}
		
		gameOver();
	}
}
