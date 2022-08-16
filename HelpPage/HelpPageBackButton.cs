using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpPageBackButton : MonoBehaviour
{
    public Button backButton;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(delegate {TaskWithParameters("BACKBUTTON"); });
    }

    void TaskWithParameters(string key) {
        if (key.Equals("BACKBUTTON"))
        {
            SceneManager.LoadScene("FrontPageScene");
        }
    }
}
