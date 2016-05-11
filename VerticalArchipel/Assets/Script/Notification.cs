using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System.Collections.Generic;



public class Notification : OneTap
{

    protected override void OnMouseDownSimulation()
    {
        main.removeNotification(this.gameObject);
    }
}
