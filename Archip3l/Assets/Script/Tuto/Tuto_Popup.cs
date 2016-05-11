using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;
using System;

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

