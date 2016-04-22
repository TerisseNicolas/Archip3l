using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;



public class Tuto_Popup : InputSource
{

    public string namePopupCanvas;
    public Tuto_MinorIsland island;
    public bool touched = false;


    void OnMouseDownSimulation()
    {
        string name = this.gameObject.transform.parent.name;
        this.namePopupCanvas = name;
        string[] nameSplitted = name.Split('_');
        this.island = GameObject.Find(nameSplitted[1] + "_" + nameSplitted[2] + "_" + nameSplitted[3]).GetComponent<Tuto_MinorIsland>();
        this.touched = true;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        Destroy(GameObject.Find(namePopupCanvas));

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

