using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMManager : MonoBehaviour
{
    public Sprite[] spriteArray;            //This is the list of sprites as shown in the Inspector for each marker gameobject
    public SpriteRenderer spriteRenderer;   //This is a component of each marker gameobject used to change its sprite

    //changes the sprite on the disease markers gameobject for each city
    //0 - white, 1 - yellow, 2 - orange, 3 - red
    //To be called from the MainGameManager.cs file only and act on its own marker that this script is connected to. 
    public void ChangeSprite(int colorIndex)
    {
        spriteRenderer.sprite =  (colorIndex >= 3) ? spriteArray[3] : spriteArray[colorIndex];
    }
}
