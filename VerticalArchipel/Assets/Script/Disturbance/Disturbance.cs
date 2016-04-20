using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;
using System.Collections;
using System;
using TouchScript;



public class Disturbance : InputSource
{
    static public bool disturbanceWindowOpen = false;

    public Text counter;
    public Text disturbanceText;
    static public string islandChosen = string.Empty;
    static public bool actionMade = false;  //not to repeat the final action as many times as there are scripts attached to objects
    public TypeDisturbance disturbanceType;

    private Client Client;

    void OnMouseDownSimulation()
    {
        Disturbance.islandChosen = this.name;
        finalAction();
    }


    void Start()
    {
        //close other windows
        if (ChallengeVertical.challengeWindowPresent)
        {
            Destroy(GameObject.Find(ChallengeVertical.challengeWindowName));
            ChallengeVertical.challengeWindowPresent = false;
        }
        if (Trophy.infoWindowPresent)
        {
            Destroy(GameObject.Find(Trophy.infoWindowName));            
            Trophy.infoWindowPresent = false;
        }
        if (Island.infoIslandPresent)
        {
            Destroy(GameObject.Find(Island.infoIslandName));
            Island.infoIslandPresent = false;
        }
        if (ChallengeWon.challengeWonWindowPresent)
        {
            Destroy(GameObject.Find(ChallengeWon.challengeWonWindowName));
            ChallengeWon.challengeWonWindowPresent = false;
        }
        if (Enigma.enigmaWindowOpen)
        {
            Destroy(GameObject.Find(Enigma.enigmaWindowName));
            Enigma.enigmaWindowOpen = false;
        }
        

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.counter = GameObject.Find("DisturbanceCounter").GetComponent<Text>();
        this.disturbanceText = GameObject.Find("DisturbanceText").GetComponent<Text>();

        if (name.Contains("sous_ile_1"))
        {
            StartCoroutine(counterDecrement());
        }
    }


    IEnumerator counterDecrement()
    {
        for (int i = 10; i >= 0; i--)
        {
            if (Disturbance.islandChosen == string.Empty)
            {
                this.counter.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            else
            {
                this.counter.text = string.Empty;
                break;
            }
        }

        if (!Disturbance.actionMade)
        {
            finalAction();
        }
    }

    void finalAction()
    {
        Disturbance.actionMade = true;
        for (int i = 1; i <= 4; i++)
            GameObject.Find("Disturbance-sous_ile_" + i.ToString()).GetComponent<BoxCollider>().enabled = false;

        /*--------------- random effect on resource-----*/
        System.Random ran = new System.Random();
        int aleat;
        int quantityLost = (-1)*(ran.Next(0, 50));
        //random type of Resource
        TypeResource resourceLost;
        aleat = ran.Next(0, Enum.GetNames(typeof(TypeResource)).Length);
        resourceLost = (TypeResource)Enum.Parse(typeof(TypeResource), Enum.GetNames(typeof(TypeResource))[aleat], true);
        /*-------------------*/

        if (Disturbance.islandChosen == string.Empty)
        {
            this.disturbanceText.text = "Vous n'avez choisi aucune île !\n\nEn conséquence, la perturbation s'abattra \nsur toutes les îles !";
            this.Client.sendData("@35770");
            //TODO : check remove resource from each island
            Client.sendData("@25394@" + resourceLost.ToString() + "@" + "-" + quantityLost.ToString());
            main.addNotification("Toutes les îles viennent de perdre " + (-quantityLost).ToString() + " de " + main.translateResourceName(resourceLost.ToString()));
        }
        else
        {
            string island = Disturbance.islandChosen.Split('-')[1];
            this.Client.sendData("@3" + island.Split('_')[2] + "770");
            //TODO : check remove resource from the island
            Client.sendData("@2" + island.Split('_')[2] + "394@" + resourceLost.ToString() + "@" + quantityLost.ToString());
            main.addNotification("L'île " + island.Split('_')[2] + " vient de perdre " + (-quantityLost).ToString() + " de " + main.translateResourceName(resourceLost.ToString()));
            for (int i = 1; i <= 4; i++)
            {
                if (("Disturbance-sous_ile_" + i.ToString()) != Disturbance.islandChosen)
                {
                    GameObject.Find("Disturbance-sous_ile_" + i.ToString()).SetActive(false);
                }
            }
        }


        Disturbance.islandChosen = string.Empty;
        StartCoroutine(wait());

        Disturbance.disturbanceWindowOpen = false;
        Disturbance.actionMade = false; //"remise à 0" de l'attribut static
        Destroy(GameObject.Find("Disturbance"), 3.1f);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        main.addEnigma();
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
        //this.OnMouseDownSimulation();
        TouchTime = Time.time;
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
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
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

