using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;


public class Tuto_BuildingInfo : InputSource {

    public string buildingClicked;
    public Tuto_MinorIsland island;

    void OnMouseDownSimulation()
    {
        island = this.transform.parent.parent.parent.GetComponent<Tuto_MinorIsland>();
        buildingClicked = island.buildingClicked;
        if (this.name == "Build")
        {
            Destroy(GameObject.Find("WheelCanvas_" + this.transform.parent.parent.parent.name));
            island.wheelPresent = false;
            island.createTuto_ChallengeBuild(buildingClicked);
            island.harborBuilt = true;
        }
        island.buildingInfoPresent = false;
        Destroy(GameObject.Find(this.transform.parent.parent.name));
    }


    // Use this for initialization
    void Start() {
        if (this.name == "Build")
        {
            Vector3 pos = this.transform.localPosition;
            pos.z = -4;
            this.transform.localPosition = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {

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
