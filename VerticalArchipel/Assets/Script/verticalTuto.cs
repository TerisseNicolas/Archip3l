using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;
using TouchScript;

public class verticalTuto : InputSource
{
    static public GameObject resourcesTuto;     //step 4
    static public GameObject islandsTuto;       //step 5
    static public GameObject timeTuto;          //step 3
    static public GameObject trophiesTuto;      //step 2
    static public GameObject challengesTuto;    //step 1
    static public GameObject notificationsTuto; //step 0

    static public GameObject resourcesTutoText;  
    static public GameObject islandsTutoText;     
    static public GameObject timeTutoText;      
    static public GameObject trophiesTutoText;    
    static public GameObject challengesTutoText; 
    static public GameObject notificationsTutoText;
    static public GameObject tutoTermine;

    static public int steps = 0;

    void OnMouseDownSimulation()
    {
        switch(verticalTuto.steps)
        {
            case 0:
                verticalTuto.notificationsTuto.SetActive(true);
                verticalTuto.notificationsTutoText.SetActive(true);
                verticalTuto.steps++;
                GameObject.Find("verticalBackground").GetComponent<BoxCollider>().enabled = false;
                break;
            case 1:
                if (this.name == "NotificationsTuto")
                {
                    verticalTuto.challengesTuto.SetActive(true);
                    verticalTuto.notificationsTuto.SetActive(false);
                    verticalTuto.challengesTutoText.SetActive(true);
                    verticalTuto.notificationsTutoText.SetActive(false);
                    verticalTuto.steps++;
                }
            break;
            case 2:
                if (this.name == "ChallengesTuto")
                {
                    verticalTuto.trophiesTuto.SetActive(true);
                    verticalTuto.challengesTuto.SetActive(false);
                    verticalTuto.trophiesTutoText.SetActive(true);
                    verticalTuto.challengesTutoText.SetActive(false);
                    verticalTuto.steps++;
                }
                    break;
            case 3:
                if (this.name == "TrophiesTuto")
                {
                    verticalTuto.timeTuto.SetActive(true);
                    verticalTuto.trophiesTuto.SetActive(false);
                    verticalTuto.timeTutoText.SetActive(true);
                    verticalTuto.trophiesTutoText.SetActive(false);
                    verticalTuto.steps++;
                }
                    break;
            case 4:
                if (this.name == "TimeTuto")
                {
                    verticalTuto.resourcesTuto.SetActive(true);
                    verticalTuto.timeTuto.SetActive(false);
                    verticalTuto.resourcesTutoText.SetActive(true);
                    verticalTuto.timeTutoText.SetActive(false);
                    verticalTuto.steps++;
                }
                    break;
            case 5:
                if (this.name == "ResourcesTuto")
                {
                    verticalTuto.islandsTuto.SetActive(true);
                    verticalTuto.resourcesTuto.SetActive(false);
                    verticalTuto.islandsTutoText.SetActive(true);
                    verticalTuto.resourcesTutoText.SetActive(false);
                    verticalTuto.steps++;
                }
                break;
            case 6:
                verticalTuto.islandsTuto.SetActive(false);
                verticalTuto.islandsTutoText.SetActive(false);
                verticalTuto.tutoTermine.SetActive(true);
                //TODO: send event to table to say verticalTuto is finished

                break;
        }
    }


    void Awake()
    {
        if (this.name == "verticalBackground")  //doing it just one time
        {
            verticalTuto.resourcesTuto = GameObject.Find("ResourcesTuto");
            verticalTuto.resourcesTuto.SetActive(false);
            verticalTuto.islandsTuto = GameObject.Find("IslandsTuto");
            verticalTuto.islandsTuto.SetActive(false);
            verticalTuto.timeTuto = GameObject.Find("TimeTuto");
            verticalTuto.timeTuto.SetActive(false);
            verticalTuto.trophiesTuto = GameObject.Find("TrophiesTuto");
            verticalTuto.trophiesTuto.SetActive(false);
            verticalTuto.challengesTuto = GameObject.Find("ChallengesTuto");
            verticalTuto.challengesTuto.SetActive(false);
            verticalTuto.notificationsTuto = GameObject.Find("NotificationsTuto");
            verticalTuto.notificationsTuto.SetActive(false);

            verticalTuto.resourcesTutoText = GameObject.Find("resourcesTutoText");
            verticalTuto.resourcesTutoText.SetActive(false);
            verticalTuto.islandsTutoText = GameObject.Find("islandsTutoText");
            verticalTuto.islandsTutoText.SetActive(false);
            verticalTuto.timeTutoText = GameObject.Find("timeTutoText");
            verticalTuto.timeTutoText.SetActive(false);
            verticalTuto.trophiesTutoText = GameObject.Find("trophiesTutoText");
            verticalTuto.trophiesTutoText.SetActive(false);
            verticalTuto.challengesTutoText = GameObject.Find("challengesTutoText");
            verticalTuto.challengesTutoText.SetActive(false);
            verticalTuto.notificationsTutoText = GameObject.Find("notificationsTutoText");
            verticalTuto.notificationsTutoText.SetActive(false);
            verticalTuto.tutoTermine = GameObject.Find("tutoTermine");
            verticalTuto.tutoTermine.SetActive(false);
        }
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
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
