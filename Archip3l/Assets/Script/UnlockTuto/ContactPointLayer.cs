﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ContactPointLayer : MonoBehaviour
{
    private int NumberOfContact;
    private int ActualContactActivated;
    private bool Completed;
    private bool BoardCompleted;
    private bool done;

    private Client Client;

    private List<ContactPoint> ListContactPoint;

    void Awake()
    {
        //TODO set to 8
        this.NumberOfContact = 1;
        this.ActualContactActivated = 0;
        this.Completed = false;
        this.BoardCompleted = false;
        this.done = false;

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageUnlockTutoEvent += Client_MessageUnlockTutoEvent;
        this.ListContactPoint = new List<ContactPoint>();

        for(int i = 1; i <= NumberOfContact; i++)
        {
            this.ListContactPoint.Add(GameObject.Find("ContactPoint_" + i.ToString()).GetComponent<ContactPoint>());
            this.ListContactPoint[i - 1].ContactTouched += ContactPoint_ContactTouched;
            this.ListContactPoint[i - 1].ContactReleased += ContactPoint_ContactReleased;
        }
    }

    void Update()
    {
        if(this.BoardCompleted && this.Completed && !this.done)
        {
            this.done = true;
            StartCoroutine(holdAndChangeScene());
        }
    }

    private void Client_MessageUnlockTutoEvent(object sender, MessageEventArgs e)
    {
        this.BoardCompleted = true;
    }

    private void ContactPoint_ContactReleased(object sender, System.EventArgs e)
    {
        this.ActualContactActivated -= 1;
    }

    private void ContactPoint_ContactTouched(object sender, System.EventArgs e)
    {
        this.ActualContactActivated += 1;
        if((this.ActualContactActivated == this.NumberOfContact) && !this.Completed)
        {
            this.Completed = true;
            GameObject.Find("WonLayer").GetComponent<FinalFireWork>().StartFire();
        }
    }

    IEnumerator holdAndChangeScene()
    {
        yield return new WaitForSeconds(5f);
        SceneSupervisor.Instance.loadBriefingScenes();
    }
}