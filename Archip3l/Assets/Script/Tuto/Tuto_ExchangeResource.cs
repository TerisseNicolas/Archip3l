using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;
using System;

public class Tuto_ExchangeResource : InputSource
{

    public Tuto_MinorIsland island;
               

    void OnMouseDownSimulation()
    {
        Destroy(GameObject.Find(this.transform.parent.parent.name));
        island.exchangeWindowPresent = false;
        island.exchangeResourceOpened = true;
        island.displayPopup("Pour finir le tutoriel, supprimez le port.", 5);
        GameObject.Find(island.nameTuto_MinorIsland + "_Harbor").GetComponent<BoxCollider>().enabled = true;

    }

    // Use this for initialization
    void Start()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<Tuto_MinorIsland>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;

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

