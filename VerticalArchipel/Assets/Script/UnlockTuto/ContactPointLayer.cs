﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ContactPointLayer : MonoBehaviour
{
    private int NumberOfContact;
    private int ActualContactActivated;
    private bool Completed;

    private Client Client;

    private FinalFireWork finalFireWork;
    private List<ContactPoint> ListContactPoint;

    private ContactPoint ContactPoint1;

    void Awake()
    {
        this.NumberOfContact = 4;
        this.ActualContactActivated = 0;
        this.Completed = false;

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.finalFireWork = GameObject.Find("WonLayer").GetComponent<FinalFireWork>();
        this.ListContactPoint = new List<ContactPoint>();

        for(int i = 1; i<=NumberOfContact; i++)
        {
            this.ListContactPoint.Add(GameObject.Find("ContactPoint_" + i.ToString()).GetComponent<ContactPoint>());
            this.ListContactPoint[this.ListContactPoint.Count -1].ContactTouched += ContactPoint_ContactTouched;
            this.ListContactPoint[this.ListContactPoint.Count -1].ContactReleased += ContactPoint_ContactReleased;
        }
    }

    private void ContactPoint_ContactReleased(object sender, System.EventArgs e)
    {
        this.ActualContactActivated -= 1;
    }

    private void ContactPoint_ContactTouched(object sender, System.EventArgs e)
    {
        this.ActualContactActivated += 1;
        if (this.ActualContactActivated == this.NumberOfContact && !this.Completed)
        {
            this.finalFireWork.StartFire();
            this.Client.sendData("@30921");
        }
    }
}