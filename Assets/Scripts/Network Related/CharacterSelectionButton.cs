using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionButton : MonoBehaviour
{

    public Sprite selectedSprite, nonSelectedSprite; //interchanged
    public int row;
    public PlayerAuthenticator pa;
    Image buttonImage;
    public string nameID;

    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    public void ButtonPressed()
    {
        pa.characterEdited(this, row);
    }

    public void changeToNonSelected()
    {
        buttonImage.sprite = selectedSprite;
    }

    public void changeToSelected()
    {
        buttonImage.sprite = nonSelectedSprite;
    }
}
