using UnityEngine;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;

public class ListResources : InputSource
{

    public MinorIsland island;

    void OnMouseDownSimulation()
    {
        if (this.name != "Close")
        {
            island.resource = this.name;
        }
        island.otherWindowOpen = false;
        Destroy(GameObject.Find(this.transform.parent.parent.name));
    }


    // Use this for initialization
    void Start()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
    }

    // Update is called once per frame
    void Update()
    {

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

