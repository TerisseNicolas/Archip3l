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

    //private bool begun = false;
    	
	void reset ()
    {
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
        reset();
    }

    protected override void OnDisable()
    {
        longGesture.LongPressed -= longPressedHandler;
    }
}
