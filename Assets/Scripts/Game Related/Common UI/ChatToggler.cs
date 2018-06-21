using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatToggler : MonoBehaviour
{

    public Sprite closedSprite, openSprite;
    public GameObject textInput;
    private Image imagePlace;
    private bool toggle;
    private InputField inputField;

    private void Start()
    {
        imagePlace = GetComponent<Image>();
        inputField = GetComponent<InputField>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (toggle)
            {
                inputField.ActivateInputField();
                PlayerAuthenticator.instance.fighterScript.isChatting = true;
                textInput.SetActive(true);
                imagePlace.sprite = openSprite;
                toggle = false;
            }
            else
            {
                PlayerAuthenticator.instance.fighterScript.isChatting = false;
                textInput.SetActive(false);
                imagePlace.sprite = closedSprite;
                toggle = true;
            }
        }
        
    }
}
