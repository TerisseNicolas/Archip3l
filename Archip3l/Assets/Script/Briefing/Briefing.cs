using UnityEngine;
using TouchScript.InputSources;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using System.Collections;

public class Briefing : InputSource
{
    private GameObject logoSprite;
    private List<Vector3> scales;
    private Vector3 changeScale;
    private Vector3 initScale;

    private bool animationFlag;
    private bool animationLaunchedFlag;

    private bool lockedScene;

    void Awake()
    {
        this.logoSprite = GameObject.Find("Logo");

        this.initScale = this.logoSprite.transform.localScale; // new Vector3(1.491848f, 1.495401f, 1f);
        this.changeScale = new Vector3(1.05f, 1.05f, 1);
        this.scales = new List<Vector3>();

        Vector3 newScale = initScale;
        for(int i = 0; i < 30; i++)
        {
            this.scales.Add(newScale);
            newScale = Vector3.Scale(this.changeScale, newScale);
        }
        this.logoSprite.gameObject.transform.localScale = newScale;

        this.animationFlag = false;
        this.animationLaunchedFlag = false;
        this.lockedScene = true;
    }

    void Start()
    {
        //SoundPlayer.Instance.playBriefingLetterSound();
        StartCoroutine(unlockScene());
    }

    void Update()
    {
        if(this.animationFlag && !this.animationLaunchedFlag && !lockedScene)
        {
            StartCoroutine(endAnimation());
            this.animationFlag = false;
            this.animationLaunchedFlag = true;
        }
    }
    private void OnMouseDownSimulation()
    {
        this.animationFlag = true;
    }

    IEnumerator unlockScene()
    {
        yield return new WaitForSeconds(7);
        this.lockedScene = false;
    }
    IEnumerator endAnimation()
    {
        for (int i = 30; i>0; i--)
        {
            this.logoSprite.gameObject.transform.Translate(new Vector3(0, 0, 0.3f));
            this.logoSprite.gameObject.transform.localScale = this.scales[i-1];
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(3);
        SceneSupervisor.Instance.loadTutoScenes();
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
            this.OnMouseDownSimulation();
            TouchTime = Time.time;
        }
    }
    
}
