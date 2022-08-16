using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FrontPageManager : MonoBehaviour
{
    public Button helpButton;
    public Button startGame;

    // Start is called before the first frame update
    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        helpButton.onClick.AddListener(delegate {TaskWithParameters("HELPBUTTON"); });
        startGame.onClick.AddListener(delegate {TaskWithParameters ("STARTGAMEBUTTON"); });
        
    }

    void TaskWithParameters(string key)
    {   
        if (key.Equals("HELPBUTTON")) {
            SceneManager.LoadScene("HelpPage");
            
        } 
        
        else if (key.Equals("STARTGAMEBUTTON")) {
            SceneManager.LoadScene("PlayGameScene");
        }
    }

    
}
