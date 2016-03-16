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
            case "tutoScene":
                SceneManager.LoadScene("verticalTuto");
                break;
            case "creditScene":
                SceneManager.LoadScene("creditsScene");
                break;
            case "resultScene":
                SceneManager.LoadScene("ResultScene");
                break;
            case "playingScene":
                SceneManager.LoadScene("verticalPlayingScene");
                break;
            case "loadingScene":
                SceneManager.LoadScene("loading");
                break;
        }
    }
}
