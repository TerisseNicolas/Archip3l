using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using UnityEngine.SceneManagement;
using System;

public class Loading : InputSource {

    void Start()
    {
        //SoundPlayer.Instance.playLoadingSound();
    }

    void OnMouseDownSimulation()
    {
        SceneSupervisor.Instance.loadMenuScenes(false);;
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
