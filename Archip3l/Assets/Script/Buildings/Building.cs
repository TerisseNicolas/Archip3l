﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;



public class Building : InputSource
{

    public TypeBuilding TypeBuilding { get; private set; }
    public Resource resourceProduced { get; private set; }
    public int quantityProduced { get; set; }
    public int buildState { get; set; }
    public int constructionTime { get; private set; }
    public MinorIsland minorIsland { get; private set; }
    public List<Tuple<TypeResource, int>> constructionResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade1ResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade2ResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade3ResourceNeeded { get; private set; }
    private string texturePath;
    public int level;       //possible levels : 0-1-2-3

    private Client Client;

    public Transform buildingConstructionPrefab;
    public Transform buildingUpgradePrefab;
    

    public void init(TypeBuilding TypeBuilding, MinorIsland island)
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.level = 0;
        this.TypeBuilding = TypeBuilding;
        this.buildState = 0;
        this.minorIsland = island;
        this.constructionResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.upgrade1ResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.upgrade2ResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.upgrade3ResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.name = this.minorIsland.nameMinorIsland + "_" + this.TypeBuilding.ToString();

        this.resourceProduced = ScriptableObject.CreateInstance<Resource>();
        this.texturePath = "Building/Icons/wheelIcon_" + TypeBuilding.ToString();

