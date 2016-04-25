using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;


public class Tuto_UpgradeBuilding : InputSource {

    public Tuto_MinorIsland island;
    public Tuto_Building building;

    void OnMouseDownSimulation()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<Tuto_MinorIsland>();
        Debug.Log(island.nameBuildingTouchCanvas);
        building = island.building; 

        if (this.name == "Upgrade")
        {
            island.buildingInfoPresent = false;
            island.createTuto_ChallengeUpgrade(building);
        }

        Destroy(GameObject.Find(this.transform.parent.parent.name));
        island.nameBuildingTouchCanvas = string.Empty;
    }


    // Use this for initialization
    void Start() {
        if (this.name == "Upgrade")
        {
            Vector3 pos = this.transform.localPosition;
            pos.z = -4;
            this.transform.localPosition = pos;
        }
    }

    // Update is called once per frame
    void Update() {

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
        }

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

