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

    void Awake()
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
            this.EnterContactZone();
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
        if (Time.time - TouchTime < 300)
        {
            this.ExitContactZone();
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
