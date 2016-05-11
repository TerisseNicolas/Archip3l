﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;



public class Tuto_Building : InputSource
{

    public int buildState { get; set; }
    public int constructionTime { get; private set; }
    public Tuto_MinorIsland tuto_minorIsland { get; private set; }
    private string texturePath;
    public int level;       //possible levels : 0-1-2-3

    public Transform buildingConstructionPrefab;
    public Transform buildingUpgradePrefab;

    public void init(Tuto_MinorIsland island)
    {
        this.level = 0;
        this.buildState = 0;
        this.tuto_minorIsland = island;
        this.name = this.tuto_minorIsland.nameTuto_MinorIsland + "_Harbor";

        this.texturePath = "Building/Icons/wheelIcon_Harbor";

        this.constructionTime = 5;

        GetComponent<Transform>().localScale = new Vector3(9f, 9f, 1f);

        StartCoroutine("build");
    }

    public IEnumerator launchUpgradeAnimation()
    {
        //Animation
        /*var buildingUpgradeTransform = Instantiate(buildingUpgradePrefab) as Transform;
        buildingUpgradeTransform.name = "BuildingUpgradeAnimation_" + tuto_minorIsland.nameTuto_MinorIsland;
        Anim_BuildingConstruction anim_BuildingConstruction = buildingUpgradeTransform.GetComponent<Anim_BuildingConstruction>();
        if (anim_BuildingConstruction != null)
        {
            anim_BuildingConstruction.transform.SetParent(this.transform);
            Vector3 vector3 = anim_BuildingConstruction.transform.parent.position;
            vector3.z = -2;
            anim_BuildingConstruction.transform.position = vector3;
        }*/
        yield return new WaitForSeconds(0);
        this.buildState = 1;
        tuto_minorIsland.harborUpgraded = true;
        tuto_minorIsland.displayPopup("Affichez ensuite la fenêtre d'échange de ressources (appui long sur la table).", 5);
        this.GetComponent<BoxCollider>().enabled = false;
        //Destroy(buildingUpgradeTransform.gameObject);
    }

    IEnumerator build()
    {
        //texture
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(texturePath);

        //Animation
        /*var buildingConstructionTransform = Instantiate(buildingConstructionPrefab) as Transform;
        buildingConstructionTransform.name = "BuildingAnnimation_" + tuto_minorIsland.nameTuto_MinorIsland;
        Anim_BuildingConstruction anim_BuildingConstruction = buildingConstructionTransform.GetComponent<Anim_BuildingConstruction>();
        if (anim_BuildingConstruction != null)
        {
            anim_BuildingConstruction.transform.SetParent(this.transform);
        }*/

        yield return new WaitForSeconds(0);
        this.buildState = 1;
        //Destroy(buildingConstructionTransform.gameObject);

    }


    void OnMouseDownSimulation()
    {
        if ((this.buildState == 1) && !tuto_minorIsland.wheelPresent && !tuto_minorIsland.buildingInfoPresent && !tuto_minorIsland.moveBuilding && tuto_minorIsland.nameBuildingTouchCanvas == String.Empty)
        {
            tuto_minorIsland.createBuildingTouch(this);
        }

    }


    //returns the name of the resource (or stat) produced
    static public string getNameTuto_ResourceOrStatProduced(string buildingName)
    {
        switch (buildingName)
        {
            case "GoldMine":
                return "Gold";
            case "StoneMine":
                return "Stone";
            case "OilPlant":
                return "Oil";
            case "Sawmill":
                return "Wood";
            case "Factory":
                return "Manufacture";
            case "WindTurbine":
                return "Electricity";
            case "Farm":
                return "Food";
            case "Lab":
                return "Health";
            case "Airport":
                return "Tourism";
            case "Hotel":
                return "Tourism";
            case "Harbor":
                return "Food";
            case "School":
                return "Education";
            case "Church":
                return "Religion";
            case "Cinema":
                return "Happiness";
            case "AmusementPark":
                return "Happiness";
            case "PowerPlant":
                return "Electricity";
            default:
                return string.Empty;
        }
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

