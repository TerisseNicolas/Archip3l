using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private Client Client;

    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemEndOfGameEvent += Client_MessageSystemEndOfGameEvent;
        this.Client.MessageSystemStartInitOfGameAnswerEvent += Client_MessageSystemStartInitofGameAnswerEvent;
    }    

    void Start()
    {
        //Launch initialization of the game
        this.Client.sendData("@30006");
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(4);
        Client_MessageSystemEndOfGameEvent(null, null);

    }
    private void Client_MessageSystemStartInitofGameAnswerEvent(object sender, MessageEventArgs e)
    {
        //Launch game
        this.Client.sendData("@30001");
    }
    private void Client_MessageSystemEndOfGameEvent(object sender, MessageEventArgs e)
    {
        //End of the game
        SceneSupervisor.Instance.loadEndScenes();
    }
}
