using UnityEngine;
using System.Collections;

public class EnigmaManager : MonoBehaviour {

    private Client Client;

    void awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        //this.Client.MessaC
    }
}
