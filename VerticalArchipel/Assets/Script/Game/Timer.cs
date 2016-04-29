using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Timer : MonoBehaviour
{

    public bool Running { get; private set; }
    public float InitTime {get; private set;}
    public float TimeLeft {get; private set;}
    public event EventHandler<EventArgs> FinalTick;

    public event EventHandler<EventArgs> PirateBoatsStartTick;
    public event EventHandler<EventArgs> PirateBoatsIncreaseTick;


    private List<string> constants;


    private float startPirateBoats = 7 * 60f + 35f;

    private bool launched = false;
    private float increasePirateBoats = 3 * 60f;
    private bool increased = false;

    public void Start()
    {
        loadConstants();
    }

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

    public void loadConstants()
    {
        constants = new List<string>();
        constants.Add("startPirateBoats.txt");
        constants.Add("increasePirateBoats.txt");

        foreach (string filePath in constants)
        {
            string line;
            if (File.Exists(filePath))
            {
                StreamReader file = new StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    switch (filePath.Split('.')[0])
                    {
                        case "startPirateBoats":
                            startPirateBoats = float.Parse(line);
                            break;
                        case "raisingRate":
                            increasePirateBoats = float.Parse(line);
                            break;
                    }
                }
                file.Close();
            }
            else
            {
                StreamWriter file = new StreamWriter(filePath);
                file.Close();
            }
        }
    }

    void Update()
    {
        if(this.Running)
        {
            this.TimeLeft -= Time.deltaTime;
            if (this.TimeLeft <= 0)
            {
                this.StopTimer();
            }
            if(!launched && this.TimeLeft-0.1 <= this.startPirateBoats && this.TimeLeft + 0.1 >= this.startPirateBoats && this.PirateBoatsStartTick != null)
            {
                launched = true;
                this.PirateBoatsStartTick(this, null);
            }
            if(!increased && this.TimeLeft - 0.1 <= this.increasePirateBoats && this.TimeLeft + 0.1 >= this.increasePirateBoats && this.PirateBoatsIncreaseTick != null)
            {
                increased = true;
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
