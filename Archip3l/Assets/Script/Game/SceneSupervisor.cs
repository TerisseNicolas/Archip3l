using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSupervisor : MonoBehaviour {

    private Client Client;
    public static SceneSupervisor Instance;

    //to fix call from events
    //function
    public bool _loadLoadingScene = false;
    public bool _loadMenuScenes = false;
    public bool _loadRegisterScene = false;
    public bool _loadLoadingScences = false;
    public bool _loadUnlockingScenes = false;
    public bool _loadBriefingScenes = false;
    public bool _loadTutoScenes = false;
    public bool _loadPalyingScenes = false;
    public bool _loadEndScenes = false;
    public bool _loadResultScenes = false;
    public bool _loadCreditScenes = false;

    //args
    public bool _loadingSceneVertical = false;
    public bool _loadResultScenesIndependent = false;


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
    }

    void Update()
    {
        if(_loadLoadingScene)
        {
            SceneManager.LoadScene("loading");
            this._loadLoadingScene = false;
        }
        if (_loadMenuScenes)
        {
            SceneManager.LoadScene("menuScene");
            if (this._loadingSceneVertical)
            {
                this.Client.sendData("@30000@BoardLoadingScene");
            }
            this._loadMenuScenes = false;
        }
        if (_loadRegisterScene)
        {
            SceneManager.LoadScene("registerScene");
            this._loadRegisterScene = false;
        }
        if (_loadLoadingScences)
        {
            this.Client.sendData("@30000@BoardLoadingScene");
            SceneManager.LoadScene("loading");
            this._loadLoadingScences = false;
        }
        if (_loadUnlockingScenes)
        {
            this.Client.sendData("@30000@BoardUnlockingTutoScene");
            SceneManager.LoadScene("unlockingTutoScene");
            this._loadUnlockingScenes = false;
        }
        if (_loadBriefingScenes)
        {
            this.Client.sendData("@30000@BoardBriefingScene");
            SceneManager.LoadScene("briefingScene");
            this._loadBriefingScenes = false;
        }
        if (_loadTutoScenes)
        {
            this.Client.sendData("@30000@BoardTutoScene");
            SceneManager.LoadScene("tutoScene");
            this._loadTutoScenes = false;
        }
        if (_loadPalyingScenes)
        {
            this.Client.sendData("@30000@BoardPlayingScene");
            SceneManager.LoadScene("playingScene");
            this._loadPalyingScenes = false;
        }
        if (_loadEndScenes)
        {
            this.Client.sendData("@30000@BoardEndScene");
            SceneManager.LoadScene("waitForVerticalSceneEnd");
            this._loadEndScenes = false;
        }
        if (_loadResultScenes)
        {
            this.Client.sendData("@30000@BoardResultScene");
            if (this._loadResultScenesIndependent)
            {
                SceneManager.LoadScene("waitForVerticalIndependentSceneResult");
            }
            else
            {
                SceneManager.LoadScene("waitForVerticalSceneResult");
            }
            this._loadResultScenes = false;
        }
        if (_loadCreditScenes)
        {
            this.Client.sendData("@30000@BoardCreditScene");
            SceneManager.LoadScene("waitForVerticalSceneMenu");
            this._loadCreditScenes = false;
        }
    }

    public void loadLoadingScene()
    {
        this._loadLoadingScene = true;
    }
    public void loadMenuScenes(bool loadingSceneVertical)
    {
        this._loadingSceneVertical = loadingSceneVertical;
        this._loadMenuScenes = true;
    }
    public void loadRegisterScene()
    {
        this._loadRegisterScene = true;
    }
    public void loadLoadingScences()
    {
        this._loadLoadingScences = true;
    }
    public void loadUnlockingScenes()
    {
        this._loadUnlockingScenes = true;
    }
    public void loadBriefingScenes()
    {
        this._loadBriefingScenes = true;
    }
    public void loadTutoScenes()
    {
        this._loadTutoScenes = true;
    }
    public void loadPalyingScenes()
    {
        this._loadPalyingScenes = true;
    }
    public void loadEndScenes()
    {
        this._loadEndScenes = true;
    }
    public void loadResultScenes(bool independent)
    {
        this._loadResultScenesIndependent = independent;
        this._loadResultScenes = true;
    }
    public void loadCreditScenes()
    {
        this._loadCreditScenes = true;
    }
}
