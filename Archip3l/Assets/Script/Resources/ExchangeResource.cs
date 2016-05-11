using UnityEngine;
using UnityEngine.UI;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;
using System;

public class ExchangeResource : InputSource
{

    public MinorIsland island;
    private Client Client;

    SpriteRenderer less = null;
    SpriteRenderer more = null;
    SpriteRenderer resource_sp = null;
    SpriteRenderer send = null;
    Text quantityValue;
    Text to;

    SpriteRenderer exchangeResourceAnimation;



    void refresh()
    {
        foreach (SpriteRenderer sp in this.transform.parent.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sp.name == "Less")
            {
                less = sp;
            }
            if (sp.name == "More")
            {
                more = sp;
            }
            if (sp.name == "ResourceValue")
            {
                resource_sp = sp;
            }
            if (sp.name == "Send")
            {
                send = sp;
            }
        }
        foreach (Text text in this.transform.parent.GetComponentsInChildren<Text>())
        {
            if (text.name == "QuantityValue")
            {
                quantityValue = text;
            }
            if (text.name == "Island")
            {
                to = text;
            }
        }
    }

    void OnMouseDownSimulation()
    {

        this.Client = GameObject.Find("Network").GetComponent<Client>();

        refresh();

        if (!island.otherWindowOpen)
        {
            string resourceName;
            int resourceQuantity;
            if (resource_sp.GetComponent<SpriteRenderer>().sprite.name == "cercleChoixRessource"){  //default sprite
                resourceName = string.Empty;
                resourceQuantity = 0;
            }
            else
            {
                resourceName = Resource.getResourceFromIconName(resource_sp.GetComponent<SpriteRenderer>().sprite.name);
                resourceQuantity = (int) this.island.resourceManager.getResource((TypeResource)System.Enum.Parse(typeof(TypeResource), resourceName)).Stock;
            }

            Vector3 vector3;

            switch (this.name)
            {
                case "ResourceValue":
                    Canvas listResourcesCanvasPrefab = Resources.Load<Canvas>("Prefab/ListResourcesCanvas");
                    Canvas listResourcesCanvas = Instantiate(listResourcesCanvasPrefab);
                    listResourcesCanvas.name = "listResourcesCanvas_" + island.nameMinorIsland;
                    listResourcesCanvas.transform.SetParent(GameObject.Find(island.nameMinorIsland).transform);
                    vector3 = GameObject.Find("sprite-" + island.nameMinorIsland).transform.position;
                    vector3.z = -6;
                    listResourcesCanvas.transform.position = vector3;
                    foreach (SpriteRenderer sr in listResourcesCanvas.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sr.GetComponent<BoxCollider>().enabled = true;
                        if ((sr.name != "Close") && (this.island.resourceManager.getResource((TypeResource)System.Enum.Parse(typeof(TypeResource), sr.name)).Stock < 5))
                        {
                            sr.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + sr.name + "Icon_disabled");
                            sr.GetComponent<BoxCollider>().enabled = false;
                        }
                    }
                    island.otherWindowOpen = true;
                    quantityValue.text = "0";
                    //rotation if other side of the table
                    char id = this.island.nameMinorIsland[this.island.nameMinorIsland.Length - 1];
                    if (id == '1' || id == '2')
                        listResourcesCanvas.transform.Rotate(Vector3.forward * 180);
                    break;
                case "Less":
                    if (quantityValue.text == "0")
                        this.GetComponent<BoxCollider>().enabled = false;
                    else
                    {
                        quantityValue.text = (int.Parse(quantityValue.text) - 5).ToString();
                        more.GetComponent<BoxCollider>().enabled = true;
                    }
                    break;
                case "More":
                    this.GetComponent<BoxCollider>().enabled = true;
                    if ((quantityValue.name == "QuantityValue") && (int.Parse(quantityValue.text) + 5 <= resourceQuantity))
                    {
                        quantityValue.text = (int.Parse(quantityValue.text) + 5).ToString();
                        less.GetComponent<BoxCollider>().enabled = true;
                    }
                    break;
                case "Island":
                    Canvas listIslandsCanvasPrefab = Resources.Load<Canvas>("Prefab/ListeIslandsCanvas");
                    Canvas listIslandsCanvas = Instantiate(listIslandsCanvasPrefab);
                    listIslandsCanvas.name = "listIslandsCanvas_" + island.nameMinorIsland;
                    listIslandsCanvas.transform.SetParent(GameObject.Find(island.nameMinorIsland).transform);
                    vector3 = GameObject.Find("sprite-" + island.nameMinorIsland).transform.position;
                    vector3.z = -6;
                    listIslandsCanvas.transform.position = vector3;
                    //disable clic on self island
                    foreach(SpriteRenderer sp in listIslandsCanvas.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (sp.name == island.nameMinorIsland)
                            sp.GetComponent<BoxCollider>().enabled = false;
                    }
                    island.otherWindowOpen = true;
                    //rotation if other side of the table
                    char idIsland = this.island.nameMinorIsland[this.island.nameMinorIsland.Length - 1];
                    if (idIsland == '1' || idIsland == '2')
                        listIslandsCanvas.transform.Rotate(Vector3.forward * 180);
                    break;
                case "Close":
                    island.exchangeWindowPresent = false;
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                    break;
                case "Send":
                    if (to.text == "Ile X")
                    {
                        island.displayPopup("Veuillez sélectionner une île, en appuyant sur \"Ile X\".", 3);
                    }
                    else
                    {
                        if (quantityValue.text == "0")
                        {
                            island.displayPopup("Veuillez sélectionner une quantité à envoyer", 3);
                        }
                        else
                        {
                            if (resource_sp.sprite.name == "Knob") //defaults sprite when not already clicked on
                            { 
                                island.displayPopup("Veuillez sélectionner une ressource à envoyer", 3);
                            }
                            else
                            {
                                if (MinorIsland.exchangePerforming)
                                {
                                    island.displayPopup("Veuillez attendre la fin de l'échange en cours", 3);
                                }
                                else
                                {
                                    //withdrawal of resources
                                    TypeResource tr = (TypeResource)System.Enum.Parse(typeof(TypeResource), Resource.getResourceFromIconName(this.resource_sp.sprite.name));
                                    int quantitySent = int.Parse(quantityValue.text);
                                    Resource res = island.resourceManager.getResource(tr);
                                    res.changeStock(-quantitySent);
                                    this.Client.sendData("@2" + this.island.nameMinorIsland.Split('_')[2] + "355@" + res.TypeResource.ToString() + "@" + (-quantitySent).ToString());

                                    MinorIsland.exchangePerforming = true;
                                    SpriteRenderer exchangeResourceAnimationPrefab = Resources.Load<SpriteRenderer>("Prefab/exchangeResourceAnimation/exchangeResourceAnimation_" + island.nameMinorIsland[island.nameMinorIsland.Length - 1].ToString()); 
                                    exchangeResourceAnimation = Instantiate(exchangeResourceAnimationPrefab);
                                    exchangeResourceAnimation.transform.parent = GameObject.Find(island.nameMinorIsland).transform;
                                    exchangeResourceAnimation.name = "ExchangeResourceAnimation_" + island.nameMinorIsland;
                                    exchangeResourceAnimation.GetComponent<BoatMoving>().islandToSend = "sous_ile_" + island.islandToSend[island.islandToSend.Length - 1].ToString();
                                    exchangeResourceAnimation.GetComponent<BoatMoving>().quantityCarried = quantitySent;
                                    exchangeResourceAnimation.GetComponent<BoatMoving>().resourceSent = tr.ToString();
                                    island.exchangeWindowPresent = false;
                                    island.displayPopup("Emmenez le bateau jusqu'au port de l'île sélectionnée pour lui apporter les ressources", 5);
                                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                                }
                            }
                        }
                    }
                    break;
            }
        }
        else    //closure of other windows (listResources & listIslands)
        {
            if (this.name == "Close")
            {
                island.otherWindowOpen = false;
                island.exchangeWindowPresent = false;
                Destroy(GameObject.Find("listIslandsCanvas_" + island.nameMinorIsland));
                Destroy(GameObject.Find("listResourcesCanvas_" + island.nameMinorIsland));
                Destroy(GameObject.Find(this.transform.parent.parent.name));
            }
        }

    }
        
        


    // Use this for initialization
    void Start()
    {
        Vector3 pos = this.transform.localPosition;
        pos.z = -3;
        this.transform.localPosition = pos;
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        refresh();
    }

    // Update is called once per frame
    void Update()
    {
        if (island.resource != string.Empty)
        {
            refresh();
            resource_sp.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + island.resource + "Icon");
            resource_sp.transform.localScale = new Vector2(0.1f, 0.1f);
            resource_sp.GetComponent<BoxCollider>().size = new Vector2(5, 5);
            //island.resource = string.Empty;
        }
        if (island.islandToSend != string.Empty)
        {
            refresh();
            switch(island.islandToSend[island.islandToSend.Length - 1].ToString())
            {
                case "1":
                    to.text = "Ile Jaune";
                    break;
                case "2":
                    to.text = "Ile Bleue";
                    break;
                case "3":
                    to.text = "Ile Grise";
                    break;
                case "4":
                    to.text = "Ile Marron";
                    break;
            }
        }
        if ((island.resource != string.Empty) && (island.islandToSend != string.Empty) && (this.quantityValue.text != "0"))
        {
            if (send.sprite.name != "boutonEnvoyer" && send.sprite.name != "boutonEnvoyerClic")
                send.sprite = Resources.Load<Sprite>("fenetreEchange/boutonEnvoyer");
        }
        else
        {
            if (send.sprite.name != "boutonEnvoyerGrise")
                send.sprite = Resources.Load<Sprite>("fenetreEchange/boutonEnvoyerGrise");
        }


    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime = 0;

    private TapGesture gesture;


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += pressedHandler;
    }


    private void pressedHandler(object sender, EventArgs e)
    {
        this.OnMouseDownSimulation();
    }

}
