using UnityEngine;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;
using System;

public class RemoveBuilding : InputSource
{

    public MinorIsland island;
    public Building building;

    void OnMouseDownSimulation()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouched).GetComponent<Building>();
        island.nameBuildingTouched = string.Empty;

        if (this.name == "Remove")
        {
            building.removed = true;
            //regain of resources
            TypeResource res1 = building.constructionResourceNeeded[0].First;
            int value1 = building.constructionResourceNeeded[0].Second / 2;
            island.resourceManager.changeResourceStock(res1, value1);
            GameObject.Find("Network").GetComponent<Client>().sendData("@2" + island.nameMinorIsland.Split('_')[2] + "355@" + res1.ToString() + "@" + value1.ToString());
            TypeResource res2;
            int value2;
            if (building.constructionResourceNeeded.Count == 2)
            {
                res2 = building.constructionResourceNeeded[1].First;
                value2 = building.constructionResourceNeeded[1].Second / 2;
                island.resourceManager.changeResourceStock(res2, value2);
                GameObject.Find("Network").GetComponent<Client>().sendData("@2" + island.nameMinorIsland.Split('_')[2] + "355@" + res2.ToString() + "@" + value2.ToString());

            }
            StartCoroutine(building.minorIsland.buildingManager.destroyBuilding(building.TypeBuilding));
        }

        island.removeBuildingInfoPresent = false;
        Destroy(GameObject.Find(this.transform.parent.parent.name));
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