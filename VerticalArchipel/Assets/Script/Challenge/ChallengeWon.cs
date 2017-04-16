using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;



public class ChallengeWon : OneTap
{
    static public bool challengeWonWindowPresent = false;
    static public string challengeWonWindowName = string.Empty;


    static public string resourceWon;
    static public int quantityWon;

    private Client client { get; set; }


    void Start()
    {
        this.client = GameObject.Find("Network").GetComponent<Client>();
        ChallengeWon.challengeWonWindowPresent = true;
    }

    public void init(string resourceWon, int quantityWon)
    {
        ChallengeWon.resourceWon = resourceWon;
        ChallengeWon.quantityWon = quantityWon;
        GameObject.Find("RewardValue").GetComponent<Text>().text = quantityWon.ToString();
        GameObject.Find("RewardSprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Resource/" + resourceWon + "Icon");
    }    

    protected override void OnMouseDownSimulation()
    {
        string islandToSend = this.name.Split('-')[1];
        string resource = char.ToUpper(resourceWon[0]).ToString() + resourceWon.Substring(1);
        this.client.sendData("@2" + islandToSend.Split('_')[2] + "394@" + resource + "@" + ChallengeWon.quantityWon.ToString());

        main.addNotification(ChallengeWon.quantityWon.ToString() + " " + main.translateResourceName(resource) + " envoyés à l'île "+ main.getIslandName(islandToSend) + " !");
        ChallengeWon.challengeWonWindowPresent = false;

        ChallengeWon.challengeWonWindowPresent = false;
        Destroy(GameObject.Find("challengeWonCanvas"));
    }
}
