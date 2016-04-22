using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;



//this class concerns the thophies + the medals + AirportMedal
public class Trophy : InputSource
{
    static public bool infoWindowPresent = false;
    static public string infoWindowName = string.Empty;

    public bool active = false;     //true if trophy unlocked
    public bool toBeActivated = false;
    public string trophyName;
    public string description;

    private Score Score;
    //Requirement resource
    //public Resource resourceRequired { get; private set; }
    //public int resourceRequiredQuantity { get; private set; }
    //Requirement challenge

    public Sprite wonSprite;

    void Awake()
    {
        this.Score = GameObject.Find("Game").GetComponent<Score>();
        this.trophyName = name;
    }

    public bool requirementVerified(GlobalResourceManager GRM)
    {
        switch (this.trophyName)
        {
            //TODO : modify
            case "Medal3": //6 types de batiments contruits
                return this.Score.numberOfDifferentBuildingBuilt >= 6;
            case "Medal2"://11 types de batiments contruits
                return this.Score.numberOfDifferentBuildingBuilt >= 11;
            case "Medal1"://tous types de batiments contruits
                return this.Score.allBuildingBuilt;
            case "Trophy3": //TOTAL resources count > 500
                return GRM.totalResourceCount > 4000;
            case "Trophy2": //TOTAL resources count > 1000
                return GRM.totalResourceCount > 10000;
            case "Trophy1": //TOTAL resources count > 5000
                return GRM.totalResourceCount > 20000;
                // Airport requirement managed in the trophy manager

                //return resourceManager.getResource(TypeResource.Gold).Stock > 10000;
                //return resourceManager.getResource(TypeResource.Food).Stock > 5000;
        }
        return false;
    }

    void OnMouseDownSimulation()
    {
        Vector3 pos;
        if (!ChallengeWon.challengeWonWindowPresent && !Enigma.enigmaWindowOpen && !Disturbance.disturbanceWindowOpen && !Island.infoIslandPresent && !ChallengeVertical.challengeWindowPresent)
        {
            if (!Trophy.infoWindowPresent)
            {
                Trophy.infoWindowPresent = true;
                switch (this.name)
                {
                    case "AirportMedal":
                        Canvas infoAirportCanvasPrefab = Resources.Load<Canvas>("Prefab/infoAirportCanvas");
                        Canvas infoAirportCanvas = Instantiate(infoAirportCanvasPrefab);
                        infoAirportCanvas.name = "infoAirportCanvas";
                        pos = infoAirportCanvas.transform.position;
                        pos.z = -2;
                        infoAirportCanvas.transform.position = pos;
                        Trophy.infoWindowName = infoAirportCanvas.name;
                        break;
                    case "MedalsCollider":
                        Canvas infoMedalsCanvasPrefab = Resources.Load<Canvas>("Prefab/infoMedalsCanvas");
                        Canvas infoMedalsCanvas = Instantiate(infoMedalsCanvasPrefab);
                        infoMedalsCanvas.name = "infoMedalsCanvas";
                        pos = infoMedalsCanvas.transform.position;
                        pos.z = -2;
                        infoMedalsCanvas.transform.position = pos;
                        Trophy.infoWindowName = infoMedalsCanvas.name;
                        break;
                    case "TrophiesCollider":
                        Canvas infoTrophiesCanvasPrefab = Resources.Load<Canvas>("Prefab/infoTrophiesCanvas");
                        Canvas infoTrophiesCanvas = Instantiate(infoTrophiesCanvasPrefab);
                        infoTrophiesCanvas.name = "infoTrophiesCanvas";
                        pos = infoTrophiesCanvas.transform.position;
                        pos.z = -2;
                        infoTrophiesCanvas.transform.position = pos;
                        Trophy.infoWindowName = infoTrophiesCanvas.name;
                        break;
                }
            }
            else if (this.name == "close")
            {
                Trophy.infoWindowPresent = false;
                Destroy(GameObject.Find(this.transform.parent.parent.name));
            }
        }
    }

    void Update()
    {
        if(this.toBeActivated)
        {
            this.toBeActivated = false;
            if (!this.active)
            {
                this.active = true;
                switch (this.trophyName)
                {
                    case "Medal1":
                    case "Medal2":
                    case "Medal3":
                        transform.localScale = new Vector3(5.6f, 5f, 1);
                        break;
                    case "Trophy1":
                    case "Trophy2":
                    case "Trophy3":
                        transform.localScale = new Vector3(3.2f, 3.8f, 1);
                        break;
                    case "AirportMedal":
                        transform.localScale = new Vector3(7f, 8f, 1);
                        break;
                }
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = wonSprite;
            main.addNotification("Vous remportez un trophée !");
        }
    }

    public bool changeToObtained()
    {
        this.toBeActivated = true;
        return true;
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
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
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