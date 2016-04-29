using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using TouchScript.InputSources;


public class ResetWindows : InputSource
{

    private bool begun = false;

    
	
	void Update () {

        if (this.begun)
        {
            if ((Time.time - TouchTime > 5) && (Time.time - TouchTime < 7))
            {
                TouchTime = 0;
                Debug.Log("resetting");

                Trophy.infoWindowPresent = false;
                Trophy.infoWindowName = string.Empty;
                Island.infoIslandPresent = false;
                Island.infoIslandName = string.Empty;
                Enigma.enigmaWindowOpen = false;
                Enigma.enigmaWindowName = string.Empty;
                Disturbance.disturbanceWindowOpen = false;
                Disturbance.islandChosen = string.Empty;
                Disturbance.actionMade = false;
                ChallengeWon.challengeWonWindowPresent = false;
                ChallengeWon.challengeWonWindowName = string.Empty;
                ChallengeVertical.challengeWindowPresent = false;
                ChallengeVertical.challengeWindowName = string.Empty; 


                //close windows
                foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
                {                    
                    if (obj.name.Contains("info") ||
                        obj.name == "Disturbance" ||
                        obj.name == "Enigma" ||
                        obj.name.Contains("Challenge_") ||
                        obj.name.Contains("challengeWon"))
                            Destroy(obj);
                }
            }
        }


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
        {
            TouchTime = Time.time;
            this.begun = true;
        }
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        this.begun = false;
        TouchTime = 0;
    }

}
