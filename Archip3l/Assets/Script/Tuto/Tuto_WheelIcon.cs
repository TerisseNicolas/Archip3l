using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using TouchScript.InputSources;

public class Tuto_WheelIcon : InputSource
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseDownSimulation()
    {
        Tuto_MinorIsland island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<Tuto_MinorIsland>();

        island.buildingClicked = this.name.Split('_')[1];

        if (island.buildingInfoPresent == false)        //if any buildingInfo is open (not more than one at the same time)
        {
            Canvas buildingInfoPrefab = Resources.Load<Canvas>("Prefab/Tuto/BuildingInfoWindowCanvasTuto");
            Canvas buildingInfo = Instantiate(buildingInfoPrefab);

            buildingInfo.name = "BuildingInfo_" + island.nameTuto_MinorIsland + "_" + this.name;
            buildingInfo.transform.SetParent(this.transform.parent.parent.parent);  //parent : minorIsland
            Vector3 pos = GameObject.Find("Virtual_" + island.nameTuto_MinorIsland).transform.position;
            pos.z = -2;
            buildingInfo.GetComponentInChildren<Image>().transform.position = pos;

            //rotation of image according to the place of the island
            char id = island.nameTuto_MinorIsland[island.nameTuto_MinorIsland.Length - 1];
            if (id == '1' || id == '2')
                buildingInfo.GetComponentInChildren<Image>().transform.Rotate(Vector3.forward * 180);

            island.buildingInfoPresent = true;
        }
        Destroy(GameObject.Find("WheelCanvas_" + island.nameTuto_MinorIsland));

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

