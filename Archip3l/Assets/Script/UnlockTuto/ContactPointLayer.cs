using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ContactPointLayer : MonoBehaviour
{
    private int NumberOfContact;
    private int ActualContactActivated;
    private bool Completed;
    private bool BoardCompleted;

    private Client Client;

    private List<ContactPoint> ListContactPoint;

    private ContactPoint ContactPoint1;

    void Awake()
    {
        //TODO update this value with the number of contact point
        this.NumberOfContact = 1;
        this.ActualContactActivated = 0;
        this.Completed = false;
        //TODO put the false to false
        this.BoardCompleted = false;

        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageUnlockTutoEvent += Client_MessageUnlockTutoEvent;
        this.ListContactPoint = new List<ContactPoint>();

        for(int i = 1; i<=NumberOfContact; i++)
        {
            this.ListContactPoint.Add(GameObject.Find("ContactPoint_" + i.ToString()).GetComponent<ContactPoint>());
            this.ListContactPoint[this.ListContactPoint.Count -1].ContactTouched += ContactPoint_ContactTouched;
            this.ListContactPoint[this.ListContactPoint.Count -1].ContactReleased += ContactPoint_ContactReleased;
        }
    }

    private void Client_MessageUnlockTutoEvent(object sender, MessageEventArgs e)
    {
        this.BoardCompleted = true;
        if(this.Completed)
        {
            StartCoroutine(holdAndChangeScene());
        }
    }

    private void ContactPoint_ContactReleased(object sender, System.EventArgs e)
    {
        this.ActualContactActivated -= 1;
    }

    private void ContactPoint_ContactTouched(object sender, System.EventArgs e)
    {
        this.ActualContactActivated += 1;
        if(this.ActualContactActivated == this.NumberOfContact && !this.Completed)
        {
            GameObject.Find("WonLayer").GetComponent<FinalFireWork>().StartFire();
            if(this.BoardCompleted)
            {
                StartCoroutine(holdAndChangeScene());
            }
        }
    }

    IEnumerator holdAndChangeScene()
    {
        yield return new WaitForSeconds(5f);
        SceneSupervisor.Instance.loadBriefingScenes();
    }
    //void Update()
    //{
    //    transform.Rotate(Vector3.down * Time.deltaTime*360);
    //}
}