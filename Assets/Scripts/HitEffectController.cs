using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectController : MonoBehaviour {

	Animator anim;
	void Start () {
		anim = GetComponent<Animator>();
		anim.Play("hitEffect");
	}
	
	public void whenDone(){
		Destroy(gameObject);
	}
}
