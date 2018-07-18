using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBattleMenuManager : MonoBehaviour
{

	public GameObject[] slots;
	[HideInInspector]
	public bool[] isSlotUsed;

	private void OnEnable()
	{
		isSlotUsed = new bool[slots.Length];
	}

	public void setNextAvailableName(string name)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (!isSlotUsed[i])
			{
				isSlotUsed[i] = true;
				slots[i].GetComponentInChildren<Text>().text = name;
				return;
			}
		}
	}

	public void removeName(int numb)
	{
		numb = numb - 1;
		slots[numb].GetComponentInChildren<Text>().text = "";
		isSlotUsed[numb] = false;
	}

	public void cleanSlots()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			isSlotUsed[i] = false;
			slots[i].GetComponentInChildren<Text>().text = "";
		}
	}
}
