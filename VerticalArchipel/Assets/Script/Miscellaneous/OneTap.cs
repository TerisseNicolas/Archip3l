using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System.Collections.Generic;
using System;

public abstract class OneTap : InputSource
{
    //public int Width = 512;
    //public int Height = 512;

    private TapGesture gesture;

    protected abstract void OnMouseDownSimulation();

    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += pressedHandler;
    }

    protected override void OnDisable()
    {
        gesture.Tapped -= pressedHandler;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        this.OnMouseDownSimulation();
    }
}