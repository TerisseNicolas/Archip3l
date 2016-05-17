using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TouchScript.InputSources;
using System;

public class PirateBoatManager : MonoBehaviour
{
    private float initInterval = Int32.Parse(ConstantsLoader.getConstant(TypeConstant.pirateBoatsInitInterval));
    private float interval;
    private float raisingRate = float.Parse(ConstantsLoader.getConstant(TypeConstant.pirateBoatsRaisingRate));
    private int boatId = 0;

    private List<string> constants;

    public Transform pirateBoatPrefab;
    private Client Client; 

    private MinorIsland island1;
    private MinorIsland island2;
    private MinorIsland island3;
    private MinorIsland island4;

    private Vector3 targetPosition;
    private Vector3 initPosition;

    private static System.Random rnd;

    private bool launchBoats = false;
    private bool raisingFlag = false;

    //Fixes coroutine bug
    private bool CoroutineBoat = false;

    void Awake()
    {
        rnd = new System.Random();
        this.island1 = GameObject.Find("sous_ile_1").GetComponent<MinorIsland>();
        this.island2 = GameObject.Find("sous_ile_2").GetComponent<MinorIsland>();
        this.island3 = GameObject.Find("sous_ile_3").GetComponent<MinorIsland>();
        this.island4 = GameObject.Find("sous_ile_4").GetComponent<MinorIsland>();

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessagePiratesStartArrivalEvent += Client_MessagePiratesStartArrivalEvent;
        this.Client.MessagePiratesIncreaseRateEvent += Client_MessagePiratesIncreaseRateEvent;

        this.interval = this.initInterval;

    }

    void Update()
    {
        if (GameObject.Find("Explosion1(Clone)") != null)
            Destroy(GameObject.Find("Explosion1(Clone)"), 0.5f);
        if (GameObject.Find("sinkingTrail(Clone)") != null)
            Destroy(GameObject.Find("sinkingTrail(Clone)"), 0.5f);
        if(this.CoroutineBoat)
        {
            StartCoroutine("StartLaunchingPirateBoats");
            this.CoroutineBoat = false;
        }
    }

    private void Client_MessagePiratesIncreaseRateEvent(object sender, MessageEventArgs e)
    {
        this.raisingFlag = true;
    }

    private void Client_MessagePiratesStartArrivalEvent(object sender, MessageEventArgs e)
    {
        launchBoats = true;
        this.CoroutineBoat = true;
    }

    IEnumerator StartLaunchingPirateBoats()
    {
        yield return new WaitForSeconds(3);

        for (;;)
        {
            if (launchBoats)
            {
                launchPirateBoat();

                this.boatId += 1;
                if(this.raisingFlag)
                {
                    this.interval *= raisingRate;
                }
            }
            yield return new WaitForSeconds(this.interval);
        }
    }

    //To stop boats appearance
    IEnumerator wait()
    {
        yield return new WaitForSeconds(15);
        launchBoats = false;
    }

    private void launchPirateBoat()
    {
        switch (rnd.Next(1, 5))
        {
            case 1:
                this.targetPosition = new Vector3(this.island1.GetComponent<MeshCollider>().bounds.center.x, this.island1.GetComponent<MeshCollider>().bounds.center.y, 0); break;
            case 2:
                this.targetPosition = new Vector3(this.island2.GetComponent<MeshCollider>().bounds.center.x, this.island2.GetComponent<MeshCollider>().bounds.center.y, 0); break;
            case 3:
                this.targetPosition = new Vector3(this.island3.GetComponent<MeshCollider>().bounds.center.x, this.island3.GetComponent<MeshCollider>().bounds.center.y, 0); break;
            case 4:
                this.targetPosition = new Vector3(this.island4.GetComponent<MeshCollider>().bounds.center.x, this.island4.GetComponent<MeshCollider>().bounds.center.y, 0); break;
        }

        //pirateBoat.transform.SetParent(this.transform);
        this.initPosition = getNewBoatPosition(); // Camera.main.ScreenToWorldPoint(new Vector3(0, 100, -4));

        var pirateBoatTransform = Instantiate(pirateBoatPrefab) as Transform; //, initPosition, initRotation) as Transform;
        pirateBoatTransform.name = "PirateBoat_" + this.boatId;
        MovePirateBoat pirateBoat = pirateBoatTransform.GetComponent<MovePirateBoat>();
        if (pirateBoat != null)
        {
            pirateBoat.init(this.initPosition, this.targetPosition);
        }
    }

    private Vector3 getNewBoatPosition()
    {
        switch (rnd.Next(1, 5))
        {
            case 1:
                return new Vector3(rnd.Next(-700, -600), rnd.Next(0, 300), -2);
            case 2:
                return new Vector3(rnd.Next(600, 700), rnd.Next(0, 300), -2);
            case 4:
                return new Vector3(rnd.Next(-700, -600), rnd.Next(-300, 0), -2);
            case 3:
                return new Vector3(rnd.Next(600, 700), rnd.Next(-300, 0), -2);
        }
        return new Vector3(0, 0);
    }
}
