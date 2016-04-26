using UnityEngine;
using System.Collections;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;


public class UpgradeBuilding : InputSource
{

    public MinorIsland island;
    public Building building;

    private Client Client;

    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
    }


    void OnMouseDownSimulation()
    {
        island = this.transform.parent.parent.parent.gameObject.GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouched).GetComponent<Building>();
        island.nameBuildingTouched = string.Empty;

        if (this.name == "Upgrade")
        {
            if (building.level < 3)
            {
                
                    
                island.buildingInfoPresent = false;
                island.createChallengeUpgrade(building);
                island.challengePresent = true;

                //To be cheked
                this.Client.sendData("@30505@" + (100*(building.level + 1)).ToString());
            }
            else
            {
                island.displayPopup("Ce bâtiment est déjà au niveau maximal !", 3);
                //StartCoroutine(island.destroyPopup(island.createPopup("Ce bâtiment est déjà au niveau maximal !"), 3));
            }
        }

        island.upgradeBuildingInfoPresent = false;
        Destroy(GameObject.Find(this.transform.parent.parent.name));
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
            if (this.name == "Upgrade")
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("infoBatiments/" + this.name + "Clic");
        }
    }
    
    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (this.name == "Upgrade")
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("infoBatiments/" + this.name);
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }
    
}