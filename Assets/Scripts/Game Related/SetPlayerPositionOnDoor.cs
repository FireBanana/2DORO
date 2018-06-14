using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPositionOnDoor : MonoBehaviour
{

	public static Vector3 lastPosition;

	public GameObject player;

	private void Start()
	{
		if(lastPosition != Vector3.zero)
		player.transform.position = lastPosition;
	}

	public void setLastPos(Vector3 newPos)
	{
		lastPosition = newPos;
		print(lastPosition);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
