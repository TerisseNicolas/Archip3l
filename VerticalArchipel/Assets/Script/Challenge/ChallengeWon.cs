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
<<<<<<< HEAD
=======
=======
<<<<<<< HEAD
        main.addNotification(ChallengeWon.quantityWon.ToString() + " " + main.translateResourceName(resource) + " envoyés à l'île "+ main.getIslandName(islandToSend) + " !");
=======
        main.addNotification(ChallengeWon.quantityWon.ToString() + " " + main.translateResourceName(resource) + " envoyés à "+ main.getIslandName(islandToSend) + " !");
<<<<<<< HEAD
        GameObject.Find("Network").GetComponent<Client>().sendData("@2" + islandToSend.Split('_')[2] + "394@" + resource + "@" + ChallengeWon.quantityWon.ToString());
        main.addNotification(ChallengeWon.quantityWon.ToString() + " " + main.translateResourceName(resource) + " envoyés à l'île "+ main.getIslandName(islandToSend) + " !");
=======
>>>>>>> bb5098b0765e618c0fc0008ed778bbcef835c4c8
>>>>>>> bc45254f01217a7f150d6e789864a83a228b14c9
>>>>>>> c465f0bc3d3e72eedf890cbe7ead75827e9098be
>>>>>>> 4ff71bb3540c9c8346d7c34cec3e7d6fc6a34861
        ChallengeWon.challengeWonWindowPresent = false;

        ChallengeWon.challengeWonWindowPresent = false;
        Destroy(GameObject.Find("challengeWonCanvas"));
    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime;

    private MetaGesture gesture;
    private Dictionary<int, int> map = new Dictionary<int, int>();

    public override void CancelTouch(TouchPoint touch, bool @return)
    {
        base.CancelTouch(touch, @return);

        map.Remove(touch.Id);
        if (@return)
        {
            TouchHit hit;
            if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
            map.Add(touch.Id, beginTouch(processCoords(hit.RaycastHit.textureCoord), touch.Tags).Id);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchMoved += touchMovedhandler;
            gesture.TouchCancelled += touchCancelledhandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (gesture)
        {
            gesture.TouchBegan -= touchBeganHandler;
            gesture.TouchMoved -= touchMovedhandler;
            gesture.TouchCancelled -= touchCancelledhandler;
            gesture.TouchEnded -= touchEndedHandler;
        }
    }

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        map.Add(touch.Id, beginTouch(processCoords(touch.Hit.RaycastHit.textureCoord), touch.Tags).Id);
        if (TouchTime == 0)
            TouchTime = Time.time;
    }

    private void touchMovedhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        TouchHit hit;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
        moveTouch(id, processCoords(hit.RaycastHit.textureCoord));
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        endTouch(id);
        if (Time.time - TouchTime < 0.5)
        {
            this.OnMouseDownSimulation();
        }
        TouchTime = 0;
    }

    private void touchCancelledhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        cancelTouch(id);
    }
}
