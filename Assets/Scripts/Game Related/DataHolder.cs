using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DataHolder : MonoBehaviour
{

	public GameObject roomWaitingScreen; //The screen where players connect;
	public GameObject serverBrowserScreen;
	public Fighter fighter;
	public AudioClip buttonClickSound;
	private AudioSource soundMaker;
	

	public static DataHolder Instance;
	
	void Start ()
	{
		if (Instance == null)
			Instance = this;

		soundMaker = GetComponent<AudioSource>();
	}


	public void playClickSound()
	{
		soundMaker.clip = buttonClickSound;
		soundMaker.Play();
	}
	
}
