using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoCardPlayerInfo;    //Text element in the info card used to display player info
    [SerializeField] private TextMeshProUGUI infoCardCityName;      //Text element in the info card used to display the name of a city
    [SerializeField] private TextMeshProUGUI infoCardDiseaseCount;  //A text element in the info card used to display disease counts
    [SerializeField] private TextMeshProUGUI alertInfo;             //text element of the contents under ALERT
    [SerializeField] private TextMeshProUGUI alertTitle;            //title for the alerts
    private string temp;
    [SerializeField] private Camera mainCamera;                     //The main (and only) camera
    MainGameManager mainGameManager;                                //grabs the MainGameManager GAMEOBJECT

    //take the MainGameManager.cs SCRIPT from the MainGameManager GAMEOBJECT and assigns it to this instance variable
    void Awake()
    {
        mainGameManager = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();
    }

    //We start the game by showing each players randomly assigned city cards
    void Start()
    {
        infoCardPlayerInfo.text = setPlayerInfo();
    }


    // Update is called once per frame
    void Update()
    {
        //Makes the center of the screen 0,0
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;      //we don't need a z value in a  2d game
       
        //move the gameobject slightly off the tip of the cursor. Without this, this gameobject
        //will sometimes interfere with the raycasts used to detect gameobject clicks
        //This hides just under the tip of the mouse so it is not visible 
        mouseWorldPosition.x += .05f;   
        mouseWorldPosition.y -= .05f;

        //Move this gameobject(the tiny dot) to follow the coords as defined above
        transform.position = mouseWorldPosition;

        if (!(alertInfo.text.Equals("")))
        {
            temp = alertInfo.text;
        }

    }

    //this will detect collisions between this gameobject and the triggers. 
    public void OnTriggerEnter2D( Collider2D other)
    {
        //This will display player info when the question mark is hovered over
        if (other.name.Equals("PlayerInfoTrigger"))
        {
            alertInfo.text = "";
            alertTitle.text = "";
            infoCardPlayerInfo.text = setPlayerInfo();
        }

        //this will occur when the cursor (and thus the gameobject following under it)
        //hover over a marker. That marker's city's disease info will then be displayed on the info card.
        else 
        {
            alertInfo.text = temp;
            alertTitle.text = "Alert";
            infoCardPlayerInfo.text = "";

            //use the name of the collided 'other' arugment to find its index in array locations[].
            //we can acccess location[] from the MainGameManager.cs file by taking its script in the Awake() function as a variable. 
            int index = mainGameManager.getIndexInLocations(other.name.Substring(2));

            infoCardCityName.text = mainGameManager.locations[index].Name;
            infoCardDiseaseCount.text = 
            "Covid: " + mainGameManager.locations[index].Diseases[0] + "                "
            + " Ecoli: " + mainGameManager.locations[index].Diseases[1]
            + "\nBubonic: " + mainGameManager.locations[index].Diseases[2] + "	     "
            + " Rabies:" + mainGameManager.locations[index].Diseases[3];
        }
        
    }

    //Displays player info on the info card
    public string setPlayerInfo ()
    {
        infoCardCityName.text = "Player Info";

        //Player info and infocarddiseasecount takes up the same space so we need to mute the other
        infoCardDiseaseCount.text = "";
        string str = "";

        for (int i = 0; i < mainGameManager.gamePlayers.Length; i++)
        {
            str += "P" + (i+1) + " (" + mainGameManager.gamePlayers[i].Role + "): ";
            for (int j = 0; j < mainGameManager.gamePlayers[i].CityCards.Count; j++)
            {
                str += mainGameManager.gamePlayers[i].CityCards[j];
                str += j + 1 < mainGameManager.gamePlayers[i].CityCards.Count ? ", " : " ";
                
            }
            str += "\n";
        }
        
        return str;
    }
}
