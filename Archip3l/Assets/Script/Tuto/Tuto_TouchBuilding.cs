using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System.Collections.Generic;



public class Tuto_TouchBuilding : InputSource
{

    public Tuto_MinorIsland island;
    public Tuto_Building building;

    void OnMouseDownSimulation()
    {
        Vector3 pos;
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<Tuto_MinorIsland>();
        island.building = GameObject.Find(island.nameTuto_MinorIsland + "_Harbor").GetComponent<Tuto_Building>();
        building = island.building;

        switch (this.name)
        {
            case "Upgrade":
                if (island.harborMoved)
                {
                    Canvas upgradeTuto_BuildingWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/UpgradeBuildingWindowCanvasTuto");
                    Canvas upgradeTuto_BuildingWindowCanvas = Instantiate(upgradeTuto_BuildingWindowCanvasPrefab);
                    upgradeTuto_BuildingWindowCanvas.name = "UpgradeBuildingWindowCanvas_" + building.name + "_" + island.nameTuto_MinorIsland;
                    upgradeTuto_BuildingWindowCanvas.transform.SetParent(this.transform.parent.parent.parent);  //parent : sous_ile
                    pos = GameObject.Find("Virtual_" + island.nameTuto_MinorIsland).transform.position;
                    pos.z = -2;
                    upgradeTuto_BuildingWindowCanvas.GetComponentInChildren<Image>().transform.position = pos;
                    //rotation of image according to the place of the island
                    char id = island.nameTuto_MinorIsland[island.nameTuto_MinorIsland.Length - 1];
                    if (id == '1' || id == '2')
                        upgradeTuto_BuildingWindowCanvas.GetComponentInChildren<Image>().transform.Rotate(Vector3.forward * 180);
                    //modification of the content of the different Text Children of the Canvas
                    foreach (Text textInCanvas in upgradeTuto_BuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Text>())
                    {
                        switch (textInCanvas.name)
                        {
                            case "Name":
                                textInCanvas.text = "Amélioration 1";
                                break;
                            case "CostValue1":
                                textInCanvas.text = "0";
                                break;
                            case "CostValue2":
                                textInCanvas.text = "0";
                                break;
                        }
                    }
                    //modification of the background of the different Image Children of the Canvas
                    foreach (Image imageInCanvas in upgradeTuto_BuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Image>())
                    {
                        switch (imageInCanvas.name)
                        {
                            case "CostImage1":
                                imageInCanvas.sprite = null;
                                break;
                            case "CostImage2":
                                imageInCanvas.sprite = null;
                                break;
                            //mêmes images
                            case "ProductionImage":
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/FoodIcon");
                                break;
                            case "ProductionImage2":
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/FoodIcon");
                                break;
                        }
                    }
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                }
                else
                {
                    island.displayPopup("Commencez par déplacer le port.", 5);
                }
                break;
            case "Remove":
                if (island.exchangeResourceOpened)
                {
                    StartCoroutine(building.tuto_minorIsland.tuto_buildingManager.destroyBuilding());
                    Destroy(GameObject.Find("touchBuilding_" + this.island.nameTuto_MinorIsland + "_Harbor"));
                }
                else
                {
                    if (island.harborMoved)
                        island.displayPopup("Maintenant, améliorez le port.", 5);
                    else
                        island.displayPopup("Commencez par déplacer le port.", 5);
                }

                break;
            case "Move":
                if (!island.harborMoved)
                {
                    island.displayPopup("Appuyez sur l'endroit où placer le bâtiment", 3);
                    island.moveBuilding = true;
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                }
                else
                {
                    if (island.harborUpgraded)
                        island.displayPopup("Affichez ensuite la fenêtre d'échange de ressources (appui long sur la table).", 5);
                    else
                        island.displayPopup("Maintenant, améliorez le port.", 5);
                }
                break;
        }
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
