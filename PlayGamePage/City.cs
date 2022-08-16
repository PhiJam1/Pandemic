using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    private string name;             //name of the city

    private string baseDisease;      //the home color/disease of the city

    private bool hasResearchStation; //is there a research station on this city

    private string[] adjCities;      //cities adjecent to this city. 

    private int[] playersIn;         //players in this city. [0, 0, 0, 0] -> The index 0 is player one, so on. A one means there is that player there. 

    private int[] diseases;          //list of diseases in a city [covid, ecoli, bub, rabies] 

    private bool alrChecked;

    //constructor
    public City(string _Name, bool _HasResearchStation, string _BaseDisease, string[] _AdjCities, int[] _PlayersIn, int[] _Diseases)
    {
        name = _Name;
        hasResearchStation = _HasResearchStation;
        baseDisease = _BaseDisease;
        alrChecked = false;

        adjCities = new string[_AdjCities.Length];
        for (int i = 0; i < _AdjCities.Length; i++) 
        {
            adjCities[i] = _AdjCities[i];
        }

        playersIn = new int[_PlayersIn.Length];
        for (int i = 0; i < _PlayersIn.Length; i++) 
        {
            playersIn[i] = _PlayersIn[i];
        }

        diseases = new int[_Diseases.Length];
        for (int i = 0; i < _Diseases.Length; i++) 
        {
            diseases[i] = _Diseases[i];
        }   
    }

    //A set of getters and setters for instance variables. Not all of them have/need setters
    public string Name
    {
        get { return name; }
    }

    public string BaseDisease
    {
        get { return baseDisease; }
    }

    public bool HasResearchStation
    {
        get { return hasResearchStation; }
        set { hasResearchStation = value; }
    }

    public bool AlrChecked
    {
        get { return alrChecked; }
        set { alrChecked = value; }
    }

    public string[] AdjCities
    {
        get { return adjCities; }
    }

    public int[] PlayersIn
    {
        get { return playersIn; }
    }

    public int[] Diseases
    {
        get { return diseases; }
    }

    //used to remove a player from a city. The index is the the player number (Which player we are removing)
    public void removePlayer (int index)
    {
        playersIn[index] = 0;
    }

    //used to add a player to a city. The index is the the player number (Which player we are adding)
    public void addPlayer (int index) 
    {
        playersIn[index] = 1;
    }

    //used to remove one disease cube from this city given its name
    public void removeDiseases (string diseaseName) 
    {
        diseases[findIndexOfDisease(diseaseName)]--;
    }

    public void removeAllDisease (int index)
    {
        diseases[index] = 0;
    }

    //Overloaded function to remove a disease given its index
    public void removeDiseases (int index)
    {
        diseases[index]--;
    }

    //used to add one disease cube to this city given its name
    public void addDiseases (string diseaseName) 
    {
        diseases[findIndexOfDisease(diseaseName)]++;
    }

    //overloaded function that will add diseases to a city. It takes a string as the disease name
    //and a second arugement as the number of times that disease will be increased by. 
    public void addDiseases (string diseaseName, int numTimes)
    {
        diseases[findIndexOfDisease(diseaseName)] += numTimes;
    }

    //Given a disease's name as a string, this function will return its index value in array diseases[]
    public int findIndexOfDisease(string diseaseName)
    {
        if(diseaseName.Equals("Covid"))
        {
            return 0;
        }
        else if (diseaseName.Equals("Ecoli"))
        {
            return 1;
        }
        else if (diseaseName.Equals("Bub"))
        {
            return 2;
        }
        else //the name is "Rabies"
        {
            return 3;
        }
    }
}
