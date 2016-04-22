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
            TouchTime = Time.time;
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