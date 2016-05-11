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



public class ChallengeVertical : OneTap
{
    static public bool challengeWindowPresent = false;
    static public string challengeWindowName = string.Empty;

    protected override void OnMouseDownSimulation()
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
}
