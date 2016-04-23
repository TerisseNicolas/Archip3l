using UnityEngine;
using System.Collections;
using TouchScript.InputSources;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterScene : InputSource {

    static public Text teamName;
    static public int level = 0;    //collège : 0, lycée : 1

    private Client Client;

    
    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        if (this.name == "enter")   //to do it one time only
        {   
            RegisterScene.teamName = GameObject.Find("TeamNameValue").GetComponent<Text>();
            teamName.text = string.Empty;
        }
        if (this.name == "college") //already selected
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    void OnMouseDownSimulation(){

        if (this.name == "back")
        {
            if (RegisterScene.teamName.text != string.Empty)
                RegisterScene.teamName.text = RegisterScene.teamName.text.Substring(0, RegisterScene.teamName.text.Length - 1);
        }
        else if (this.name == "enter")  //change scene + send name to Vertical (store in file)
        {
            if (teamName.text != string.Empty)
            {
                StartCoroutine(envoiData());
                /*this.Client.sendData("@30004@" + RegisterScene.teamName.text);
                this.Client.sendData("@30005@" + RegisterScene.level.ToString());
                SceneSupervisor.Instance.loadUnlockingScenes();*/
            }
        }
        else if (this.name == "PreviousScene")
            SceneSupervisor.Instance.loadLoadingScene();
        else if (this.name == "space")
        {
            if (teamName.text != string.Empty)
                RegisterScene.teamName.text += " ";
        }
        else if (this.name == "college")
        {
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChoixNiveau/boutonCollegeValide");
            GameObject.Find("lycee").GetComponent<BoxCollider>().enabled = true;
            GameObject.Find("lycee").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChoixNiveau/boutonLyceeGrise");
            RegisterScene.level = 0;
        }
        else if (this.name == "lycee")
        {
            if (GetComponent<SpriteRenderer>().sprite == null)
                Debug.Log("null");
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChoixNiveau/boutonLyceeValide");
            GameObject.Find("college").GetComponent<BoxCollider>().enabled = true;
            GameObject.Find("college").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChoixNiveau/boutonCollegeGrise");
            RegisterScene.level = 1;
        }
        else
            RegisterScene.teamName.text += this.name;


    }

    public IEnumerator envoiData()
    {
        this.Client.sendData("@30004@" + RegisterScene.teamName.text);
        yield return new WaitForSeconds(0.5f);
        this.Client.sendData("@30005@" + RegisterScene.level.ToString());
        yield return new WaitForSeconds(0.1f);
        SceneSupervisor.Instance.loadUnlockingScenes();
    }

        // Use this for initialization
        void Start () {
	}


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime;

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
            if (this.name != "college" && this.name != "lycee")
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("keyboard/" + this.name + "Clic");
        }

    }
    
    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 0.5)
        {
            this.OnMouseDownSimulation();
        }
        if (this.name != "college" && this.name != "lycee")
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("keyboard/" + this.name);
        TouchTime = 0;

    }
    

}
