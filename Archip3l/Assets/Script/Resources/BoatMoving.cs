using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections;
using TouchScript;



public class BoatMoving : InputSource
{

    public MinorIsland island;
    public string islandToSend;
    public int quantityCarried;
    public string resourceSent;
    public bool appeared = false;
    public bool collided = false;
    public bool resetting = false;
    public Vector3 startPosition;

	public GameObject sinkingTrail;

    private GameObject harbor;

    private Client Client;

    private float x1, y1;

    void OnTriggerEnter(Collider col)
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();

        if (col.name == islandToSend + "_Harbor")
        {
            MinorIsland islandReceiver = GameObject.Find(islandToSend).GetComponent<MinorIsland>();
            this.collided = true;
            islandReceiver.displayPopup("Vous venez de recevoir une cargaison de " + this.quantityCarried.ToString() + " " + Resource.translateResourceName(resourceSent) + " !", 3);
            MinorIsland.exchangePerforming = false;
            //add resources to islandToSend
            TypeResource res = (TypeResource)System.Enum.Parse(typeof(TypeResource), resourceSent);
            islandReceiver.resourceManager.changeResourceStock(res, this.quantityCarried);
            this.Client.sendData("@2" + islandReceiver.nameMinorIsland.Split('_')[2] + "355@" + islandReceiver.resourceManager.getResource(res).TypeResource.ToString() + "@" + this.quantityCarried.ToString());
            StartCoroutine(startBoatDisappearance());
        }
        else
        {
            if (this.quantityCarried / 2 == 0)
            {
                this.collided = true;
                island.displayPopup("Suite aux dommages subis, votre bateau coule, ainsi que toutes les ressources transportées ...", 3);
                MinorIsland.exchangePerforming = false;
                //SINK ANIMATION
                StartCoroutine(startBoatDisappearance());
				StartCoroutine(SinkingCargo());
            }
            else
            {
                resetting = true;
                StartCoroutine(resetPosition());
                if (col.name.Contains("Harbor"))
                    island.displayPopup("Ce n'est pas le bon port ! Vous perdez donc la moitié des ressources à transmettre.", 3);
                else
                    island.displayPopup("Attention ! Vous venez de subir une collision, vous perdez donc la moitié des ressources à transmettre.", 3);
                this.quantityCarried /= 2;
            }
        }
    }

	IEnumerator SinkingCargo()
	{
		Instantiate (sinkingTrail, transform.position, Quaternion.identity);
		GetComponent<Animator> ().SetInteger ("animCargo", 1);
		yield return new WaitForSeconds (1f);
		Destroy (gameObject);
		yield return new WaitForSeconds (1f);
		Destroy (sinkingTrail);
	}

    public IEnumerator resetPosition()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.00005f);
            color = this.GetComponent<SpriteRenderer>().color;
            color.a -= 0.01f;
            this.GetComponent<SpriteRenderer>().color = color;
        }
        this.transform.position = startPosition;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.00005f);
            color = this.GetComponent<SpriteRenderer>().color;
            color.a += 0.01f;
            this.GetComponent<SpriteRenderer>().color = color;
        }
        this.GetComponent<BoxCollider>().enabled = true;
        resetting = false;
    }


    void Start()
    {
		GetComponent<Animator>().SetInteger("animCargo", 0);
        this.island = this.transform.parent.GetComponent<MinorIsland>();
        this.startPosition = this.transform.position;
        this.harbor = GameObject.Find(this.islandToSend + "_Harbor");
        StartCoroutine(startBoatAppearance());
        SpriteRenderer cyclonePrefab = Resources.Load<SpriteRenderer>("Prefab/cyclone");
        SpriteRenderer cyclone = Instantiate(cyclonePrefab);
        cyclone.name = "cyclone";
        StartCoroutine(spine(cyclone));
    }

    public IEnumerator startBoatAppearance()
    {
        Color color;
        color = this.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        this.GetComponent<SpriteRenderer>().color = color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.00005f);
            color = this.GetComponent<SpriteRenderer>().color;
            color.a += 0.01f;
            this.GetComponent<SpriteRenderer>().color = color;
        }

        this.appeared = true;
    }

    public IEnumerator startBoatDisappearance()
    {

        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.00005f);
            color = this.GetComponent<SpriteRenderer>().color;
            color.a -= 0.01f;
            this.GetComponent<SpriteRenderer>().color = color;
        }
        Destroy(GameObject.Find("cyclone"));
        Destroy(this.gameObject);
    }

    public IEnumerator spine(SpriteRenderer cyclone)
    {
        while(!this.collided)
        {
            yield return new WaitForSeconds(0.001f);
            cyclone.transform.Rotate(Vector3.back, 2);
        }
    }


    void FixedUpdate()
    {
        x1 = harbor.GetComponent<BoxCollider>().bounds.center.x;
        y1 = harbor.GetComponent<BoxCollider>().bounds.center.y;

        float alpha = 90 - (Mathf.Rad2Deg * Mathf.Atan2(y1 - transform.position.y, x1 - transform.position.x));
        transform.rotation = Quaternion.Euler(0f, 0f, -alpha);
        Vector3 v = transform.position;
        v.z = -3;
        transform.position = v;

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
            gesture.TouchMoved += touchMovedhandler;
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
        }
    }

    private void touchMovedhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        var touch = metaGestureEventArgs.Touch;
        if (this.appeared && !this.collided && !this.resetting)
        {
            Vector3 positionTouched = Camera.main.ScreenToWorldPoint(touch.Position);
            positionTouched.z = 0;
            this.transform.position = positionTouched;
        }
     }
    

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        TouchTime = 0;
    }
}
