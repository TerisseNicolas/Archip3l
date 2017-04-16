using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System;

public class SecretBackdoor : InputSource
{
    private Game Game;

	void Start ()
    {
        this.Game = GameObject.Find("Game").GetComponent<Game>();
	}

    private void OnMouseDownSimulation()
    {
        this.Game.Timer_FinalTick(this, null);
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
