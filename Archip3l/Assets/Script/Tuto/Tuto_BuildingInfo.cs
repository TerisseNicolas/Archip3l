using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System;

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

    private TapGesture gesture;


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += pressedHandler;
    }


    private void pressedHandler(object sender, EventArgs e)
    {
        this.OnMouseDownSimulation();
    }

}
