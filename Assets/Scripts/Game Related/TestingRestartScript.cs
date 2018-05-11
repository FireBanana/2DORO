using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Com.LuisPedroFonseca.ProCamera2D;

public class TestingRestartScript : MonoBehaviour {


	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if(Input.GetKeyDown(KeyCode.T)){
            GetComponent<ProCamera2DShake>().Shake();
        }
	}
}
