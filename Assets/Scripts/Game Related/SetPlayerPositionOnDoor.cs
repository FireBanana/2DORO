using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPlayerPositionOnDoor : MonoBehaviour
{

	public static Vector3 lastPosition;

	public GameObject player;

	private void Start()
	{
		if (SceneManager.GetActiveScene().name == "Hallway")
		{
			if (lastPosition != Vector3.zero)
			{
				player.transform.position = lastPosition;
				Camera.main.GetComponent<ProCamera2D>().MoveCameraInstantlyToPosition(player.transform.position);
			}
		}
	}

	public void setLastPos(Vector3 newPos)
	{
		if (SceneManager.GetActiveScene().name == "Hallway")
		{
			lastPosition = newPos;
			print(lastPosition);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
