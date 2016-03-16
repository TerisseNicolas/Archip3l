using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSupervisor : MonoBehaviour {

    private Client Client;

    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemChangeSceneEvent += Client_MessageSystemChangeSceneEvent;
    }

    private void Client_MessageSystemChangeSceneEvent(object sender, MessageEventArgs e)
    {
        string sceneName = (string)e.message.Split('@')[2];

        switch(sceneName)
        {
           case "BoardUnlockingTutoScene":
                SceneManager.LoadScene("verticalUnlockingTuto");
                break;
            case "BoardTutoScene":
                SceneManager.LoadScene("verticalTuto");
                break;
            case "BoardCreditScene":
                SceneManager.LoadScene("creditsScene");
                break;
            case "BoardResultScene":
                SceneManager.LoadScene("ResultScene");
                break;
            case "BoardPlayingScene":
                SceneManager.LoadScene("verticalPlayingScene");
                break;
            case "BoardLoadingScene":
                SceneManager.LoadScene("loading");
                break;
        }
    }
}
