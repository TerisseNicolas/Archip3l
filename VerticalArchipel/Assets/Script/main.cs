﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TouchScript.InputSources;
using System.Collections.Generic;

public class main : MonoBehaviour
{

    public const int nbChallengesMax = 3;
    public const int nbNotificationsMax = 8;

    static public int level = 0;    //TODO : initialize with Register's value (from Archipel) --> event Client

    private Client Client;

    void Start()
    {
        //SoundPlayer.Instance.playStartGameSound();
        //hiding challenges and notifications at the beginning
        for (int i = 1; i <= nbChallengesMax; i++)
        {
            GameObject.Find("Challenge" + i.ToString()).GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("Challenge" + i.ToString()).GetComponent<BoxCollider>().enabled = false;
        }

        for (int i = 0; i < nbNotificationsMax; i++)
        {
            GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = string.Empty;
            GameObject.Find("Notif" + i.ToString()).GetComponent<BoxCollider>().enabled = false;
        }

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemTeamLevelEvent += Client_MessageSystemTeamLevelEvent;
    }

    private void Client_MessageSystemTeamLevelEvent(object sender, MessageEventArgs e)
    {
        main.level = Int32.Parse((string)e.message.Split('@').GetValue(2));
    }

    static public void removeChallenge(GameObject go)
    {
        go.GetComponent<SpriteRenderer>().enabled = false;
        go.GetComponent<BoxCollider>().enabled = false;
    }

    static public void addNotification(string text)
    {
        for (int i = 0; i < nbNotificationsMax; i++)
            if (GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text == string.Empty)
            {
                GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = addLineBreaks(text);
                GameObject.Find("Notif" + i.ToString()).GetComponent<BoxCollider>().enabled = true;
                return;
            }
        //no place --> make some place to add this notification
        removeNotification(GameObject.Find("Notif1"));
        GameObject.Find("Notif" + (nbNotificationsMax-1).ToString()).GetComponent<Text>().text = text;
        GameObject.Find("Notif" + (nbNotificationsMax - 1).ToString()).GetComponent<BoxCollider>().enabled = true;
    }

    static public void removeNotification(GameObject go)  //id : last character of the notification's name
    {
            
        //each notification below the removed one goes up
        for (int i = int.Parse(go.name[go.name.Length - 1].ToString()); i < nbNotificationsMax - 1; i++)
        {
            GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = GameObject.Find("Notif" + (i + 1).ToString()).GetComponent<Text>().text;
        }
        GameObject.Find("Notif" + (nbNotificationsMax - 1).ToString()).GetComponent<Text>().text = string.Empty;
    }

    static public void addEnigma()
    {
        Canvas enigmaPrefab = Resources.Load<Canvas>("Prefab/Enigma");
        Canvas enigma = Instantiate(enigmaPrefab);
        enigma.name = "Enigma";
        Enigma.enigmaWindowName = enigma.name;
        Enigma.enigmaWindowOpen = true;
    }
    


    static string addLineBreaks(string text)
    {
        const int maxChar = 35;
        List<int> spaces = new List<int>();
        int i = 0;
        foreach (char c in text)
        {
            if (c == ' ')
                spaces.Add(i);
            i++;
        }

        int j = 0;
        i = 1;
        while (maxChar * i <= text.Length)
        {
            while (j < spaces.Count && spaces[j] < maxChar * i)
                j++;
            text = text.Substring(0, spaces[j - 1]) + "\n" + text.Substring(spaces[j - 1]);
            i++;
        }
        return text;
    }

    

    //translation of the resource's name to french
    static public string translateResourceName(string resourceName)
    {
        switch (resourceName)
        {
            case "Gold":
                return "Or";
            case "Stone":
                return "Pierre";
            case "Oil":
                return "Pétrole";
            case "Wood":
                return "Bois";
            case "Manufacture":
                return "Manufacture";
            case "Electricity":
                return "Electricité";
            case "Food":
                return "Nourriture";
            case "Health":
                return "Santé";
            case "Tourism":
                return "Tourisme";
            case "Education":
                return "Education";
            case "Religion":
                return "Religion";
            case "Happiness":
                return "Bonheur";
            default:
                return string.Empty;
        }
    }

    //returns the name of the resource (or stat) produced
    static public string getNameResourceOrStatProduced(string buildingName)
    {
        switch (buildingName)
        {
            case "GoldMine":
                return "Gold";
            case "StoneMine":
                return "Stone";
            case "OilPlant":
                return "Oil";
            case "Sawmill":
                return "Wood";
            case "Factory":
                return "Manufacture";
            case "WindTurbine":
                return "Electricity";
            case "Farm":
                return "Food";
            case "Lab":
                return "Health";
            case "Airport":
                return "Tourism";
            case "Hotel":
                return "Tourism";
            case "Harbor":
                return "Food";
            case "School":
                return "Education";
            case "Church":
                return "Religion";
            case "Cinema":
                return "Happiness";
            case "AmusementPark":
                return "Happiness";
            case "PowerPlant":
                return "Electricity";
            default:
                return string.Empty;
        }
    }

    static public string getIslandName(string sousIle)
    {
        switch (sousIle)
        {
            case "sous_ile_1":
                return "Jaune";
            case "sous_ile_2":
                return "Bleue";
            case "sous_ile_3":
                return "Grise";
            case "sous_ile_4":
                return "Marron";
            default:
                return string.Empty;
        }
    }


}
