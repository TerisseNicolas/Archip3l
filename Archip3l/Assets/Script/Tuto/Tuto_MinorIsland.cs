using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using UnityEngine.SceneManagement;

public class Tuto_MinorIsland : InputSource {

    public Tuto_BuildingManager tuto_buildingManager { get; private set; }
    public Transform tuto_buildingManagerPrefab;

    Canvas startCanvas;

    private bool begun = false;

    public string nameTuto_MinorIsland;

    //communication with WheelIcon, BuildingInfo & Tuto_ChallengeBuild scripts + Popups & TouchBuilding
    public Vector2 placeOfBuildingConstruction;
    public Vector2 positionTouched;
    public bool wheelPresent = false;                   //wheel present on the island
    public bool buildingInfoPresent = false;            //buildingInfo present on the island
    public bool moveBuilding = false;                   //moving a building
    public bool exchangeWindowPresent = false;          //exchangeWindow present on the island
    public string nameBuildingTouchCanvas;
    public string buildingClicked;
    public int numPopup = 0;

    //steps of tuto
    public bool harborBuilt = false;
    public bool harborMoved = false;
    public bool harborUpgraded = false;
    public bool exchangeResourceOpened = false;
    public bool harborRemoved = false;

    public Canvas endCanvas;
    public bool ended = false;

    private Client Client;
    private bool verticalTutoCompleted;


    void Awake()
    {
        var buildingManagerTransform = Instantiate(tuto_buildingManagerPrefab) as Transform;
        Tuto_BuildingManager mytuto_buildingManager = buildingManagerTransform.GetComponent<Tuto_BuildingManager>();
        if (mytuto_buildingManager != null)
        {
            mytuto_buildingManager.init(this);
            mytuto_buildingManager.transform.SetParent(this.transform);
            mytuto_buildingManager.name = "BuildingManager_" + this.nameTuto_MinorIsland;
            this.tuto_buildingManager = mytuto_buildingManager;
        }

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageTutoCompleteEvent += Client_MessageTutoCompleteEvent;
        this.verticalTutoCompleted = false;


    }

    public void Start()
    {
        if (nameTuto_MinorIsland == "sous_ile_1")
        {

            Canvas startCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/StartCanvas");
            startCanvas = Instantiate(startCanvasPrefab);
            startCanvas.name = "StartCanvas";
            StartCoroutine(this.startFade());
        }
        displayPopup("Bienvenue dans le tutoriel. \nPour commencer, appuyez n'importe où puis créez le port.", 10);

    }

