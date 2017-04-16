using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalFireWork : MonoBehaviour
{
    private List<ParticleSystem> PSList;

    void Awake()
    {
        this.PSList = new List<ParticleSystem>();
        for (int i = 1; i < 5; i++)
        {
            this.PSList.Add(GameObject.Find("PS_" + i.ToString()).GetComponent<ParticleSystem>());
        }
    }

    void Destroy()
    {
        this.StopFire();
        for (int i = 1; i < 5; i++)
        {
            Destroy(GameObject.Find("PS_" + i.ToString()));
        }
    }

    public void StartFire()
    {
        foreach (ParticleSystem ps in this.PSList)
        {
            ps.Play(true);
        }
    }
    public void StopFire()
    {
        foreach (ParticleSystem ps in this.PSList)
        {
            ps.Stop(true);
        }
    }
}
