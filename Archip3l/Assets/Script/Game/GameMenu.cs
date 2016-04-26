using UnityEngine;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using TouchScript.InputSources;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMenu : InputSource
{
    private Client Client;
    private bool unlockedScene;
    private bool QuitApplication;

    void Awake()
    {
        this.unlockedScene = false;
        this.QuitApplication = false;
    }

    void Start()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        StartCoroutine(unlockScene());
    }

    IEnumerator unlockScene()
    {
        yield return new WaitForSeconds(5f);
        this.unlockedScene = true;
        //Debug.Log("Unlocked");
    }
    void OnMouseDownSimulation()
    {
        switch(this.name)
        {
            case "Jouer":
                SceneSupervisor.Instance.loadRegisterScene();
                break;
            case "Credits":
                SceneSupervisor.Instance.loadCreditScenes();
                break;
            case "Classement":
                SceneSupervisor.Instance.loadResultScenes(true);
                break;
            case "returnArrow":
            case "Quitter":
                //SceneSupervisor.Instance.loadLoadingScene();
                this.QuitApplication = true;
                break;
            //waitForVertical scene --> return to menu
            case "endWindowBackground":
                switch (SceneManager.GetActiveScene().name)
                {
                    //from the menu, access to the result or credit scene
                    case "waitForVerticalSceneMenu":
                        SceneSupervisor.Instance.loadMenuScenes(true);
                        break;
                    //when end scene is on vertical
                    case "waitForVerticalSceneEnd":
                        if(this.unlockedScene)
                        {
                            SceneSupervisor.Instance.loadResultScenes(false);
                        }
                        break;
                    //when result scene is on vertical(at the end of the game)
                    case "waitForVerticalSceneResult":
                        SceneSupervisor.Instance.loadCreditScenes();
                        break;
                    case "waitForVerticalIndependentSceneResult":
                        SceneSupervisor.Instance.loadMenuScenes(true);
                        break;
                }
                break;
        }
    }

    void Update()
    {
        if(this.QuitApplication)
        {
            StartCoroutine(quitGame());
            this.QuitApplication = false;
        }
    }

    IEnumerator quitGame()
    {
        this.Client.sendData("@30100");
        yield return new WaitForSeconds(0.3f);
        Application.Quit();
    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime = 0;

    private MetaGesture gesture;

    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }
    

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (TouchTime == 0)
        {
            TouchTime = Time.time;
            if (this.name == "Jouer" || this.name == "Credits" || this.name == "Classement" || this.name == "Quitter")
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("menu/" + this.name + "Clic");
        }

    }
    

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 1.5)
        {
            if (this.name == "Jouer" || this.name == "Credits" || this.name == "Classement" || this.name == "Quitter")
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("menu/" + this.name);
            this.OnMouseDownSimulation();
        }
        TouchTime = 0;
    }
    

}
