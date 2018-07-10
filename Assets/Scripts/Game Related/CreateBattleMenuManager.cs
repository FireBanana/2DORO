using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBattleMenuManager : MonoBehaviour
{

	public GameObject[] slots;
	[HideInInspector]
	public bool[] isSlotUsed;

	public int myPos;

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
				myPos = i + 1;
				return;
			}
		}
	}
}
