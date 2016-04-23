using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;



public class BuildingInfo : InputSource
{

    public string buildingClicked { get; private set; }
    public MinorIsland island { get; private set; }

    void OnMouseDownSimulation()
    {
        island = this.transform.parent.parent.parent.GetComponent<MinorIsland>();
        buildingClicked = island.buildingClickedWheel;
        if (this.name == "Build")
        {
            island.wheelPresent = false;
            island.createChallengeBuild(buildingClicked);
            island.challengePresent = true;
            Destroy(GameObject.Find("WheelCanvas_" + this.transform.parent.parent.parent.name));
        }

        island.buildingInfoPresent = false;
        Destroy(this.transform.parent.parent.gameObject);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime;

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
        {
            TouchTime = Time.time;
            if (this.name == "Build")
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("infoBatiments/" + this.name + "Clic");
        }
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (this.name == "Build")
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("infoBatiments/" + this.name);
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }
    
}
