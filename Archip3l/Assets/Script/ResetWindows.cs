using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using TouchScript.InputSources;


public class ResetWindows : InputSource
{

    private bool begun = false;

    private MinorIsland island;


    // Use this for initialization
    void Start () {
        island = GameObject.Find("sous_ile_" + gameObject.transform.parent.name.Split('_')[3].ToString()).GetComponent<MinorIsland>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.begun)
        {
            if ((Time.time - TouchTime > 5) && (Time.time - TouchTime < 7))
            {
                TouchTime = 0;
                island.TouchTime = 0;
                Debug.Log("resetting");

                //set all booleans of windows to false
                island.wheelPresent = false;
                island.buildingInfoPresent = false;
                island.upgradeBuildingInfoPresent = false;
                island.challengePresent = false;
                island.moveBuilding = false;
                island.exchangeWindowPresent = false;
                island.disturbancePresent = false;
                island.touchBuildingPresent = false;
                island.removeBuildingInfoPresent = false;
                island.otherWindowOpen = false;


                //close windows
                foreach(GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
                {
                    if (obj.name.Contains("WheelCanvas_" + island.nameMinorIsland) ||
                        obj.name.Contains("BuildingInfo_" + island.nameMinorIsland) ||
                        obj.name.Contains("UpgradeBuildingWindowCanvas_" + island.nameMinorIsland) ||
                        obj.name.Contains("Challenge_QCM_" + island.nameMinorIsland) ||
                        obj.name.Contains("Challenge_VraiFaux_" + island.nameMinorIsland) ||
                        obj.name.Contains("ExchangeWindowCanvas_" + island.nameMinorIsland) ||
                        obj.name.Contains("touchBuilding_" + island.nameMinorIsland) ||
                        obj.name.Contains("RemoveBuildingWindowCanvas_" + island.nameMinorIsland) ||
                        obj.name.Contains("listResourcesCanvas_" + island.nameMinorIsland) ||
                        obj.name.Contains("listIslandsCanvas_" + island.nameMinorIsland))
                            Destroy(obj);
                }
            }
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
        {
            TouchTime = Time.time;
            this.begun = true;
        }
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        this.begun = false;
        TouchTime = 0;
    }

}

