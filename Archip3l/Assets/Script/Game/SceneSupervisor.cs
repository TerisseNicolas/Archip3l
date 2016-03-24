using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSupervisor : MonoBehaviour {

    private Client Client;
    public static SceneSupervisor Instance;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SceneSupervisor!");
            Destroy(gameObject);
        }
        Instance = this;

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemChangeSceneEvent += Client_MessageSystemChangeSceneEvent;
    }

    private void Client_MessageSystemChangeSceneEvent(object sender, MessageEventArgs e)
    {
        //All scene change request in the specific code (event)

        //string sceneName = (string)e.message.Split('@')[2];
        //switch(sceneName)
        //{
        //    case "TableTutoScene":
        //        //Todo to be deleted
        //        //Do not load scene unitl the board didn't completed the unlock
        //        //SceneManager.LoadScene("tutoScene");
        //        break;
        //    case "TableLoadingScene":
        //        SceneManager.LoadScene("loading");
        //        break;
        //}
    }
    public void loadLoadingScene()
    {
        SceneManager.LoadScene("loading");
    }
    public void loadMenuScenes(bool loadingSceneVertical)
    {
        SceneManager.LoadScene("menuScene");
        if(loadingSceneVertical)
        {
            this.Client.sendData("@30000@BoardLoadingScene");
        }
    }
    public void loadRegisterScene()
    {
        SceneManager.LoadScene("registerScene");
    }
    public void loadLoadingScences()
    {
        this.Client.sendData("@30000@BoardLoadingScene");
        SceneManager.LoadScene("loading");
    }
    public void loadUnlockingScenes()
    {
        this.Client.sendData("@30000@BoardUnlockingTutoScene");
        SceneManager.LoadScene("unlockingTutoScene");
    }
    public void loadTutoScenes()
    {
        this.Client.sendData("@30000@BoardTutoScene");
        SceneManager.LoadScene("tutoScene");
    }
    public void loadPalyingScenes()
    {
        this.Client.sendData("@30000@BoardPlayingScene");
        SceneManager.LoadScene("playingScene");
    }
    public void loadEndScenes()
    {
        this.Client.sendData("@30000@BoardEndScene");
        SceneManager.LoadScene("waitForVerticalSceneEnd");
    }
    public void loadResultScenes()
    {
        this.Client.sendData("@30000@BoardResultScene");
        SceneManager.LoadScene("waitForVerticalSceneResult");
    }
    public void loadCreditScenes()
    {
        this.Client.sendData("@30000@BoardCreditScene");
        SceneManager.LoadScene("waitForVerticalSceneMenu");
    }
}
