using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;

public class SecretBackdoorTuto : InputSource
{
    private Client Client;

    void Start()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();

    }
    private void OnMouseDownSimulation()
    {
        this.Client.sendData("@90000");
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

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (TouchTime == 0)
            TouchTime = Time.time;
    }


    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if ((Time.time - TouchTime < 10) && (Time.time - TouchTime > 3))
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }
}