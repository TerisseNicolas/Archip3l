using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TouchScript;


public class InfoIsland : InputSource
{

    public string nameIsland;

    private ResourceManager ResourceManager;

    void OnMouseDownSimulation()    //close button
    {
        Island.infoIslandPresent = false;
        Destroy(GameObject.Find(this.transform.parent.parent.name));
    }


    // Use this for initialization
    void Start()
    {
        GameObject canvasParent = this.transform.parent.parent.gameObject;
        string[] nameSplitted = canvasParent.name.Split('_');
        this.nameIsland = nameSplitted[1] + "_" + nameSplitted[2] + "_" + nameSplitted[3];
        GameObject.Find("nameIsland").GetComponent<Text>().text = Island.getSpecialityNameIsland(this.nameIsland);

        this.ResourceManager = GameObject.Find(this.nameIsland).GetComponent<ResourceManager>();

        string resource;
        foreach (TypeResource typeResource in Enum.GetValues(typeof(TypeResource)))
        {
            resource = typeResource.ToString();
            GameObject.Find(resource + "_Total").GetComponent<Text>().text = this.ResourceManager.getResource(typeResource).Stock.ToString(); ;
            GameObject.Find(resource + "_PerCycle").GetComponent<Text>().text = this.ResourceManager.getResource(typeResource).Production.ToString();                
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