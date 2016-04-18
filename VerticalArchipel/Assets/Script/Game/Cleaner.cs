using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cleaner : MonoBehaviour {

    private List<String> ToBeSaved;

    void Awake()
    {
        this.ToBeSaved = new List<string>();
        this.ToBeSaved.Add("Main Camera");
        this.ToBeSaved.Add("logo2_sans_gears");
        this.ToBeSaved.Add("v");
        this.ToBeSaved.Add("vv");
        this.ToBeSaved.Add("vvv");
        this.ToBeSaved.Add("vvvv");
        this.ToBeSaved.Add("vvvvv");
        this.ToBeSaved.Add("vvvvvv");
        this.ToBeSaved.Add("waves");
        this.ToBeSaved.Add("Cleaner");
        this.ToBeSaved.Add("GlobalInfo");
        this.ToBeSaved.Add("Network");
        this.ToBeSaved.Add("Music");
        this.ToBeSaved.Add("TouchScript");
        this.ToBeSaved.Add("GestureManager Instance");
        this.ToBeSaved.Add("TouchManager Instance");

    }
	void Start ()
    {
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if(!mustbeSaved(obj.name))
            {
                Debug.Log("Destroying " + obj.name);
                Destroy(obj);
            }
        }
    }

    private bool mustbeSaved(string name)
    {
        foreach(string elt in this.ToBeSaved)
        {
            if(elt == name)
            {
                return true;
            }
        }
        return false;
    }
}
