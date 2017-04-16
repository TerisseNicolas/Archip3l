using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System;

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

