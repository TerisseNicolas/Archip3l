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
        if (this.counter.text != "0")
        {
            Disturbance.islandChosen = this.name;
            this.counter.text = string.Empty;
            finalAction();
        }
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
        for (int i = 10; i >= 0 && Disturbance.islandChosen == string.Empty && !Disturbance.actionMade; i--)
        {
            this.counter.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        this.counter.text = string.Empty;
        if (Disturbance.islandChosen == string.Empty && !Disturbance.actionMade)
        {
            finalAction();
        }
    }

    void finalAction()
    {
        if (!Disturbance.actionMade)
        {
            Disturbance.actionMade = true;
            this.counter.text = string.Empty;
            for (int i = 1; i <= 4; i++)
                GameObject.Find("Disturbance-sous_ile_" + i.ToString()).GetComponent<BoxCollider>().enabled = false;

            /*--------------- random effect on resource-----*/
            System.Random ran = new System.Random();
            int aleat;
            int quantityLost = (-1) * (ran.Next(50, 100));
            //random type of Resource
            TypeResource resourceLost;
            do
            {
                aleat = ran.Next(0, Enum.GetNames(typeof(TypeResource)).Length);
                resourceLost = (TypeResource)Enum.Parse(typeof(TypeResource), Enum.GetNames(typeof(TypeResource))[aleat], true);
            }
            while (Enum.IsDefined(typeof(TypeResourceStat), resourceLost.ToString()));

            if (Disturbance.islandChosen == string.Empty)
            {
                this.disturbanceText.text = "Vous n'avez choisi aucune île !\n\nEn conséquence, la perturbation s'abattra \nsur toutes les îles !";
                StartCoroutine(envoiDataToutes(resourceLost, quantityLost));
                /*this.Client.sendData("@35770");
                Client.sendData("@25394@" + resourceLost.ToString() + "@" + "-" + quantityLost.ToString());
                */main.addNotification("Toutes les îles viennent de perdre " + (-quantityLost).ToString() + " de " + main.translateResourceName(resourceLost.ToString()));
            }
            else
            {
                string island = Disturbance.islandChosen.Split('-')[1];
                StartCoroutine(envoiDataUne(resourceLost, quantityLost, island));
                /*this.Client.sendData("@3" + island.Split('_')[2] + "770");
                Client.sendData("@2" + island.Split('_')[2] + "394@" + resourceLost.ToString() + "@" + quantityLost.ToString());
                */main.addNotification("L'île " + main.getIslandName(island) + " vient de perdre des ressources ! (" + main.translateResourceName(resourceLost.ToString()) + ")");
                for (int i = 1; i <= 4; i++)
                {
                    if (("Disturbance-sous_ile_" + i.ToString()) != Disturbance.islandChosen)
                    {
                        //GameObject.Find("Disturbance-sous_ile_" + i.ToString()).SetActive(false);
                        GameObject.Find("Disturbance-sous_ile_" + i.ToString()).GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }


            StartCoroutine(wait());

            Disturbance.actionMade = false; //"remise à 0" de l'attribut static
            Destroy(GameObject.Find("Disturbance"), 3.1f);
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        Disturbance.islandChosen = string.Empty;
        if (main.level == 1)
            main.addEnigma();
        Disturbance.disturbanceWindowOpen = false;
    }

    public IEnumerator envoiDataToutes(TypeResource resourceLost, int quantityLost)
    {
        this.Client.sendData("@35770");
        yield return new WaitForSeconds(0.5f);
        this.Client.sendData("@25394@" + resourceLost.ToString() + "@" + "-" + quantityLost.ToString());
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator envoiDataUne(TypeResource resourceLost, int quantityLost, string island)
    {
        this.Client.sendData("@3" + island.Split('_')[2] + "770");
        yield return new WaitForSeconds(0.5f);
        Client.sendData("@2" + island.Split('_')[2] + "394@" + resourceLost.ToString() + "@" + quantityLost.ToString());
        yield return new WaitForSeconds(0.1f);
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
            TouchTime = Time.time;
    }
    
    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }
    
}

