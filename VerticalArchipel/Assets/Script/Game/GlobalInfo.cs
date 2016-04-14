using UnityEngine;
using System.Collections;

public class GlobalInfo : MonoBehaviour {

    public string teamName;
    private Client Client;

    void Start()
    {
        //Keep the game object at scene change
        DontDestroyOnLoad(transform.gameObject);

        this.teamName = string.Empty;
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemTeamNameEvent += Client_MessageSystemTeamNameEvent;

        //Watch out, the client should not be used after the scene changes
    }

    private void Client_MessageSystemTeamNameEvent(object sender, MessageEventArgs e)
    {
        this.teamName = (string)e.message.Split('@').GetValue(2);
    }
}
