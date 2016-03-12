using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private Timer Timer;
    private DisturbanceTimer TimerDisturbance;
    private int DisturbanceCount;
    private Client Client;
    private GlobalResourceManager GlobalResourceManager;

    private Score Score;
    private GlobalInfo GlobalInfo;

    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemStartOfGameEvent += Client_MessageSystemStartOfGame;
        this.Client.MessageSystemStartInitOfGameEvent += Client_MessageSystemStartInitOfGame;

        this.Timer = gameObject.GetComponent<Timer>();
        //this.Timer.Init(0.1f * 60f);
        this.Timer.Init(13f * 60f);
        this.Timer.FinalTick += Timer_FinalTick;

        this.TimerDisturbance = gameObject.GetComponent<DisturbanceTimer>();
        this.TimerDisturbance.Init(10f);
        this.TimerDisturbance.FinalTick += TimerDisturbance_FinalTick;
        this.DisturbanceCount = 0;

        this.GlobalResourceManager = GameObject.Find("Resources").GetComponent<GlobalResourceManager>();
        this.GlobalResourceManager.MessageInitialized += GlobalResourceManager_MessageInitialized;

        this.Score = gameObject.GetComponent<Score>();
        //To be activated, the object come from the previous scene
        //this.GlobalInfo = GameObject.Find("GlobalInfo").GetComponent<GlobalInfo>();
    }

    void Start()
    {
        //Client_MessageSystemStartOfGame(this, null);
    }

    private void Client_MessageSystemStartOfGame(object sender, MessageEventArgs e)
    {
        Debug.Log("Starting game");
        this.Timer.StartTimer();
        this.TimerDisturbance.StartTimer();
    }
    private void Client_MessageSystemStartInitOfGame(object sender, MessageEventArgs e)
    {
        Debug.Log("Start initializing game");
        StartCoroutine(this.GlobalResourceManager.initResources());
    }
    private void GlobalResourceManager_MessageInitialized(object sender, System.EventArgs e)
    {
        this.Client.sendData("@30087");
    }
    private void Timer_FinalTick(object sender, System.EventArgs e)
    {
        //End of the game
        this.Client.sendData("@30002");
        //To be activated
        //this.Score.addScore(this.GlobalInfo.teamName);
    }
    private void TimerDisturbance_FinalTick(object sender, System.EventArgs e)
    {
        if (!Disturbance.disturbanceWindowOpen)
        {
            Disturbance.disturbanceWindowOpen = true;
            Canvas eventCanvasPrefab = Resources.Load<Canvas>("Prefab/DisturbanceCanvas");
            Canvas eventCanvas = Instantiate(eventCanvasPrefab);
            eventCanvas.name = "DisturbanceCanvas";
            /*foreach (Disturbance e in eventCanvas.GetComponentsInChildren<Disturbance>())
            {
                e.disturbanceType = disturbanceType;
            }*/

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
