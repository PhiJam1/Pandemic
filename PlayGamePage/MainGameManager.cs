using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class MainGameManager : MonoBehaviour
{
    public Button startButton;                              //Start button
    public Button driveButton;                              //Action button
    public Button directFlight;                             //Action button
    public Button charterFlight;                            //Action button
    public Button shuttleFlight;                            //Action button
    public Button newResearchStation;                       //Action button
    public Button treatDisease;                             //Action button
    public Button shareKnowledge;                           //Action button
    public Button discoverCure;                             //Action button
    public Button playerOneSelector;                        //Player selector
    public Button playerTwoSelector;                        //Player selector
    public Button playerThreeSelector;                      //Player selector
    public Button playerFourSelector;                       //Player selector
    public Button covidSelector;                            //Disease selector
    public Button ecoliSelector;                            //Disease selector
    public Button bubonicSelector;                          //Disease selector
    public Button rabiesSelector;                           //Disease selector

    //Text Elements
    public TextMeshProUGUI alertInfo;                       //Displayed on info card to alert users
    public TextMeshProUGUI outbreakTrackerDisplay;          //Displays the number of outbreaks 

    //Button click trackers
    private string lastCityClicked;                         //The name of the last city that was pressed
    private int lastClickedSelectorIndex;                   //The 0-3 index of the last clicked player selector button
    private bool[] selectorBtnPressed = new bool[4];        //Boolean to see which player selector button has been pressed

    //In game counters
    private int numInfectionCards = 6;                      //The number of infection cards in a game
    private int currentPlayer;                              //The 0-3 index of the current player
    private int outbreakTracker;                            //0-8 counter for the number of outbreak;
    private int infectionRate;                              //The 2-4 counter for the infection rate
    private int currentResearchStation;                     //A 0-6 counter for the number of research stations have been built
    private bool gameInPlay;                                //boolean used to determine when the game starts
    private bool[] eradicated = new bool[4];                //used to determine when a disease (corresponding to this index) is eradicated
    private Stack unusedCityCards = new Stack();            //stack of shuffled city cards
    private Stack unusedEpidemicCards = new Stack();        //stack of shuffled epidemic cards
    private List<string> usedEpidemicCards = new List<string>();//stack of used epidemic 

    //Gameobject used as UI markers
    public GameObject infectionRateTracker;                 //Hexagon above the number indicating the current infection rate
    public GameObject[] infectionRateBackgrounds;
    public int IRBIndex = 0;
    public GameObject[] eradicatedMarkers;
    public GameObject[] playerMarkers;                      //A triangle representing each player
    public GameObject[] reseachStations;                    //A diamond representing each research station
    public GameObject[] markers;                            //An array of all the marker circles. These indexes correspond with that of the locations array.
    
    //Custom game objects
    List<DMManager> DMScripts = new List<DMManager>();      //A list of the scripts of each marker. This could probaby be made into an array
    public Players[] gamePlayers = new Players[4];          //The array of Player objects 
    public City[] locations =                               //An array of city objects
    {
        new City("Madrid", false, "Ecoli", new string[] {"New York", "London", "Paris", "Algiers", "Sao Paulo"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Atlanta", false, "Ecoli", new string[] {"Chicago", "Miami", "Washington"}, new int[] {1, 1, 1, 1}, new int[] {0, 0, 0, 0}),
        new City("Chicago", false, "Ecoli", new string[] {"Montreal", "Atlanta", "San Francisco", "Los Angeles", "Mexico City"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Montreal", false, "Ecoli", new string[] {"Chicago", "New York", "Washington"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("New York", false, "Ecoli", new string[] {"London", "Madrid", "Washington", "Montreal"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("London", false, "Ecoli", new string[] {"Essen", "Paris", "Madrid", "New York"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("San Francisco", false, "Ecoli", new string[] {"Chicago", "Los Angeles", "Tokyo", "Manila"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Washington", false, "Ecoli", new string[] {"Montreal", "New York", "Atlanta"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Essen", false, "Ecoli", new string[] {"London", "Paris", "Milan", "St. Petersburg"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Milan", false, "Ecoli", new string[] {"Essen", "Paris", "Istanbul"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("St. Petersburg", false, "Ecoli", new string[] {"Essen", "Istanbul", "Moscow"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Paris", false, "Ecoli", new string[] {"London", "Madrid", "Algiers", "Milan", "Essen"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Moscow", false, "Bub", new string[] {"St. Petersburg", "Istanbul", "Tehran"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Tehran", false, "Bub", new string[] {"Karachi", "Baghdad", "Moscow", "Delhi"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Baghdad", false, "Bub", new string[] {"Cairo", "Istanbul", "Tehran", "Karachi", "Riyadh"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Istanbul", false, "Bub", new string[] {"St. Petersburg", "Milan", "Moscow", "Baghdad", "Cairo", "Algiers"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Cairo", false, "Bub", new string[] {"Algiers", "Istanbul", "Baghdad", "Riyadh", "Khartoum"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Algiers", false, "Bub", new string[] {"Madrid", "Paris", "Istanbul", "Cairo"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Karachi", false, "Bub", new string[] {"Tehran", "Baghdad", "Riyadh", "Delhi", "Mumbai"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Kolkata", false, "Bub", new string[] {"Delhi", "Hong Kong", "Bangkok", "Chennai"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Delhi", false, "Bub", new string[] {"Kolkata", "Tehran", "Karachi", "Mumbai", "Chennai"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Riyadh", false, "Bub", new string[] {"Cairo", "Baghdad", "Karachi"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Mumbai", false, "Bub", new string[] {"Karachi", "Delhi", "Chennai"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Chennai", false, "Bub", new string[] {"Mumbai", "Delhi", "Kolkata", "Bangkok", "Jakarta"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Beijing", false, "Rabies", new string[] {"Shanghai", "Seoul"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Seoul", false, "Rabies", new string[] {"Beijing", "Shanghai", "Tokyo"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Sydney", false, "Rabies", new string[] {"Los Angeles", "Manila", "Jakarta"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Shanghai", false, "Rabies", new string[] {"Beijing", "Seoul", "Tokyo", "Taipei", "Hong Kong"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Tokyo", false, "Rabies", new string[] {"San Francisco", "Shanghai", "Seoul", "Osaka"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Hong Kong", false, "Rabies", new string[] {"Shanghai", "Taipei", "Manila", "Ho Chi Minh City", "Bangkok", "Kolkata"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Bangkok", false, "Rabies", new string[] {"Kolkata", "Hong Kong", "Ho Chi Minh City", "Jakarta", "Chennai"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Ho Chi Minh City", false, "Rabies", new string[] {"Jakarta", "Manila", "Hong Kong", "Bangkok"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Jakarta", false, "Rabies", new string[] {"Chennai", "Bangkok", "Ho Chi Minh City", "Sydney"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Osaka", false, "Rabies", new string[] {"Taipei", "Tokyo"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Manila", false, "Rabies", new string[] {"San Francisco", "Taipei", "Hong Kong", "Ho Chi Minh City", "Sydney"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Taipei", false, "Rabies", new string[] {"Osaka", "Shanghai", "Manila", "Hong Kong"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Lagos", false, "Covid", new string[] {"Khartoum", "Kinshasa", "Sao Paulo"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Khartoum", false, "Covid", new string[] {"Cairo", "Kinshasa", "Lagos", "Johannesburg"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Kinshasa", false, "Covid", new string[] {"Khartoum", "Lagos", "Johannesburg"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Johannesburg", false, "Covid", new string[] {"Khartoum", "Kinshasa"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Los Angeles", false, "Covid", new string[] {"San Francisco", "Sydney", "Mexico City", "Chicago"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Mexico City", false, "Covid", new string[] {"Los Angeles", "Chicago", "Lima", "Bogota", "Miami"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Lima", false, "Covid", new string[] {"Santiago", "Bogota", "Mexico City"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Miami", false, "Covid", new string[] {"Washington", "Atlanta", "Mexico City", "Bogota"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Bogota", false, "Covid", new string[] {"Miami", "Mexico City", "Lima", "Buenos Aires", "Sao Paulo"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Santiago", false, "Covid", new string[] {"Lima"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Sao Paulo", false, "Covid", new string[] {"Bogota", "Buenos Aires", "Lagos", "Madrid"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0}),
        new City("Buenos Aires", false, "Covid", new string[] {"Bogota", "Sao Paulo"}, new int[] {0, 0, 0, 0}, new int[] {0, 0, 0, 0})
        
    }; 

    //Called once
    void Start()
    {
        //set up all the buttons
        driveButton.onClick.AddListener(delegate {TaskWithParameters("DRIVEBUTTON"); });
        directFlight.onClick.AddListener(delegate {TaskWithParameters("DIRECTFLIGHT"); });
        charterFlight.onClick.AddListener(delegate {TaskWithParameters("CHARTERFLIGHT"); });
        shuttleFlight.onClick.AddListener(delegate {TaskWithParameters("SHUTTLEFLIGHT"); });
        newResearchStation.onClick.AddListener(delegate {TaskWithParameters("NEWRESEARCHSTATION"); });
        treatDisease.onClick.AddListener(delegate {TaskWithParameters("TREATDISEASE"); });
        shareKnowledge.onClick.AddListener(delegate {TaskWithParameters("SHAREKNOWLEDGE"); });
        discoverCure.onClick.AddListener(delegate {TaskWithParameters("DISCOVERCURE"); });
        startButton.onClick.AddListener(delegate {TaskWithParameters("STARTGAME"); });
        playerOneSelector.onClick.AddListener(delegate {TaskWithParameters("PLAYERONESELECTOR"); });
        playerTwoSelector.onClick.AddListener(delegate {TaskWithParameters("PLAYERTWOSELECTOR"); });
        playerThreeSelector.onClick.AddListener(delegate {TaskWithParameters("PLAYERTHREESELECTOR"); });
        playerFourSelector.onClick.AddListener(delegate {TaskWithParameters("PLAYERFOURSELECTOR"); });
        covidSelector.onClick.AddListener(delegate {TaskWithParameters("COVIDSELECTOR"); });
        ecoliSelector.onClick.AddListener(delegate {TaskWithParameters("ECOLISELECTOR"); });
        bubonicSelector.onClick.AddListener(delegate {TaskWithParameters("BUBONICSELECTOR"); });
        rabiesSelector.onClick.AddListener(delegate {TaskWithParameters("RABIESSELECTOR"); });

        //The list of possbile roles a player can have
        string[] rolesList =
        {
            "Medic", 
            "Operations Expert", 
            "Scientist",
            "Researcher"
        };

        //Shuffle the array of roles. Then create a list of players and assign them a role. 
        shuffle(rolesList, rolesList.Length);

        for (int i = 0; i < gamePlayers.Length; i++) 
        {
            gamePlayers[i] = new Players(rolesList[i]);
        }

        //This will hold all city names. It will be used to shuffle the cards.
        string[] cityCardsDeck = new string[locations.Length];
        List<string> epidemicCardsDeck = new List<string>();


        //Does two seperate things as described below
        for (int i = 0; i < locations.Length; i++)
        {
            //fill up the list of scripts connected to the markers
            DMScripts.Add(markers[i].GetComponent<DMManager>());

            //use as a stack of cards and send to the shuffle() function to shuffle them.
            cityCardsDeck[i] = locations[i].Name;
            epidemicCardsDeck.Add(locations[i].Name);
        }
        
        //shuffle the card name array. Then fill up the city cards with the shuffled names. 
        shuffle(cityCardsDeck, cityCardsDeck.Length); 
        for (int i = 0; i < locations.Length; i++)
        {
            unusedCityCards.Push(cityCardsDeck[i]);
        }

        //reshuffle the card name array. Then fill up the unused epidemic cards with the reshuffled city names

        shuffle(epidemicCardsDeck, epidemicCardsDeck.Count);

        //up the game board by spreading the diseases
        //take cards from unusedEpidemicCards[] and move them to usedEpidemicCards[] as we use them.
        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {                
                int index = getIndexInLocations(epidemicCardsDeck[j]);
                usedEpidemicCards.Add(epidemicCardsDeck[j]);
                epidemicCardsDeck.RemoveAt(j);
                locations[index].addDiseases(locations[index].BaseDisease, i + 1);       
            }
        }

        //Now add in some epidemic cards
        for (int i = 0; i < numInfectionCards; i++)
        {
            epidemicCardsDeck.Add("EPIDEMIC");
        }

        //fillEpidemicList(epidemicCardsDeck);

        shuffle(epidemicCardsDeck, epidemicCardsDeck.Count);

        //fill up the epidemic cards pile
        for (int i = 0; i < epidemicCardsDeck.Count; i++)
        {
            if (epidemicCardsDeck[i].Equals("EPIDEMIC"))
            {
                Debug.Log(epidemicCardsDeck[i] + "  " + i);
            }

            else 
            {
                Debug.Log(epidemicCardsDeck[i]);
            }

            unusedEpidemicCards.Push(epidemicCardsDeck[i]);
        }

        //give each player two city cards to begin with
        for (int i = 0; i < gamePlayers.Length; i++) 
        {
            gamePlayers[i].addCityCard(unusedCityCards.Pop().ToString());
            gamePlayers[i].addCityCard(unusedCityCards.Pop().ToString());
        }

        for (int i = 0; i < eradicatedMarkers.Length; i++)
        {
            eradicatedMarkers[i].gameObject.SetActive(false);
        }

        //reset some in game counters
        infectionRate = 2;
        currentPlayer = 0;
        currentResearchStation = 0;
        outbreakTracker = 0;

        //Get the player selector button ready by reseting all clicks. 
        //also set up the eradicated array
        for (int i = 0;  i < selectorBtnPressed.Length; i++)
        {
            selectorBtnPressed[i] = false;
            eradicated[i] = false;
        }

        //set up some in game UI
        infectionRateTracker.transform.position = infectionRateBackgrounds[IRBIndex].transform.position;
        reseachStations[currentResearchStation].transform.position = markers[1].transform.position;
        currentResearchStation++;
        locations[1].HasResearchStation = true;
    }

    // Update is called once per frame
    void Update()
    {  
        //used to get bottom right hand side selector button input
        int buttonInput = getIndexOfSelectorButtonTrigger();
        if (buttonInput != -1)
        {
            lastClickedSelectorIndex = buttonInput;
        }
         
        //This will change the marker's sprite based on the number of that city's base disease is there.
        //White: 0, Yellow: 1, Orange: 2, Red: 3
        for (int h = 0; h < locations.Length; h++)
        {
            int maxIndex = 0;
            int maxNum = 0;
            for (int i = 0; i < locations[h].Diseases.Length; i++)
            {
                if (locations[h].Diseases[i] > maxNum)
                {
                    maxNum = locations[h].Diseases[i];
                    maxIndex = i;
                }
            }
            DMScripts[h].ChangeSprite(locations[h].Diseases[maxIndex]);
        }

        if (gameInPlay)
        {

            //This will move the players gameobjects to their current city
            //It offsets each player from the city cetner by a small amount;
            for (int j = 0; j < playerMarkers.Length; j++)
            {
               int k = getIndexInLocations(gamePlayers[j].CurrentCity);
               Vector3 temp = markers[k].transform.position;
               temp.x += ((j % 2) == 0) ? 0.1f : -0.1f;
               temp.y +=(j > 0 && j < 3) ? 0.1f : -0.1f;
               playerMarkers[j].transform.position = temp;
            }

            //This grabs the name of the gameobejct the mouse has just clicked
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero);

                if (hit.collider != null) // && hit.collider.gameObject.name.Equals("MouseFollower") == false)
                {
                    lastCityClicked = hit.collider.gameObject.name.Substring(2);                
                }
            }

            for (int i = 0; i < eradicated.Length; i++)
            {
                if (!(eradicated[i]))
                {
                    break;
                }
                alertInfo.text = "You have won the game";
            }

            gameInPlay = (!checkEndGame());
            checkForOutbreaks();
            
            outbreakTrackerDisplay.text = outbreakTracker.ToString();
        }
    }

    void TaskWithParameters(string key)
    {
        switch (key)
        {
            case "DRIVEBUTTON":
                driveAction();
                break;
            
            case "DIRECTFLIGHT":
                directFlightAction();
                break;

            case "CHARTERFLIGHT":
                charterFlightAction();
                break;

            case "SHUTTLEFLIGHT":
                shuttleFlightAction();
                break;

            case "NEWRESEARCHSTATION":
                newRSAction();
                break;

            case "TREATDISEASE":
                treatDiseaseAction();
                break;
           
            case "SHAREKNOWLEDGE":
                shareKnowledgeAction();
                break;
            
            case "DISCOVERCURE":
                discoverCureAction();
                break;
            case "STARTGAME":
                startGame();
                break;

            case "PLAYERONESELECTOR":
                selectorBtnPressed[0] = true;
                break;

            case "PLAYERTWOSELECTOR":
                selectorBtnPressed[1] = true;
                break;

            case "PLAYERTHREESELECTOR":
                selectorBtnPressed[2] = true;
                break;

            case "PLAYERFOURSELECTOR":
                selectorBtnPressed[3] = true;
                break;
            
            case "COVIDSELECTOR":
                selectorBtnPressed[0] = true;
                break;
            
            case "ECOLISELECTOR":
                selectorBtnPressed[1] = true;
                break;
            
            case "BUBONICSELECTOR":
                selectorBtnPressed[2] = true;
                break;

            case "RABIESSELECTOR":
                selectorBtnPressed[3] = true;
                break;
                
        }       
    }

    //To start the game
    private void startGame()
    {
        startButton.gameObject.SetActive(false);
        alertInfo.text = "";
        gameInPlay = true;
        nextMove("Game has started");
    }

    //After clicking on an adjecent city, player clicks on the drive button. This function will check to see 
    //if the lastclickedcity and your current city are adjecent. If so, it will move you there
    private void driveAction()
    {
        int index = getIndexInLocations(gamePlayers[currentPlayer].CurrentCity);
        bool foundAdjCity = false;

        for (int i = 0; i < locations[index].AdjCities.Length; i++)
        {
            if (locations[index].AdjCities[i].Equals(lastCityClicked))
            {
                gamePlayers[currentPlayer].CurrentCity = lastCityClicked;
                gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                foundAdjCity = true;
            }
        }

        nextMove(foundAdjCity ? "You moved to " + lastCityClicked : "Please select an adjecent city");        
    }

    //Go through the player's city cards. If they have the card for lastClickedCity, move them there
    //and take away the card
    private void directFlightAction()
    {
        bool foundDFL = false;
        for (int i = 0; i < gamePlayers[currentPlayer].CityCards.Count; i++)
        {
            if (lastCityClicked.Equals(gamePlayers[currentPlayer].CityCards[i]))
            {
                gamePlayers[currentPlayer].CityCards.RemoveAt(i);
                gamePlayers[currentPlayer].CurrentCity = lastCityClicked;
                gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                foundDFL = true;
            }
        } 
        nextMove(foundDFL ? "You moved to " + lastCityClicked : "You do not have this city card");
    }

    //Go through player's city cards. If they have the card for their current city, move them to lastclicked city
    private void charterFlightAction()
    {
        bool foundCFL = false;
        for (int i = 0; i < gamePlayers[currentPlayer].CityCards.Count; i++) 
        {
            if (gamePlayers[currentPlayer].CurrentCity.Equals(gamePlayers[currentPlayer].CityCards[i]))
            {
                gamePlayers[currentPlayer].CityCards.RemoveAt(i);
                gamePlayers[currentPlayer].CurrentCity = lastCityClicked;
                gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                foundCFL = true;
            }
        }
        nextMove(foundCFL ? "You moved to " + lastCityClicked : "You do not have the city card for your current city");
    }

    //Get the indexes for current city and lastClickedCity. If they both have a research station, make the move
    private void shuttleFlightAction() 
    {

        int currentCityIndex = getIndexInLocations(gamePlayers[currentPlayer].CurrentCity);
        int nextCityIndex = getIndexInLocations(lastCityClicked);

        if (locations[currentCityIndex].HasResearchStation && locations[nextCityIndex].HasResearchStation)
        {
            gamePlayers[currentPlayer].CurrentCity = lastCityClicked;
            gamePlayers[currentPlayer].decreaseNumPlaysLeft();
            nextMove("You moved to " + lastCityClicked);
        }

        else 
        {
            nextMove("Please select a city with a research station from a city with an research station");
        }
    }

    //Make sure there are enough RS left and that the player has the city card for their current city.
    //Then make a RS at their current city
    private void newRSAction()
    {
        bool builtRS = false;
        int currentCityIndex = getIndexInLocations(gamePlayers[currentPlayer].CurrentCity);
        if (currentResearchStation <= 5)
        {
            if (gamePlayers[currentPlayer].Role.Equals("Operations Expert"))
            {
                reseachStations[currentResearchStation].transform.position = markers[currentCityIndex].transform.position;
                currentResearchStation++;
                locations[currentCityIndex].HasResearchStation = true;
                builtRS = true;
                gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                nextMove("You built a research station in " + gamePlayers[currentPlayer].CurrentCity);
            }
            for (int i = 0; i < gamePlayers[currentPlayer].CityCards.Count && (!(builtRS)); i++)
            {
                if (gamePlayers[currentPlayer].CityCards[i].Equals(gamePlayers[currentPlayer].CurrentCity))
                {
                    
                    reseachStations[currentResearchStation].transform.position = markers[currentCityIndex].transform.position;
                    currentResearchStation++;
                    locations[currentCityIndex].HasResearchStation = true;
                    builtRS = true;
                    gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                    gamePlayers[currentPlayer].CityCards.RemoveAt(i);
                    nextMove("You built a research station in " + gamePlayers[currentPlayer].CurrentCity);
                    break;
                }
            }

            if (!(builtRS))
            {
                nextMove("You do not have the city card for you current city");
            }
        }

        else 
        {
            nextMove("You have no more research stations ");
        }
    }

    //Check to see if there is only one disease here. If so, remove that disease
    //If not, player must select which disease to remove
    private void treatDiseaseAction()
    {
        int currentCityIndex = getIndexInLocations(gamePlayers[currentPlayer].CurrentCity);
        int baseDiseaseIndex = locations[currentCityIndex].findIndexOfDisease(locations[currentCityIndex].BaseDisease);
        bool moreThanOneD = false;
        int totalType = 0;

        //If there is only one disease type, this is used to identify which type that is
        int index = 0;

        //see if there is a disease other than baseDisease
        for (int i = 0; i < locations[currentCityIndex].Diseases.Length; i++)
        {
            if (locations[currentCityIndex].Diseases[i] > 0)
            {
                index = i;
                totalType++;
            }
        }

        moreThanOneD = totalType > 1 ? true : false;

        //If there is only one type, make sure that there is more than 0 disease cubes of type index and remove it
        if (!(moreThanOneD))
        {
            
            if (locations[currentCityIndex].Diseases[index] > 0)
            {
                if (gamePlayers[currentPlayer].Role.Equals("Medic"))
                {
                    locations[currentCityIndex].removeAllDisease(index);
                    gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                    nextMove("Removed all disease units (of that type) from " + gamePlayers[currentPlayer].CurrentCity);

                }
                else 
                {
                    locations[currentCityIndex].removeDiseases(index);
                    gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                    nextMove("Removed one disease unit from " + gamePlayers[currentPlayer].CurrentCity);
                }
            }

            else 
            {
                nextMove("There are 0 diseases here");
            }
        } 
        
        //If we got more than one disease type, we must use the selector to figure out which to remove
        else
        {
            int removeDiseasesIndex = getIndexOfSelectorButtonTrigger();

            if (removeDiseasesIndex != -1 && locations[currentCityIndex].Diseases[removeDiseasesIndex] > 0)
            {
                locations[currentCityIndex].removeDiseases(removeDiseasesIndex);
                gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                nextMove("Removed one disease unit from " + gamePlayers[currentPlayer].CurrentCity);
            }

            else
            {
                nextMove("Select disease again");
            }
        }
        
    }

    //if both players are in the same city, players can give/take city cards
    private void shareKnowledgeAction()
    {
        bool gaveCard = false;

        //Make sure they are in the same city
        if (gamePlayers[lastClickedSelectorIndex].CurrentCity.Equals(gamePlayers[currentPlayer].CurrentCity))
        {
            //If either person is a researcher
            if (gamePlayers[currentPlayer].Role.Equals("Researcher") || gamePlayers[lastClickedSelectorIndex].Role.Equals("Researcher"))
            {
                //This is to see if the current player is giving a card
                for (int i = 0; i < gamePlayers[currentPlayer].CityCards.Count; i++)
                {
                    if (gamePlayers[currentPlayer].CityCards[i].Equals(lastCityClicked))
                    {
                        gamePlayers[lastClickedSelectorIndex].CityCards.Add(lastCityClicked);
                        gamePlayers[currentPlayer].CityCards.RemoveAt(i);
                        gaveCard = true;
                        gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                        nextMove("Gave " + gamePlayers[currentPlayer].CityCards[i] + " to Player " + (lastClickedSelectorIndex + 1));
                        break;
                    }
                }
                
                //This is to see if the selected player is giving a card
                for (int i = 0; i < gamePlayers[lastClickedSelectorIndex].CityCards.Count && (!(gaveCard)); i++)
                {
                    if (gamePlayers[lastClickedSelectorIndex].CityCards[i].Equals(lastCityClicked))
                    {
                        gamePlayers[currentPlayer].CityCards.Add(lastCityClicked);
                        gamePlayers[lastClickedSelectorIndex].CityCards.RemoveAt(i);
                        gaveCard = true;
                        gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                        nextMove("Took " + gamePlayers[lastClickedSelectorIndex].CityCards[i] + " from Player " + (lastClickedSelectorIndex + 1));
                        break;
                    }

                }
            }

            //This is if neither person in a researcher
            else
            {
                for (int i = 0; i < gamePlayers[currentPlayer].CityCards.Count; i++)
                {
                    //Make sure the city card that is to be given matches the city they are both in
                    //This is to give a card
                    if (gamePlayers[currentPlayer].CurrentCity.Equals(gamePlayers[currentPlayer].CityCards[i]))
                    {
                        gamePlayers[lastClickedSelectorIndex].CityCards.Add(gamePlayers[currentPlayer].CityCards[i]);
                        gamePlayers[currentPlayer].CityCards.RemoveAt(i);
                        gaveCard = true;
                        gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                        nextMove("Gave " + gamePlayers[currentPlayer].CityCards[i] + " to Player " + (lastClickedSelectorIndex + 1));
                        break;
                    }
                }

                for (int i = 0; (!(gaveCard)) && i < gamePlayers[lastClickedSelectorIndex].CityCards.Count; i++)
                {
                    //Make sure they are in the same city as the city card that is to be given.
                    //This is to take a card
                    if (gamePlayers[currentPlayer].CurrentCity.Equals(gamePlayers[lastClickedSelectorIndex].CityCards[i]))
                    {
                        gamePlayers[currentPlayer].CityCards.Add(gamePlayers[lastClickedSelectorIndex].CityCards[i]);
                        gamePlayers[lastClickedSelectorIndex].CityCards.RemoveAt(i);
                        gaveCard = true;
                        gamePlayers[currentPlayer].decreaseNumPlaysLeft();
                        nextMove("Took " + gamePlayers[lastClickedSelectorIndex].CityCards[i] + " from Player " + (lastClickedSelectorIndex + 1));
                        break;
                    }
                    
                }
            }
        }

        else
        {
            nextMove("Not in the same city");
        }


        if (!(gaveCard))
        {
            nextMove("Error sharing");
        }
        
    }

    //we get the name of the disease players want to eradicate from the selector index. we make sure they have five cards 
    //of that disease. if so, remove the first five cards of that disease. Also, ensure that player is at a research station
    private void discoverCureAction()
    {
       if (locations[getIndexInLocations(gamePlayers[currentPlayer].CurrentCity)].HasResearchStation)
       {
            string DName = getNameOfDisease(lastClickedSelectorIndex);
            int count = 0;
            bool finished = false;
            int cardsNeeded = gamePlayers[currentPlayer].Role.Equals("Scientist") ? 4 : 5;
            

            for (int i = 0; i < gamePlayers[currentPlayer].CityCards.Count; i++)
            {
                if (locations[getIndexInLocations(gamePlayers[currentPlayer].CityCards[i])].BaseDisease.Equals(DName))
                {
                    count++;
                }

                if (count == cardsNeeded)
                {
                    finished = true;
                    eradicated[lastClickedSelectorIndex] = true;
                    eradicatedMarkers[lastClickedSelectorIndex].gameObject.SetActive(true);
                    nextMove(DName + " has been eRADicated");
                    break;
                }
            }

            if (!finished)
            {
                nextMove("You do not have enough cards of the right type");
            }
            count = 0;
            for (int i = 0; finished && count < cardsNeeded; i++)
            {
                if (locations[getIndexInLocations(gamePlayers[currentPlayer].CityCards[i])].BaseDisease.Equals(DName))
                {
                    gamePlayers[currentPlayer].CityCards.RemoveAt(i);
                    i--;
                    count++;
                }
            }
       }

       else
       {
            nextMove("You need to be at a research station");
       }
    }

    private void nextMove (string actionMsg)
    {
        alertInfo.text = actionMsg + ". ";

        //Keep the game going
        if (gamePlayers[currentPlayer].NumPlaysLeft <= 0)
        {
            
            //Give currentplayer 2 new city cards and let alerts know
            alertInfo.text += "\nNew City cards: " + unusedCityCards.Peek();
            gamePlayers[currentPlayer].CityCards.Add(unusedCityCards.Pop().ToString());
            alertInfo.text += " and " + unusedCityCards.Peek();
            gamePlayers[currentPlayer].CityCards.Add(unusedCityCards.Pop().ToString());

            //Pick out cards from the epidemic deck depending on what the infection rate is at.
            //Move those cards to the used pile and add that city's base disease
            //NOT ADJUSTED FOR ERADICATED DISEASES

            alertInfo.text += "\nNew infections: ";
            for (int i = 0; i < infectionRate; i++)
            {
                
                if (unusedEpidemicCards.Peek().Equals("EPIDEMIC"))
                {
                    unusedEpidemicCards.Pop();
                    alertInfo.text += "\nEPIDEMIC TIME\n";
                    alertInfo.text += " +3 diseases at: " + unusedEpidemicCards.Peek();
                    epidemicHandler();
                    break;
                }
                else 
                {
                    int index = getIndexInLocations(unusedEpidemicCards.Peek().ToString());
                    locations[index].addDiseases(locations[index].BaseDisease);
                    usedEpidemicCards.Add(unusedEpidemicCards.Pop().ToString());
                    alertInfo.text += usedEpidemicCards[usedEpidemicCards.Count - 1] + ((i + 1) == infectionRate ? " " : ", ");
                }   
            }

            gamePlayers[currentPlayer].resetNumPlaysLeft();
            currentPlayer = (currentPlayer == 3) ? 0 : ++currentPlayer;
        }

        alertInfo.text += "\nPlayer " + (currentPlayer + 1) + " has " + gamePlayers[currentPlayer].NumPlaysLeft 
        + " moves left. Currently in " + gamePlayers[currentPlayer].CurrentCity;
        

        for (int i = 0; i < locations.Length; i++)
        {
            locations[i].AlrChecked = false;
        }
    }

    private int getIndexOfSelectorButtonTrigger ()
    {
        for (int i = 0;  i < selectorBtnPressed.Length; i++)
        {
            if (selectorBtnPressed[i] == true)
            {
                selectorBtnPressed[i] = false;
                return i;
            }
        }
        return -1;
    }

    private bool checkEndGame ()
    {
        if (outbreakTracker == 8)
        {
            alertInfo.text = "LOST, too many outbreaks";
            return true;
        }

        else if (unusedCityCards.Count < 2)
        {  
            alertInfo.text = "Lost, too few city cards left";
            return true;
            
        }

        int counterC = 0;
        int counterE = 0;
        int counterB = 0;
        int counterR = 0;

        for (int i = 0 ; i < locations.Length; i++)
        {
            counterC += locations[i].Diseases[0];
            counterE += locations[i].Diseases[1];
            counterB += locations[i].Diseases[2];
            counterR += locations[i].Diseases[3];
        }

        if (counterC >= 26 || counterE >= 26 || counterB >= 26 || counterR >= 26)
        {
            alertInfo.text = "Lost, not enough disease cubes";
            return true;
        }

        return false;
    }

    //function used to shuffle a set cards.
    //given the array that needs to be shuffled with n as the length of that array
    public static void shuffle(string []card, int n)
    {
        for (int i = 0; i < n; i++)
        {
            // Random for remaining positions.
            int r = i + Random.Range(0, n - i);

            //swapping the elements
            string temp = card[r];
            card[r] = card[i];
            card[i] = temp;
            
        }
    }

        //function used to shuffle a set cards.
    //given the array that needs to be shuffled with n as the length of that array
    public static void shuffle(List<string> card, int n)
    {
        for (int i = 0; i < n; i++)
        {
            // Random for remaining positions.
            int r = i + Random.Range(0, n - i);

            //swapping the elements
            string temp = card[r];
            card[r] = card[i];
            card[i] = temp;
            
        }
    }

    //function used to shuffle a set cards.
    //given the array that needs to be shuffled with n as the length of that array

    //This will return the index of a city object in array location[] given it's city name as a string
    public int getIndexInLocations(string name) 
    {
        for (int i = 0; i < locations.Length; i++) 
        {
            if (locations[i].Name.Equals(name)) 
            {
                return i;
            }
        }

        //if this executes, there is probably a typo in hard coded data
        Debug.Log("Name could not be found");
        return -1;
    }

    public string getNameOfDisease (int index)
    {
        switch (index)
        {
            case 0:
                return "Covid";

            case 1:
                return "Ecoli";

            case 2:
                return "Bubonic";

            case 3:
                return "Rabies";
        }
        Debug.Log("FUCK");
        return "";
    }

    private void checkForOutbreaks ()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            for (int j = 0; j < locations[i].Diseases.Length; j++)
            {
                if (locations[i].Diseases[j] > 3)
                {
                    nextMove("\nOUTBREAK AT " + locations[i].Name + "\n");
                    locations[i].AlrChecked = true;
                    for (int k = 0; k < locations[i].AdjCities.Length; k++)
                    {
                        if (!(locations[getIndexInLocations(locations[i].AdjCities[k])].AlrChecked))
                        {
                            locations[getIndexInLocations(locations[i].AdjCities[k])].addDiseases(locations[i].BaseDisease);
                        }
                    }
                    locations[i].Diseases[j] = 3;
                    outbreakTracker++;
                }
            }
        }
    }

    //shuffle the used epidemic cards and move them to the unused epidemic cards stack
    //Move the infection rate up
    //Empty used epidemic cards list
    private void epidemicHandler ()
    {
        int index = getIndexInLocations(unusedEpidemicCards.Peek().ToString());
        usedEpidemicCards.Add(unusedEpidemicCards.Pop().ToString());
        locations[index].addDiseases(locations[index].BaseDisease, 3);
        shuffle(usedEpidemicCards, usedEpidemicCards.Count);
        for (int i = 0;  i < usedEpidemicCards.Count; i++)
        {
            unusedEpidemicCards.Push(usedEpidemicCards[i]);
            usedEpidemicCards.RemoveAt(i);
            i--;
        }

        for (int i = 0; i < usedEpidemicCards.Count; i++)
        {
            //Debug.Log(usedEpidemicCards[i]);
        }

        infectionRateTracker.transform.position = infectionRateBackgrounds[++IRBIndex].transform.position;
        if (IRBIndex <= 2) 
        {
            infectionRate = 2; 
        }

        else if (IRBIndex > 4)
        {
            infectionRate = 4;
        }

        else 
        {
            infectionRate = 3;
        }
    }

    private void fillEpidemicList (List<string> Deck)
    {
        /*
        List<string>[] stacksHolder = new List<string>[numInfectionCards];
        for (int i = 0; i < numInfectionCards; i++)
        {
            stacksHolder[i] = new List<string>();
        }

        int j = Deck.Count / numInfectionCards;

        int k = 0;
        for (int i = 0; i < Deck.Count; i++)
        {
            stacksHolder[k].Add(Deck[i]);
            k++;
            if (k == j)
            {
                k = 0;
            }
        }

        for (int i = 0; i < numInfectionCards; i++)
        {
            stacksHolder[i].Add("EPIDEMIC");
            shuffle(stacksHolder[i], stacksHolder[i].Count);
        }

        for (int i = 0; i < numInfectionCards; i++)
        {
            for (int z = 0; z < stacksHolder[i].Count; z++)
            {
                epidemicCardsDeck.Add(stacksHolder[i][z]);
            }
        }
        */
    }

    private void testing()
    {
        gamePlayers[0].CityCards.Add("Washington");
        gamePlayers[0].CityCards.Add("Taipei");
        gamePlayers[0].CityCards.Add("Manila");
        gamePlayers[0].CityCards.Add("Ho Chi Minh City");

        gamePlayers[0].CityCards.Add("Bangkok");



    }
}
