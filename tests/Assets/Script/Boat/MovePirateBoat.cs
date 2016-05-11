using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System.Collections.Generic;
using TouchScript;
using TouchScript.InputSources;
using TouchScript.Hit;

public class MovePirateBoat : InputSource
{
    public Vector2 speed = new Vector2(0.2f, 0.2f);
    public Vector2 direction = new Vector2(0, 1);
    private Vector3 movement;

	private bool sinking;
	private float x, y;

    private float lifeTime;
    public ParticleSystem explosionEffect;
//    public ParticleSystem sinkEffect;
	public GameObject sinkingTrail;

    private Client Client;

    void Awake()
    {
        this.lifeTime = 20f;
        Destroy(gameObject, this.lifeTime);
        gameObject.SetActive(false);
		sinking = false;
        this.Client = GameObject.Find("Network").GetComponent<Client>();
    }

	void Start()
	{
		GetComponent<Animator> ().SetInteger ("animBoat", 0);
	}

    public void init(Vector3 initPosition, Vector3 targetPosition)
    {
        System.Random rnd = new System.Random();

        this.direction = targetPosition - initPosition;

        transform.position = initPosition;
        movement = initPosition;

        float alpha = -90 + (Mathf.Rad2Deg * Mathf.Atan2(targetPosition.y - initPosition.y, targetPosition.x - initPosition.x)); //, transform.position.y, targetPosition.x - transform.position.x));
        transform.rotation = Quaternion.Euler(0f, 0f, alpha);

        float boatSpeed = rnd.Next(3, 6) / 1000f;
		//float boatSpeed = 5 / 1000f;
        this.speed = new Vector2(boatSpeed, boatSpeed);

        float boatScale = 7 + rnd.Next(-1, 1);
        transform.localScale = new Vector3(boatScale, boatScale, 0);

        gameObject.SetActive(true);
    }

    void Update()
    {
        movement = new Vector3(transform.position.x + this.speed.x * direction.x, transform.position.y + this.speed.y * direction.y, -4);
        if (System.Math.Abs(this.transform.position.x) > 700 || System.Math.Abs(this.transform.position.y) > 500)
        {
            Destroy(gameObject);
        }

    }

    void FixedUpdate()
    {

		x = transform.position.x;
		y = transform.position.y;

		if (sinking) 
		{
			this.transform.position = new Vector3(x,y,transform.position.z);
		}
		else
			this.transform.position = movement;


    }
    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    private Vector3 getNewBoatPosition()
    {
        System.Random rnd = new System.Random();
        switch (rnd.Next(1, 5))
        {
            case 1:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(0, 400), 0);
            case 2:
                return new Vector3(rnd.Next(430, 500), rnd.Next(0, 400), 0);
            case 4:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(-400, 0), 0);
            case 3:
                return new Vector3(rnd.Next(430, 500), rnd.Next(-400, 0), 0);
        }
        return new Vector3(0, 0);
    }
    private Vector2 getNewBoatDirection()
    {
        Vector2 direction = new Vector2();

        return direction;
    }
    void OnMouseDownSimulation()
    {
        //Score to add must be checked
        this.Client.sendData("@30505@" + 10.ToString());
        this.destroyBoat(true);
    }
    
	IEnumerator ShipSinking()
	{
		sinking = true;
		this.GetComponent<BoxCollider> ().enabled = false;
		Instantiate (sinkingTrail, transform.position, Quaternion.identity);
		GetComponent<Animator> ().SetInteger ("animBoat", 1);
		GetComponent<Animator> ().Play ("sinking");
		yield return new WaitForSeconds (1.5f);
		Destroy (this.gameObject);
		yield return new WaitForSeconds (5f);
		Destroy (sinkingTrail);
		sinking = false;
	}


    public void destroyBoat(bool touched)
    {

        if(touched)
        {
            SoundPlayer.Instance.playSinkingPirateShipSound();
			StartCoroutine(ShipSinking());
        }
        else
        {
            SoundPlayer.Instance.playCrashPirateShipSound();
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name.Contains("ExchangeResourceAnimation"))
        {
            this.destroyBoat(true);
        }
        if(collider.name.Contains("PirateBoat"))
        {
            this.destroyBoat(false);
        }
        if (collider.name.Contains("Harbor"))
        {
            this.destroyBoat(false);
        }
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
        if (TouchTime == 0 && !MinorIsland.exchangePerforming)
        {
            TouchTime = Time.time;
        }

    }
    

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 0.5)
        {
            this.OnMouseDownSimulation();
        }
        TouchTime = 0;
    }
    
}