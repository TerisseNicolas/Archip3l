using UnityEngine;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;



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
            TouchTime = Time.time;
    }
    

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }
    
}