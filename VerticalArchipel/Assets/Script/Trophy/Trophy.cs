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
                return this.Score.numberOfDifferentBuildingBuilt >= 8;
            case "Medal2"://11 types de batiments contruits
                return this.Score.numberOfDifferentBuildingBuilt >= 12;
            case "Medal1"://tous types de batiments contruits
                return this.Score.allBuildingBuilt;
            case "Trophy3": //TOTAL resources count > 500
                return GRM.totalResourceCount > 6000;
            case "Trophy2": //TOTAL resources count > 1000
                return GRM.totalResourceCount > 15000;
            case "Trophy1": //TOTAL resources count > 5000
                return GRM.totalResourceCount > 30000;
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
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }

}