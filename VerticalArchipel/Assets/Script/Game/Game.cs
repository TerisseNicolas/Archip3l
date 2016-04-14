﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    private Timer Timer;
    private DisturbanceTimer TimerDisturbance;
    private ChallengeTimer ChallengerTimer;
    private int DisturbanceCount;
    private Client Client;
    private GlobalResourceManager GlobalResourceManager;

    private Score Score;
    private GlobalInfo GlobalInfo;

    public const int nbChallengesMax = 3;

    private List<GameObject> ChallengesGameObjects;

    private bool CoroutineInitOfGame = false;

    void Start()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemStartOfGameEvent += Client_MessageSystemStartOfGame;
        this.Client.MessageSystemStartInitOfGameEvent += Client_MessageSystemStartInitOfGame;

        this.Timer = gameObject.GetComponent<Timer>();
        this.Timer.Init(2f * 60f);
        //this.Timer.Init(13f * 60f);
        this.Timer.FinalTick += Timer_FinalTick;
        this.Timer.PirateBoatsStartTick += Timer_PirateBoatsStartTick;
        this.Timer.PirateBoatsIncreaseTick += Timer_PirateBoatsIncreaseTick;

        this.TimerDisturbance = gameObject.GetComponent<DisturbanceTimer>();
        //Todo : keep the good one
        this.TimerDisturbance.Init(180f);
        //this.TimerDisturbance.Init(10f);
        this.TimerDisturbance.FinalTick += TimerDisturbance_FinalTick;
        this.DisturbanceCount = 0;

        this.ChallengerTimer = gameObject.GetComponent<ChallengeTimer>();
        //Todo : keep the good one
        //this.ChallengerTimer.Init(30f);
        this.ChallengerTimer.Init(5f);
        this.ChallengerTimer.FinalTick += ChallengerTimer_FinalTick;

        this.GlobalResourceManager = GameObject.Find("Resources").GetComponent<GlobalResourceManager>();
        this.GlobalResourceManager.MessageInitialized += GlobalResourceManager_MessageInitialized;

        this.Score = gameObject.GetComponent<Score>();
        this.GlobalInfo = GameObject.Find("GlobalInfo").GetComponent<GlobalInfo>();

        this.ChallengesGameObjects = new List<GameObject>();
        for (int i = 1; i <= nbChallengesMax; i++)
        {
            this.ChallengesGameObjects.Add(GameObject.Find("Challenge" + i.ToString()));
        }

        Client_MessageSystemStartOfGame(this, null);
    }
    
    void Update()
    {
        if(this.CoroutineInitOfGame)
        {
            StartCoroutine(this.GlobalResourceManager.initResources());
            this.CoroutineInitOfGame = false;
        }
    }

    private void Client_MessageSystemStartOfGame(object sender, MessageEventArgs e)
    {
        Debug.Log("Starting game");
        this.Timer.StartTimer();
        this.TimerDisturbance.StartTimer();
        this.ChallengerTimer.StartTimer();
    }
    private void Client_MessageSystemStartInitOfGame(object sender, MessageEventArgs e)
    {
        Debug.Log("Start initializing game");
        this.CoroutineInitOfGame = true;
    }
    private void GlobalResourceManager_MessageInitialized(object sender, System.EventArgs e)
    {
        this.Client.sendData("@30087");
    }
    private void Timer_FinalTick(object sender, System.EventArgs e)
    {
        this.Client.sendData("@30002");
        this.Score.addScore(this.GlobalInfo.teamName);
    }
    private void ChallengerTimer_FinalTick(object sender, System.EventArgs e)
    {
        for (int i = 0; i < nbChallengesMax; i++)
        {
            if (this.ChallengesGameObjects[i].GetComponent<SpriteRenderer>().enabled == false)
            {
                this.ChallengesGameObjects[i].GetComponent<SpriteRenderer>().enabled = true;
                this.ChallengesGameObjects[i].GetComponent<BoxCollider>().enabled = true;
            }
        }

        this.ChallengerTimer.Init(30f);
        this.ChallengerTimer.StartTimer();
    }

    private void Timer_PirateBoatsIncreaseTick(object sender, System.EventArgs e)
    {
        Debug.LogWarning("Increase pirate boats appearance rate");
        this.Client.sendData("@40001");
    }

    private void Timer_PirateBoatsStartTick(object sender, System.EventArgs e)
    {
        Debug.LogWarning("Launch pirate boats");
        this.Client.sendData("@40002");
    }

    private void TimerDisturbance_FinalTick(object sender, System.EventArgs e)
    {
        if (!Disturbance.disturbanceWindowOpen)
        {
            Disturbance.disturbanceWindowOpen = true;
            Canvas disturbancePrefab = Resources.Load<Canvas>("Prefab/DisturbanceCanvas");
            Canvas disturbance = Instantiate(disturbancePrefab);
            disturbance.name = "Disturbance";
            

            this.DisturbanceCount += 1;
            switch (this.DisturbanceCount)
            {
                case 1:
                case 2:
                    this.TimerDisturbance.Init(180f);
                    break;
                case 3:
                    this.TimerDisturbance.Init(120f);
                    break;
                case 4:
                    this.TimerDisturbance.Init(60f);
                    break;
            }
            this.TimerDisturbance.StartTimer();            
        }
    }
}