    public IEnumerator startFade()
    {
        SpriteRenderer sp = startCanvas.GetComponentInChildren<SpriteRenderer>();
        Color colorBlack;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 200; i++)
        {
            yield return new WaitForSeconds(0.01f);
            colorBlack = sp.color;
            colorBlack.a -= 0.005f;
            sp.color = colorBlack;
        }
        Destroy(GameObject.Find("StartCanvas"));
    }

    public IEnumerator endFade()
    {
        SpriteRenderer sp = endCanvas.GetComponentInChildren<SpriteRenderer>();
        Color color;
        for (int i = 0; i < 200; i++)
        {
            yield return new WaitForSeconds(0.01f);
            color = sp.color;
            color.a += 0.005f;
            sp.color = color;
        }
        while(true)
        {
            if(this.verticalTutoCompleted)
            {
                SceneSupervisor.Instance.loadPalyingScenes();
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void createTuto_ChallengeBuild(string buildingClicked)
    {
        this.tuto_buildingManager.createBuilding(this.placeOfBuildingConstruction);
    }

    public void createTuto_ChallengeUpgrade(Tuto_Building tuto_building)
    {
        tuto_building.level += 1;
        tuto_building.buildState = 0;
        StartCoroutine(tuto_building.launchUpgradeAnimation());
    }



    public void displayPopup(string popupText, int time)
    {
        if (popupText != string.Empty)
            StartCoroutine(destroyPopup(createPopup(popupText), time));
    }

    //surcharge: for building (explaination displayed at the end of previous popup)
    public void displayPopup(string popupText, int time, string explaination)
    {
        if (popupText != string.Empty)
            StartCoroutine(destroyPopup(createPopup(popupText), time, explaination));
    }

    //returns the name of the Popup (GameObject) created
    public string createPopup(string popupText)
    {
        if (GameObject.Find("PopupCanvas" + "_" + this.nameTuto_MinorIsland) != null)
        {
            GameObject.Find("PopupCanvas" + "_" + this.nameTuto_MinorIsland).GetComponentInChildren<Tuto_Popup>().touched = true;
            Destroy(GameObject.Find("PopupCanvas" + "_" + this.nameTuto_MinorIsland));
        }

        Canvas popupCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/PopupCanvasTuto");
        Canvas popupCanvas = Instantiate(popupCanvasPrefab);
        popupCanvas.name = "PopupCanvas" + "_" + this.nameTuto_MinorIsland;
        this.numPopup++;
        popupCanvas.transform.SetParent(GameObject.Find(this.nameTuto_MinorIsland).transform);
        Vector3 vector3 = GameObject.Find("sprite-" + this.nameTuto_MinorIsland).transform.position;
        vector3.z = -2;
        popupCanvas.transform.position = vector3;

        //rotation of image according to the place of the island
        char id = this.nameTuto_MinorIsland[this.nameTuto_MinorIsland.Length - 1];
        if (id == '1' || id == '2')
            popupCanvas.transform.Rotate(Vector3.forward * 180);

        popupCanvas.GetComponentInChildren<Text>().text = popupText;

        //name + island passed to get the Canvas to destroy
        popupCanvas.GetComponentInChildren<Tuto_Popup>().namePopupCanvas = popupCanvas.name;
        popupCanvas.GetComponentInChildren<Tuto_Popup>().island = this;

        return popupCanvas.name;
    }


    //destroy popup after a certain time
    public IEnumerator destroyPopup(string namePopup, int timer)
    {
        Tuto_Popup popup = GameObject.Find(namePopup).GetComponentInChildren<Tuto_Popup>();
        SpriteRenderer popupImage = GameObject.Find(namePopup).GetComponentInChildren<SpriteRenderer>();

        yield return new WaitForSeconds(timer);
        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            if (!popup.touched)
            {
                color = popupImage.color;
                color.a -= 0.01f;
                popupImage.color = color;
            }
            else
                break;

        }
        if (!popup.touched)
            Destroy(GameObject.Find(namePopup));
    }

    //surcharge: for buildings (display explaination after previous popup)
    //destroy popup after a certain time
    public IEnumerator destroyPopup(string namePopup, int timer, string explaination)
    {
        Tuto_Popup popup = GameObject.Find(namePopup).GetComponentInChildren<Tuto_Popup>();
        SpriteRenderer popupImage = GameObject.Find(namePopup).GetComponentInChildren<SpriteRenderer>();

        yield return new WaitForSeconds(timer);
        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            if (!popup.touched)
            {
                color = popupImage.color;
                color.a -= 0.01f;
                popupImage.color = color;
            }
            else
                break;

        }
        if (!popup.touched)
            Destroy(GameObject.Find(namePopup));

        yield return new WaitForSeconds(0.5f);
        displayPopup(explaination, timer);
    }


    public void removeAllPopups()
    {
        for (int i = 0; i < this.numPopup; i++)
        {

            if (GameObject.Find("PopupCanvas" + i.ToString() + "_" + nameTuto_MinorIsland) != null)
            {
                GameObject.Find("PopupCanvas" + i.ToString() + "_" + nameTuto_MinorIsland).GetComponentInChildren<Tuto_Popup>().touched = true;
                Destroy(GameObject.Find("PopupCanvas" + i.ToString() + "_" + nameTuto_MinorIsland));
            }
        }
    }


    public void createBuildingTouch(Tuto_Building building)
    {
        this.nameBuildingTouchCanvas = building.name;

        Canvas touchBuildingCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/touchBuildingCanvasTuto");
        Canvas touchBuildingCanvas = Instantiate(touchBuildingCanvasPrefab);
        touchBuildingCanvas.transform.SetParent(this.transform);
        touchBuildingCanvas.name = "touchBuilding_" + this.nameBuildingTouchCanvas;
        touchBuildingCanvas.transform.position = GameObject.Find(this.nameBuildingTouchCanvas).transform.position;

        foreach (Tuto_TouchBuilding touchBuilding in touchBuildingCanvas.GetComponentsInChildren<Tuto_TouchBuilding>())
        {
            touchBuilding.island = this;
            touchBuilding.building = building;
        }
        //touchBuildingCanvas.GetComponent<TouchBuilding>().island = this;

        //rotation of image according to the place of the island
        char id = this.nameTuto_MinorIsland[this.nameTuto_MinorIsland.Length - 1];
        if (id == '1' || id == '2')
            touchBuildingCanvas.transform.Rotate(Vector3.forward * 180);

    }


    void createExchangeWindowTuto()
    {
        if (!exchangeWindowPresent && !wheelPresent)
        {
            Canvas exchangeWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/exchangeWindowCanvasTuto");
            Canvas exchangeWindowCanvas = Instantiate(exchangeWindowCanvasPrefab);
            exchangeWindowCanvas.transform.parent = GameObject.Find(this.nameTuto_MinorIsland).transform;
            exchangeWindowCanvas.name = "ExchangeWindowCanvas_" + this.nameTuto_MinorIsland;
            Vector3 vector3 = GameObject.Find("Virtual_" + this.nameTuto_MinorIsland).transform.position;
            vector3.z = -5.1f;
            exchangeWindowCanvas.GetComponentInChildren<SpriteRenderer>().transform.position = vector3;
            //rotation of image according to the place of the island
            char id = this.nameTuto_MinorIsland[this.nameTuto_MinorIsland.Length - 1];
            if (id == '1' || id == '2')
                exchangeWindowCanvas.GetComponentInChildren<SpriteRenderer>().transform.Rotate(Vector3.forward * 180);

            this.exchangeWindowPresent = true;
        }
    }



    // Update is called once per frame
    void Update() {

        if (!ended)
        {
            //when all islands have finished the tuto + Vertical tuto finished, change scene

            //TODO: add condition on Vertical finishing tuto
            if (GameObject.Find("sous_ile_1").GetComponent<Tuto_MinorIsland>().harborRemoved &&
                GameObject.Find("sous_ile_2").GetComponent<Tuto_MinorIsland>().harborRemoved &&
                GameObject.Find("sous_ile_3").GetComponent<Tuto_MinorIsland>().harborRemoved &&
                GameObject.Find("sous_ile_4").GetComponent<Tuto_MinorIsland>().harborRemoved)
            {
                this.ended = true;
                if (nameTuto_MinorIsland == "sous_ile_1")
                {
                    Canvas endCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/EndCanvas");
                    endCanvas = Instantiate(endCanvasPrefab);
                    Color color = endCanvas.GetComponentInChildren<SpriteRenderer>().color;
                    color.a = 0;
                    endCanvas.GetComponentInChildren<SpriteRenderer>().color = color;
                    StartCoroutine(this.endFade());
                }

            }
        }

        /*------- open exchangeWindow after 1 sec --------*/

        if (this.begun)
        {
            if ((Time.time - TouchTime > 1) && (Time.time - TouchTime < 1.5))
            {
                TouchTime = 0;
                if (this.harborUpgraded && !this.exchangeResourceOpened)
                {
                    displayPopup("Voici la fenêtre d'échange de ressources. Vous pouvez y accéder à n'importe quel moment grâce à un appui long.", 5);
                    this.createExchangeWindowTuto();
                }
            }
        }
    }


    void OnMouseDownSimulation()
    {

        //moving a building
        if (moveBuilding)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(positionTouched.x, positionTouched.y, 0));
            pos.z = -5;
            GameObject.Find(this.nameBuildingTouchCanvas).transform.position = pos;
            this.moveBuilding = false;
            this.nameBuildingTouchCanvas = string.Empty;
            this.harborMoved = true;
            displayPopup("Maintenant, améliorez le port.", 5);
        }
        else
        {
            if (this.nameBuildingTouchCanvas != String.Empty)
            {
                Destroy(GameObject.Find("touchBuilding_" + this.nameBuildingTouchCanvas));
                this.nameBuildingTouchCanvas = String.Empty;
            }
            else
            {
                if (!wheelPresent && !harborBuilt)  //if the wheel is not on the island
                {

                    //Wheel appearance
                    this.placeOfBuildingConstruction = this.positionTouched;
                    Canvas prefabWheelCanvas = Resources.Load<Canvas>("Prefab/Tuto/WheelCanvasTuto");
                    Canvas wheelCanvas = Instantiate(prefabWheelCanvas);
                    wheelCanvas.name = "WheelCanvas_" + nameTuto_MinorIsland;
                    //parent : island
                    wheelCanvas.transform.SetParent(GameObject.Find(nameTuto_MinorIsland).transform);
                    SpriteRenderer wheelImage = wheelCanvas.GetComponentInChildren<SpriteRenderer>();
                    //position of wheel where it was clicked on
                    Vector3 pos = Camera.main.ScreenToWorldPoint(this.positionTouched);
                    pos.z = -2;
                    
                    wheelImage.transform.position = pos;
                    //rotation of image according to the place of the island
                    char id = this.nameTuto_MinorIsland[this.nameTuto_MinorIsland.Length - 1];
                    if (id == '1' || id == '2')
                        wheelImage.transform.Rotate(Vector3.forward * 180);
                    
                    //disable specific buildings
                    foreach (SpriteRenderer sr in wheelImage.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (sr.name != "wheelIcon_Harbor" && sr.name != "WheelImage")
                        {
                            sr.sprite = Resources.Load<Sprite>("Building/Icons_Disabled/" + sr.name + "_disabled");
                        }
                    }

                    this.wheelPresent = true;
                }
                else
                {
                    if (!buildingInfoPresent)       //if the wheel is on the island, but not the buildingInfo
                    {
                        //destruction of the wheel if clic somewhere else in the island
                        Destroy(GameObject.Find("WheelCanvas_" + nameTuto_MinorIsland));
                        this.wheelPresent = false;
                    }
                }
            }
        }
    }

    private void Client_MessageTutoCompleteEvent(object sender, MessageEventArgs e)
    {
        this.verticalTutoCompleted = true;
    }

    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime = 0;

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
        {
            TouchTime = Time.time;
            this.positionTouched = touch.Position;
            this.begun = true;
        }

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
        this.begun = false;
        if (Time.time - TouchTime < 1)
        {
            TouchTime = 0;
            this.OnMouseDownSimulation();
        }
        else if (Time.time - TouchTime < 3)
        {
            TouchTime = 0;
            if (this.harborUpgraded && !this.exchangeResourceOpened)
            {
                displayPopup("Voici la fenêtre d'échange de ressources. Vous pouvez y accéder à n'importe quel moment grâce à un appui long.", 5);
                this.createExchangeWindowTuto();
            }
        }
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


