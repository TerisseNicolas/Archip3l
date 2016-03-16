using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSupervisor : MonoBehaviour {

    private Client Client;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemChangeSceneEvent += Client_MessageSystemChangeSceneEvent;
    }

    private void Client_MessageSystemChangeSceneEvent(object sender, MessageEventArgs e)
    {
        string sceneName = (string)e.message.Split('@')[2];

        switch(sceneName)
        {
            case "TableTutoScene":
                //Do not load scene unitl the board didn't completed the unlock
                SceneManager.LoadScene("tutoScene");
                break;
            case "TableLoadingScene":
                SceneManager.LoadScene("loading");
                break;
        }
    }
}
