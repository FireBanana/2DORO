using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine;
using UnityEngine.UI;

public class ServerBrowserManager : MonoBehaviour
{
	public GameObject[] Row1;
	public GameObject[] Row2;
	public GameObject[] Row3;
	public GameObject[] Row4;

	private bool hasInitialized;
	
	[HideInInspector]
	public string[] RowMatchIds;
	[HideInInspector]
	public int[] playersInRoom;
	
	List<GameObject[]> RowContainer = new List<GameObject[]>();
	
	//num
	//creator
	//mode
	//capacity

	private int numbering;
	[HideInInspector]
	public bool[] isRowUsed;

	public List<GSData> totalMatches = new List<GSData>();

	private int counter;

	public void selectMatch(int row)
	{
		if (RowMatchIds[row] == null)
			return;
		if (playersInRoom[row] >= 6)
			return;
		
		PlayerAuthenticator.instance.joinBattleMatch(RowMatchIds[row]);
		DataHolder.Instance.serverBrowserScreen.SetActive(false);
	}

	public void initializeList()
	{
		
		RowMatchIds = new string[4];
		playersInRoom = new int[4];
		
		RowContainer.Add(Row1);
		RowContainer.Add(Row2);
		RowContainer.Add(Row3);
		RowContainer.Add(Row4);

		hasInitialized = true;

	}

	public void populateInitialList()
	{

		numbering = 0;
		
		if (totalMatches.Count == 0)
		{
			print("No matches");
			return;
		}
		
		if (totalMatches.Count > 4)
		{
			for (int i = 0; i < 4; i++)
			{
				RowContainer[i][0].GetComponent<Text>().text = numbering++.ToString();
				RowContainer[i][1].GetComponent<Text>().text = totalMatches[i].GetGSData("data").GetString("Creator");
				RowContainer[i][2].GetComponent<Text>().text = "PVP";

				RowContainer[i][3].GetComponent<Text>().text = totalMatches[i].GetGSData("data").GetInt("PlayerCount") + "/6";

				playersInRoom[i] = (int)totalMatches[i].GetGSData("data").GetInt("PlayerCount");
				RowMatchIds[i] = totalMatches[i].GetGSData("data").GetString("CreatorId");
			}
		}
		else
		{
			for (int i = 0; i < totalMatches.Count; i++)
			{
				RowContainer[i][0].GetComponent<Text>().text = numbering++.ToString();
				RowContainer[i][1].GetComponent<Text>().text = totalMatches[i].GetGSData("data").GetString("Creator");
				RowContainer[i][2].GetComponent<Text>().text = "PVP";

				RowContainer[i][3].GetComponent<Text>().text = totalMatches[i].GetGSData("data").GetInt("PlayerCount") + "/6";
				
				playersInRoom[i] = (int)totalMatches[i].GetGSData("data").GetInt("PlayerCount");
				RowMatchIds[i] = totalMatches[i].GetGSData("data").GetString("CreatorId");
			}
		}
	}

	public void cleanSlots()
	{
		if (!hasInitialized)
			return;
		
		counter = 0;
		numbering = 0;

		for (int i = 0; i < RowMatchIds.Length; i++)
		{
			RowMatchIds[i] = "";
		}
		
		for (int i = 0; i < isRowUsed.Length; i++)
		{
			isRowUsed[i] = false;
		}

		for (int i = 0; i < 4; i++)
		{
			RowContainer[i][0].GetComponent<Text>().text = "";
			RowContainer[i][1].GetComponent<Text>().text = "";
			RowContainer[i][2].GetComponent<Text>().text = "";
			RowContainer[i][3].GetComponent<Text>().text = "";
		}
		
		RowContainer.Clear();
		totalMatches.Clear();
	}
	

}
