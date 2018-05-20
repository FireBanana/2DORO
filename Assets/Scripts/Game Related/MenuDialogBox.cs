using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDialogBox : MonoBehaviour {

    public PlayerAuthenticator pa;
    int numb;
    string strNumb;

    
	public void updateNumb(string s)
    {
        strNumb = s;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            numb = int.Parse(strNumb);
            pa.verifyCaptcha(numb);
        }
	}
}
