using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;
using System;

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
