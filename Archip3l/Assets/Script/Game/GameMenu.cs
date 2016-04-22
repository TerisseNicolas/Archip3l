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
                SceneSupervisor.Instance.loadLoadingScene();
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
                        SceneSupervisor.Instance.loadResultScenes(false);
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


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime;

    private MetaGesture gesture;
    private Dictionary<int, int> map = new Dictionary<int, int>();

    public override void CancelTouch(TouchPoint touch, bool @return)
    {
        base.CancelTouch(touch, @return);

        map.Remove(touch.Id);
        if (@return)
        {
            TouchHit hit;
            if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
            map.Add(touch.Id, beginTouch(processCoords(hit.RaycastHit.textureCoord), touch.Tags).Id);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchMoved += touchMovedhandler;
            gesture.TouchCancelled += touchCancelledhandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (gesture)
        {
            gesture.TouchBegan -= touchBeganHandler;
            gesture.TouchMoved -= touchMovedhandler;
            gesture.TouchCancelled -= touchCancelledhandler;
            gesture.TouchEnded -= touchEndedHandler;
        }
    }

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        map.Add(touch.Id, beginTouch(processCoords(touch.Hit.RaycastHit.textureCoord), touch.Tags).Id);
        if (TouchTime == 0)
        {
            TouchTime = Time.time;
            if (this.name == "Jouer" || this.name == "Credits" || this.name == "Classement" || this.name == "Quitter")
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("menu/" + this.name + "Clic");
        }

    }

    private void touchMovedhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        TouchHit hit;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
        moveTouch(id, processCoords(hit.RaycastHit.textureCoord));
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        endTouch(id);
        if (Time.time - TouchTime < 1.5)
        {
            if (this.name == "Jouer" || this.name == "Credits" || this.name == "Classement" || this.name == "Quitter")
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("menu/" + this.name);
            this.OnMouseDownSimulation();
        }
        TouchTime = 0;
    }

    private void touchCancelledhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        cancelTouch(id);
    }

}
