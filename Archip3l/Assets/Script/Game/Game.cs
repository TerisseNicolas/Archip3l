using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private Client Client;
    
    void Start()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemEndOfGameEvent += Client_MessageSystemEndOfGameEvent;
        //this.Client.MessageSystemStartInitOfGameAnswerEvent += Client_MessageSystemStartInitofGameAnswerEvent;

        //Launch initialization of the game
        //this.Client.sendData("@30006");

        StartCoroutine(startGameVertical());
    }
    
    IEnumerator startGameVertical()
    {
        yield return new WaitForSeconds(0.5f);
        this.Client.sendData("@30001");
    }

    //private void Client_MessageSystemStartInitofGameAnswerEvent(object sender, MessageEventArgs e)
    //{
    //    //Launch game
    //    this.Client.sendData("@30001");
    //}
    private void Client_MessageSystemEndOfGameEvent(object sender, MessageEventArgs e)
    {
        //End of the game
        Debug.Log("End of game");
        SceneSupervisor.Instance.loadEndScenes();
    }
}
