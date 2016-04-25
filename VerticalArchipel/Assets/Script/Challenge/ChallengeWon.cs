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



public class ChallengeWon : InputSource
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

    

    public void OnMouseDownSimulation()
    {
        string islandToSend = this.name.Split('-')[1];
        string resource = char.ToUpper(resourceWon[0]).ToString() + resourceWon.Substring(1);
        this.client.sendData("@2" + islandToSend.Split('_')[2] + "394@" + resource + "@" + ChallengeWon.quantityWon.ToString());

        main.addNotification(ChallengeWon.quantityWon.ToString() + " " + main.translateResourceName(resource) + " envoyés à l'île "+ main.getIslandName(islandToSend) + " !");
        ChallengeWon.challengeWonWindowPresent = false;

        ChallengeWon.challengeWonWindowPresent = false;
        Destroy(GameObject.Find("challengeWonCanvas"));
    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime = 0;

    private MetaGesture gesture;


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }
    

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (TouchTime == 0)
            TouchTime = Time.time;
    }
    

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 0.5)
        {
            this.OnMouseDownSimulation();
        }
        TouchTime = 0;
    }
    
}
