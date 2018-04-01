using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatTest : MonoBehaviour {

    public Material mat;
	void Start () {
        //mat = GetComponent<Material>();
        mat.SetFloat("FirstBar", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
