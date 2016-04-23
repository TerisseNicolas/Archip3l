using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;



public class ChallengeVertical : InputSource
{
    static public bool challengeWindowPresent = false;
    static public string challengeWindowName = string.Empty;  
    


    public void OnMouseDownSimulation()
    {
        if (!ChallengeVertical.challengeWindowPresent && !ChallengeWon.challengeWonWindowPresent && !Trophy.infoWindowPresent && !Enigma.enigmaWindowOpen && !Disturbance.disturbanceWindowOpen && !Island.infoIslandPresent)
        {
            ChallengeVertical.challengeWindowPresent = true;

            TypeChallenge tc;
            System.Random ran = new System.Random();
            int aleat = ran.Next(0, 2);
            if (aleat == 0)
                tc = TypeChallenge.VraiFaux;
            else
                tc = TypeChallenge.QCM;

            Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenge_" + tc.ToString());
            Canvas canvasChallenge = Instantiate(challengePrefab);
            canvasChallenge.name = "Challenge_" + tc.ToString();
            canvasChallenge.GetComponentInChildren<ChallengeVerticalClick>().init();
            ChallengeVertical.challengeWindowName = canvasChallenge.name;
            main.removeChallenge(this.gameObject);
        }
            
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
            TouchTime = Time.time;
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
