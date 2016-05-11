using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System.Collections.Generic;


public class Island : OneTap
{

    static public bool infoIslandPresent = false;
    static public string infoIslandName = string.Empty;

    protected override void OnMouseDownSimulation()
    {
        if (!ChallengeWon.challengeWonWindowPresent && !Enigma.enigmaWindowOpen && !Disturbance.disturbanceWindowOpen && !Trophy.infoWindowPresent && !Island.infoIslandPresent && !ChallengeVertical.challengeWindowPresent)
        {
            infoIslandPresent = true;
            Canvas infoInslandCanvasPrefab = Resources.Load<Canvas>("Prefab/infoIslandCanvas");
            Canvas infoInslandCanvas = Instantiate(infoInslandCanvasPrefab);
            infoInslandCanvas.name = "infoInslandCanvas_" + this.name;
            Vector3 pos = infoInslandCanvas.transform.position;
            pos.z = -2;
            infoInslandCanvas.transform.position = pos;
            Island.infoIslandName = infoInslandCanvas.name;
        }
    }

    //returns le name of an island's speciality
    static public string getSpecialityNameIsland(string nameIsland)
    {
        switch (nameIsland)
        {
            case "sous_ile_1":
                return "Ile de l'Or";
            case "sous_ile_2":
                return "Ile du Pétrole";
            case "sous_ile_3":
                return "Ile de la Pierre";
            case "sous_ile_4":
                return "Ile du Bois";
            default:
                return string.Empty;
        }
    }
}
