using UnityEngine;
using System.Collections;
using System;

public class GlobalInfo : MonoBehaviour {

    public string teamName;
    public int teamLevel;

    private Client Client;

    void Awake()
    {
        this.teamName = string.Empty;
        this.teamLevel = 0;
    }

    void Start()
    {
        //Keep the game object at scene change
        DontDestroyOnLoad(transform.gameObject);

        this.teamName = string.Empty;
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemTeamNameEvent += Client_MessageSystemTeamNameEvent;
        this.Client.MessageSystemTeamLevelEvent += Client_MessageSystemTeamLevelEvent;
    }

    private void Client_MessageSystemTeamLevelEvent(object sender, MessageEventArgs e)
    {
        this.teamLevel = Int32.Parse((string)e.message.Split('@').GetValue(2));
    }

    private void Client_MessageSystemTeamNameEvent(object sender, MessageEventArgs e)
    {
        this.teamName = (string)e.message.Split('@').GetValue(2);
    }
}
