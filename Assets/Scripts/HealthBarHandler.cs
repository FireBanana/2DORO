using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarHandler : MonoBehaviour {

    Material mat;
    Tween currTween;
    float healthVal;

	void Start () {
        mat = GetComponent<Image>().material;
        mat.SetFloat("_SecondBar", 0);
        mat.SetFloat("_FirstBar", 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currTween.Kill();
            healthVal = mat.GetFloat("_SecondBar") + 0.1f;
            currTween = mat.DOFloat(healthVal, "_SecondBar", 0.5f).OnComplete(updateHealthValue);
        }
    }

    void updateHealthValue()
    {
        mat.DOFloat(healthVal, "_FirstBar", 0.7f);
    }
}
