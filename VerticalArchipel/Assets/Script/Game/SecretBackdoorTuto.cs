using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System;

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

    private LongPressGesture longGesture;

    protected override void OnEnable()
    {
        base.OnEnable();
        longGesture = GetComponent<LongPressGesture>();
        longGesture.LongPressed += longPressedHandler;
    }

    private void longPressedHandler(object sender, EventArgs e)
    {
        OnMouseDownSimulation();
    }

    protected override void OnDisable()
    {
        longGesture.LongPressed -= longPressedHandler;
    }
}