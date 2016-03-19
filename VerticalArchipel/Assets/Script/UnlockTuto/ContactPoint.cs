using UnityEngine;
using System.Collections;
using System;

public class ContactPoint : MonoBehaviour {

    private ParticleSystem ParticleSystem;
    private ParticleSystem.EmissionModule ParticleSystemEmit;

    private ContactPointLayer ContactPointLayer;

    public delegate void ContactPointTouchedEvent(object sender, EventArgs e);
    public event ContactPointTouchedEvent ContactTouched;
    public event ContactPointTouchedEvent ContactReleased;

    private Sprite SpriteRed;
    private Sprite SpriteGreen;

    void Awake()
    {
        this.ParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        this.ParticleSystemEmit = this.ParticleSystem.emission;
        this.ContactPointLayer = GameObject.Find("ContactPointLayer").GetComponent<ContactPointLayer>();

        this.SpriteRed = Resources.Load<Sprite>("unlock/boutonStart");
        this.SpriteGreen = Resources.Load<Sprite>("unlock/boutonStartClic");
    }
    void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().sprite = this.SpriteGreen;
        if (!ParticleSystem.isPlaying)
        {
            ParticleSystem.Simulate(0.0f, true, true);
            this.ParticleSystemEmit.enabled = true;
        }
        ParticleSystem.Play(true);
        if(this.ContactTouched != null)
        {
            this.ContactTouched(this, null);
        }
    }
    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = this.SpriteRed;
        if (this.ParticleSystem.isPlaying)
        {
            this.ParticleSystem.Stop();
        }
        if(this.ContactReleased != null)
        {
            this.ContactReleased(this, null);
        }
    }
}
