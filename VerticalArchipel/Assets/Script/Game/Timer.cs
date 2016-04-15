using UnityEngine;
using System.Collections;
using System;

public class Timer : MonoBehaviour
{

    public bool Running { get; private set; }
    public float InitTime {get; private set;}
    public float TimeLeft {get; private set;}
    public event EventHandler<EventArgs> FinalTick;

    public event EventHandler<EventArgs> PirateBoatsStartTick;
    public event EventHandler<EventArgs> PirateBoatsIncreaseTick;
    //Todo keep good values
    private float startPirateBoats = 30f; //7 * 60f;
    private float increasePirateBoats = 60f; //10 * 60f;

    public void Init(float seconds)
    {
        this.InitTime = seconds;
        this.TimeLeft = this.InitTime;
        this.Running = false;
    }

    public void StartTimer()
    {
        this.Running = true;
    }
    public void StopTimer()
    {
        this.Running = false;
        if(this.FinalTick != null)
        {
            FinalTick(this, new EventArgs());
        }
    }

    void Update()
    {
        if(this.Running)
        {
            this.TimeLeft -= Time.deltaTime;
            if (this.TimeLeft < 0)
            {
                this.StopTimer();
            }
            if(this.TimeLeft == this.startPirateBoats && this.PirateBoatsStartTick != null)
            {
                this.PirateBoatsStartTick(this, null);
            }
            if(this.TimeLeft == this.increasePirateBoats && this.PirateBoatsIncreaseTick != null)
            {
                this.PirateBoatsIncreaseTick(this, null);
            }
        }
    }
    public int getMinutes()
    {
        return (int) this.TimeLeft / 60;
    }
    public int getSeconds()
    {
        return (int)this.TimeLeft % 60;
    }
    public override string ToString()
    {
        return String.Format("{0:00}:{1:00}", this.getMinutes(), this.getSeconds());
    }
    public string ToStringSecondOnly()
    {
        return String.Format("{0:00}", this.TimeLeft);
    }
}