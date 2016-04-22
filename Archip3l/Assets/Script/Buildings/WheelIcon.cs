using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

using TouchScript.Gestures;
using TouchScript.InputSources;
using TouchScript.Hit;
using TouchScript;


public class WheelIcon : InputSource
{

    MinorIsland island;


    // Use this for initialization
    void Start()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseDownSimulation()
    {
        island.buildingClickedWheel = this.name.Split('_')[1];

        if (island.buildingInfoPresent == false)        //if any buildingInfo is open (not more than one at the same time)
        {
            Canvas buildingInfoPrefab = Resources.Load<Canvas>("Prefab/BuildingInfoWindowCanvas");
            Canvas buildingInfo = Instantiate(buildingInfoPrefab);

            buildingInfo.name = "BuildingInfo_" + island.nameMinorIsland + "_" + this.name;
            buildingInfo.transform.SetParent(this.transform.parent.parent.parent);  //parent : minorIsland
            Vector3 pos = GameObject.Find("sprite-" + island.nameMinorIsland).transform.position;
            pos.z = -2;
            buildingInfo.transform.position = pos;

            //modification of the content of the different Text Children of the Canvas

            List<Tuple<TypeResource, int>> constructionResourceNeeded = Building.getConstructionResourcesNeeded(island.buildingClickedWheel);


            foreach (Text textInCanvas in buildingInfo.GetComponent<Canvas>().GetComponentsInChildren<Text>())
            {
                switch (textInCanvas.name)
                {
                    case "Name":
                        textInCanvas.text = Building.translateBuildingName(island.buildingClickedWheel);
                        break;
                    case "CostValue1":
                        textInCanvas.text = constructionResourceNeeded[0].Second.ToString();
                        if(constructionResourceNeeded[0].Second>island.resourceManager.getResource(constructionResourceNeeded[0].First).Stock)
                            textInCanvas.color = Color.red;
                        else
                            textInCanvas.color = Color.green;
                        break;
                    case "CostValue2":
                        if (constructionResourceNeeded.Count == 2){
                            textInCanvas.text = constructionResourceNeeded[1].Second.ToString();
                            if(constructionResourceNeeded[1].Second>island.resourceManager.getResource(constructionResourceNeeded[1].First).Stock)
                                textInCanvas.color = Color.red;
                            else
                                textInCanvas.color = Color.green;
                        }
                        else
                            textInCanvas.text = "-";
                        break;
                    case "ProductionValueGoodAnswer":
                        textInCanvas.text = (Building.getQuantityResourceOrStatProduced(island.buildingClickedWheel) * 2).ToString();
                        break;
                    case "ProductionValueBadAnswer":
                        textInCanvas.text = (Building.getQuantityResourceOrStatProduced(island.buildingClickedWheel)).ToString();
                        break;
                }
            }

            //modification of the background of the different SpriteRenderer Children of the Canvas
            foreach (SpriteRenderer imageInCanvas in buildingInfo.GetComponent<Canvas>().GetComponentsInChildren<SpriteRenderer>())
            {
                switch (imageInCanvas.name)
                {
                    case "CostImage1":
                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + constructionResourceNeeded[0].First.ToString() + "Icon");
                        break;
                    case "CostImage2":
                        if (constructionResourceNeeded.Count == 2)
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + constructionResourceNeeded[1].First.ToString() + "Icon");
                        else
                            imageInCanvas.sprite = null;
                        break;
                    //mêmes images
                    case "ProductionImage":
                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(island.buildingClickedWheel) + "Icon");
                        break;
                    case "ProductionImage2":
                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(island.buildingClickedWheel) + "Icon");
                        break;
                    case "Build":
                        if (island.resourceManager.getResource(constructionResourceNeeded[0].First).Stock < constructionResourceNeeded[0].Second)
                        {
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/BuildGrise");
                            imageInCanvas.GetComponent<BoxCollider>().enabled = false;
                        }
                        else if (constructionResourceNeeded.Count == 2)
                        {
                            if (island.resourceManager.getResource(constructionResourceNeeded[1].First).Stock < constructionResourceNeeded[1].Second)
                            {
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/BuildGrise");
                                imageInCanvas.GetComponent<BoxCollider>().enabled = false;
                            }
                        }
                        break;
                }
            }
            //rotation of image according to the place of the island
            char id = island.nameMinorIsland[island.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                buildingInfo.transform.Rotate(Vector3.forward * 180);

            island.buildingInfoPresent = true;
            island.wheelPresent = false;
            Destroy(this.transform.parent.parent.gameObject);
        }

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
        var touch = metaGestureEventArgs.Touch;
        if (TouchTime == 0)
        {
            TouchTime = Time.time;
            island.positionTouched = touch.Position;
        }
    }
    
    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }

}