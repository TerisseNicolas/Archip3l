using UnityEngine;
using System.Collections;
using System;
using TouchScript.Gestures;
using System.Collections.Generic;
using TouchScript;
using TouchScript.InputSources;
using TouchScript.Hit;

public class ContactPoint : InputSource
{

    private ParticleSystem ParticleSystem;
    private ParticleSystem.EmissionModule ParticleSystemEmit;

    private ContactPointLayer ContactPointLayer;

    public delegate void ContactPointTouchedEvent(object sender, EventArgs e);
    public event ContactPointTouchedEvent ContactTouched;
    public event ContactPointTouchedEvent ContactReleased;

    private Sprite SpriteRed;
    private Sprite SpriteGreen;

    public bool touched = false;

    void Start()
    {
        this.ParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        this.ParticleSystemEmit = this.ParticleSystem.emission;
        this.ContactPointLayer = GameObject.Find("ContactPointLayer").GetComponent<ContactPointLayer>();

        this.SpriteRed = Resources.Load<Sprite>("unlock/boutonStart");
        this.SpriteGreen = Resources.Load<Sprite>("unlock/boutonStartClic");
    }

    void EnterContactZone()
    {
        if (!touched)
        {
            touched = true;
            GetComponent<SpriteRenderer>().sprite = this.SpriteGreen;
            if (!ParticleSystem.isPlaying)
            {
                ParticleSystem.Simulate(0.0f, true, true);
                this.ParticleSystemEmit.enabled = true;
            }
            ParticleSystem.Play(true);
            if (this.ContactTouched != null)
            {
                this.ContactTouched(this, null);
            }
        }
    }

    void ExitContactZone()
    {
        if (touched)
        {
            touched = false;
            GetComponent<SpriteRenderer>().sprite = this.SpriteRed;
            if (this.ParticleSystem.isPlaying)
            {
                this.ParticleSystem.Stop();
            }
            if (this.ContactReleased != null)
            {
                this.ContactReleased(this, null);
            }
        }
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
            this.EnterContactZone();
        }
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        this.ExitContactZone();
        TouchTime = 0;
    }
}
