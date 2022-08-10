using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players
{
    private string currentCity;                             //The name of the city they are in
    private string role;                                    //Their unique random role
    private int numPlaysLeft;                               //The number of plays this person has left. Starts at 4
    private List<string> cityCards = new List<string>();    //The list of city cards in their deck

    //A constructor. Everyone starts in Atlanta with 4 numPlaysLeft and is given a randomized role.
    //Arugemnt _role is already randomized as preformed in MainGameManager.cs
    public Players (string _role)
    {
        currentCity = "Atlanta";
        numPlaysLeft = 4;
        role = _role;
    }

    //Getters and setters for the instance variables. Not all will have/need a setter.
    public string CurrentCity
    {
        get { return currentCity; }
        set { currentCity = value; }
    }

    public string Role
    {
        get { return role; }
    }

    public int NumPlaysLeft
    {
        get { return numPlaysLeft; }
    }

    public List<string> CityCards
    {
        get { return cityCards; }
    }

    //This will decrease the number of plays a player has by one. 
    public void decreaseNumPlaysLeft() 
    {
        numPlaysLeft--;
    }

    //This will add a city name to the end of the cityCards list
    public void addCityCard (string newCityCard) 
    {
        cityCards.Add(newCityCard);
    }

    //This will reset a player numPlaysleft. To be used at the end of their turn
    public void resetNumPlaysLeft()
    {
        numPlaysLeft = 4;
    }
}
