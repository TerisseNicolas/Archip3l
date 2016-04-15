using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSupervisor : MonoBehaviour {

    private Client Client;
    private string sceneName = string.Empty;

    void Start()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemChangeSceneEvent += Client_MessageSystemChangeSceneEvent;
    }

    void Update()
    {
        if(this.sceneName != string.Empty)
        {
            switch (sceneName)
            {
                case "BoardBriefingScene":
                    SceneManager.LoadScene("briefingScene");
                    break;
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
                case "BoardEndScene":
                    SceneManager.LoadScene("endScene");
                    break;
            }
            this.sceneName = string.Empty;
        }
    }

    private void Client_MessageSystemChangeSceneEvent(object sender, MessageEventArgs e)
    {
       this.sceneName = (string)e.message.Split('@')[2];
    }
}
