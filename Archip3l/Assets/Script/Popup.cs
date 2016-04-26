using UnityEngine;
using System.Collections;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;


public class Popup : InputSource
{

    public string namePopupCanvas;
    public MinorIsland island;
    public bool touched = false;

    void OnMouseDownSimulation()
    {
        string name = this.gameObject.transform.parent.name;
        this.namePopupCanvas = name;
        string[] nameSplitted = name.Split('_');
        this.island = GameObject.Find(nameSplitted[1] + "_" + nameSplitted[2] + "_" + nameSplitted[3]).GetComponent<MinorIsland>();
        this.touched = true;
        Destroy(GameObject.Find(namePopupCanvas));
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
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
