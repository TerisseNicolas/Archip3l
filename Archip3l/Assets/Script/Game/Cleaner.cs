using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cleaner : MonoBehaviour {

    private List<String> ToBeSaved;

    void Awake()
    {
        this.ToBeSaved = new List<string>();
        this.ToBeSaved.Add("Jouer");
        this.ToBeSaved.Add("Quitter");
        this.ToBeSaved.Add("Credits");
        this.ToBeSaved.Add("Classement");
        this.ToBeSaved.Add("returnArrow");
        this.ToBeSaved.Add("Main Camera");
        this.ToBeSaved.Add("endWindowBackground");
        this.ToBeSaved.Add("EventSystem");
        this.ToBeSaved.Add("ARCHIPEL");
        this.ToBeSaved.Add("Embellishment");
        this.ToBeSaved.Add("ParticleSystemMenu");
        this.ToBeSaved.Add("Embelishment2");
        this.ToBeSaved.Add("Particle System");
        this.ToBeSaved.Add("Particle System (1)");
        this.ToBeSaved.Add("Particle System (2)");
        this.ToBeSaved.Add("Embelishment3");

        this.ToBeSaved.Add("Cleaner");
        this.ToBeSaved.Add("GlobalInfo");
        this.ToBeSaved.Add("Network");
        this.ToBeSaved.Add("Music");
        this.ToBeSaved.Add("TouchScript");

        //hidden
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