        this.constructionResourceNeeded = getConstructionResourcesNeeded(TypeBuilding.ToString());
        switch (TypeBuilding)
        {
            case TypeBuilding.GoldMine:
                this.resourceProduced.init(TypeResource.Gold, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.StoneMine:
                this.resourceProduced.init(TypeResource.Stone, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.OilPlant:
                this.resourceProduced.init(TypeResource.Oil, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Sawmill:
                this.resourceProduced.init(TypeResource.Wood, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Factory:
                this.resourceProduced.init(TypeResource.Manufacture, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.WindTurbine:
                this.resourceProduced.init(TypeResource.Electricity, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Farm:
                this.resourceProduced.init(TypeResource.Food, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Harbor:
                this.resourceProduced.init(TypeResource.Food, 0, 1);
                this.quantityProduced = 1;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.PowerPlant:
                this.resourceProduced.init(TypeResource.Electricity, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Lab:
                this.resourceProduced.init(TypeResource.Health, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Airport:
                this.resourceProduced.init(TypeResource.Tourism, 0, 10);
                this.quantityProduced = 10;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 100));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 100));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 150));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 150));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 200));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 200));
                this.constructionTime = 20;
                break;
            case TypeBuilding.Hotel:
                this.resourceProduced.init(TypeResource.Tourism, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 80));
                this.constructionTime = 30;
                break;
            case TypeBuilding.School:
                this.resourceProduced.init(TypeResource.Education, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 80));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Church:
                this.resourceProduced.init(TypeResource.Religion, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                this.constructionTime = 10;
                break;
            case TypeBuilding.Cinema:
                this.resourceProduced.init(TypeResource.Happiness, 0, 5);
                this.quantityProduced = 5;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 60));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 80));
                this.constructionTime = 15;
                break;
            case TypeBuilding.AmusementPark:
                this.resourceProduced.init(TypeResource.Happiness, 0, 10);
                this.quantityProduced = 10;
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 100));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 100));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 150));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 150));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 200));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 200));
                this.constructionTime = 20;
                break;
        }

        GetComponent<Transform>().localScale = new Vector3(9f, 9f, 1f);

        StartCoroutine("build");
    }

    public IEnumerator launchUpgradeAnimation(GameObject go)
    {
        this.buildState = 0;
        //Animation
        var buildingUpgradeTransform = Instantiate(buildingUpgradePrefab) as Transform;
        buildingUpgradeTransform.name = "BuildingUpgradeAnimation_" + minorIsland.nameMinorIsland;
        Anim_BuildingUpgrade anim_BuildingConstruction = buildingUpgradeTransform.GetComponent<Anim_BuildingUpgrade>();
        if (anim_BuildingConstruction != null)
        {
            anim_BuildingConstruction.transform.SetParent(this.transform);
            Vector3 vector3 = anim_BuildingConstruction.transform.parent.position;
            vector3.z = -6;
            anim_BuildingConstruction.transform.position = vector3;
        }
        yield return new WaitForSeconds(this.constructionTime);
        this.buildState = 1;
        Destroy(go);
        Destroy(buildingUpgradeTransform.gameObject);
    }

    IEnumerator build()
    {
        bool flag = true;
        //check needed resources (except for Harbor, already created on islands)
        if (this.TypeBuilding != TypeBuilding.Harbor)
        {
            foreach (Tuple<TypeResource, int> item in this.constructionResourceNeeded)
            {
                //avoid null references
                Resource resource = this.minorIsland.resourceManager.getResource(item.First);
                if (item.Second > resource.Stock)
                {
                    flag = false;
                }
            }
        }

        if (flag == true)
        {
            //again, Harbor already created on islands
            if (this.TypeBuilding != TypeBuilding.Harbor)
            {
                //withdrawal of resources needed for the construction
                foreach (Tuple<TypeResource, int> item in this.constructionResourceNeeded)
                {
                    this.minorIsland.resourceManager.changeResourceStock(item.First, -item.Second);
                }
            }

            //texture
            //Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(this.texturePath, typeof(Texture2D));
            //GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(texturePath);

            //Animation
            var buildingConstructionTransform = Instantiate(buildingConstructionPrefab) as Transform;
            buildingConstructionTransform.name = "BuildingAnnimation_" + minorIsland.nameMinorIsland;
            Anim_BuildingConstruction anim_BuildingConstruction = buildingConstructionTransform.GetComponent<Anim_BuildingConstruction>();
            if (anim_BuildingConstruction != null)
            {
                anim_BuildingConstruction.transform.SetParent(this.transform);
            }

            yield return new WaitForSeconds(this.constructionTime);
            this.buildState = 1;
            quantityProduced -= (int)this.resourceProduced.Production;
            this.changeProduction((int)this.resourceProduced.Production);


            //Todo : score to add must be checked
            yield return new WaitForSeconds(Int32.Parse(minorIsland.nameMinorIsland.Split('_')[2])-1);
            this.Client.sendData("@30505@" + 100.ToString());
            yield return new WaitForSeconds(0.1f);
            this.Client.sendData("@2" + minorIsland.nameMinorIsland.Split('_')[2] + "345@" + resourceProduced.TypeResource.ToString() + "@" + resourceProduced.Production);
            yield return new WaitForSeconds(0.1f);
            this.Client.sendData("@2" + minorIsland.nameMinorIsland.Split('_')[2] + "111@" + this.TypeBuilding.ToString());

            //StartCoroutine("updateStocks");
            Destroy(buildingConstructionTransform.gameObject);
            SoundPlayer.Instance.playBuildingSound(this.TypeBuilding.ToString());
        }
        else
        {
            Debug.Log("Not enough resources");
        }

    }
    public bool changeProduction(int value)
    {
        bool res = this.minorIsland.resourceManager.changeResourceProduction(this.resourceProduced.TypeResource, value);
        if (res)
            quantityProduced += value;
        return res;

    }
    public bool changeStock(int value)
    {
        return this.minorIsland.resourceManager.changeResourceStock(this.resourceProduced.TypeResource, value);
    }
    

    void OnMouseDownSimulation()
    {
        if ((this.buildState == 1) && !MinorIsland.exchangePerforming && !minorIsland.wheelPresent && !minorIsland.touchBuildingPresent && !minorIsland.exchangeWindowPresent && !minorIsland.buildingInfoPresent && !minorIsland.challengePresent && !minorIsland.moveBuilding)
        {
            minorIsland.createBuildingTouch(this);
        }

    }

    static public List<Tuple<TypeResource, int>> getConstructionResourcesNeeded(string nameBuilding)
    {
        List<Tuple<TypeResource, int>> constructionResourcesNeeded = new List<Tuple<TypeResource, int>>();

        switch (nameBuilding)
        {
            case "GoldMine":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                break;
            case "StoneMine":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                break;
            case "OilPlant":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                break;
            case "Sawmill":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                break;
            case "Factory":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                break;
            case "WindTurbine":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 10));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 20));
                break;
            case "Farm":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 5));
                break;
            case "Harbor":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                break;
            case "PowerPlant":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                break;
            case "Lab":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 10));
                break;
            case "Airport":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 200));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 200));
                break;
            case "Hotel":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                break;
            case "School":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 20));
                break;
            case "Church":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                break;
            case "Cinema":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 30));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                break;
            case "AmusementPark":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 100));
                break;
        }
        return constructionResourcesNeeded;
    }


    //translation of the building's name to french
    static public string translateBuildingName(string buildingName)
    {
        switch (buildingName)
        {
            case "GoldMine":
                return "Mine d'or";
            case "StoneMine":
                return "Mine de pierre";
            case "OilPlant":
                return "Puit de pétrole";
            case "Sawmill":
                return "Scierie";
            case "Factory":
                return "Usine";
            case "WindTurbine":
                return "Eolienne";
            case "Farm":
                return "Ferme";
            case "Lab":
                return "Laboratoire";
            case "Airport":
                return "Aéroport";
            case "Hotel":
                return "Hôtel";
            case "Harbor":
                return "Port";
            case "School":
                return "Ecole";
            case "Church":
                return "Eglise";
            case "Cinema":
                return "Cinéma";
            case "AmusementPark":
                return "Parc d'attraction";
            case "PowerPlant":
                return "Centrale électrique";
            default:
                return string.Empty;
        }
    }

    //returns the name of the resource (or stat) produced
    static public string getNameResourceOrStatProduced(string buildingName)
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

    //returns the initial quantity of the resource (or stat) produced
    static public int getQuantityResourceOrStatProduced(string buildingName)
    {
        switch (buildingName)
        {
            case "GoldMine":
                return 5;
            case "StoneMine":
                return 5;
            case "OilPlant":
                return 5;
            case "Sawmill":
                return 5;
            case "Factory":
                return 5;
            case "WindTurbine":
                return 5;
            case "Farm":
                return 5;
            case "Lab":
                return 5;
            case "Airport":
                return 20;
            case "Hotel":
                return 5;
            case "Harbor":
                return 1;
            case "School":
                return 5;
            case "Church":
                return 5;
            case "Cinema":
                return 5;
            case "AmusementPark":
                return 10;
            case "PowerPlant":
                return 10;
            default:
                return 0;
        }
    }

    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime;

    private MetaGesture gesture;
    private Dictionary<int, int> map = new Dictionary<int, int>();

    public override void CancelTouch(TouchPoint touch, bool @return)
    {
        base.CancelTouch(touch, @return);

        map.Remove(touch.Id);
        if (@return)
        {
            TouchHit hit;
            if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
            map.Add(touch.Id, beginTouch(processCoords(hit.RaycastHit.textureCoord), touch.Tags).Id);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchMoved += touchMovedhandler;
            gesture.TouchCancelled += touchCancelledhandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (gesture)
        {
            gesture.TouchBegan -= touchBeganHandler;
            gesture.TouchMoved -= touchMovedhandler;
            gesture.TouchCancelled -= touchCancelledhandler;
            gesture.TouchEnded -= touchEndedHandler;
        }
    }

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        map.Add(touch.Id, beginTouch(processCoords(touch.Hit.RaycastHit.textureCoord), touch.Tags).Id);
        if (TouchTime == 0)
        {
            TouchTime = Time.time;
        }
    }

    private void touchMovedhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        TouchHit hit;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
        moveTouch(id, processCoords(hit.RaycastHit.textureCoord));
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        endTouch(id);
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }

    private void touchCancelledhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        cancelTouch(id);
    }

}
