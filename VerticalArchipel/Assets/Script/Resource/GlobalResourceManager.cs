using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class GlobalResourceManager : MonoBehaviour
{
    public List<Resource> Resources;
    public List<ResourceManager> ResourceManagers;

    private float resourceCountForScoreInit;
    private float resourceCountForScoreCurrent;

    private Client Client;

    public int totalResourceCount = 0;

    public event EventHandler<EventArgs> MessageInitialized;

    void Start()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();

        this.Resources = new List<Resource>();

        this.ResourceManagers.Add(GameObject.Find("sous_ile_1").GetComponent<ResourceManager>());
        this.ResourceManagers.Add(GameObject.Find("sous_ile_2").GetComponent<ResourceManager>());
        this.ResourceManagers.Add(GameObject.Find("sous_ile_3").GetComponent<ResourceManager>());
        this.ResourceManagers.Add(GameObject.Find("sous_ile_4").GetComponent<ResourceManager>());

        this.resourceCountForScoreInit = 500;
        this.resourceCountForScoreCurrent = this.resourceCountForScoreInit;

        foreach(ResourceManager rm in this.ResourceManagers)
        {
            rm.ChangeResourceStockEvent += _ChangeResourceStockEvent;
            rm.ChangeResourceProductionEvent += _ChangeResourceProductionEvent;
        }

        //Add all resources
        foreach (TypeResource resourceType in Enum.GetValues(typeof(TypeResource)))
        {
            this.addResource(resourceType, 0);
        }
    }

    private void _ChangeResourceProductionEvent(object sender, ChangeResourceProductionEventArgs e)
    {
        Resource resource = this.getResource(e.resourceType);
        if (resource != null)
        {
            resource.changeProduction((int)e.production);
        }
        else
        {
            if (e.production >= 0)
            {
                this.addResource(e.resourceType, (int)e.production);
            }
        }
    }
    private void _ChangeResourceStockEvent(object sender, ChangeResourceStockEventArgs e)
    {
        //Debug.Log("Global resource manager : change resource stock event : " + e.resourceType.ToString() + " : " + e.stock.ToString());
        Resource resource = this.getResource(e.resourceType);
        if (resource != null)
        {
            this.resourceCountForScoreCurrent -= e.stock;
            if(this.resourceCountForScoreCurrent < 0)
            {
                this.resourceCountForScoreCurrent = this.resourceCountForScoreInit;
                this.Client.sendData("@30505@" + 50.ToString());
            }
            resource.changeStock((int)e.stock);
        }
        else
        {
            if (e.stock >= 0)
            {
                this.addResource(e.resourceType, (int)e.stock);
            }
            else
            {
                this.addResource(e.resourceType, -(int)e.stock);
            }
        }
        if (this.totalResourceCount + (int)e.stock >= 0)
        {
            this.totalResourceCount += (int)e.stock;
        }
    }
    public bool addResource(TypeResource resourceType, int quantity, int production = 0)
    {
        bool flag = false;
        foreach (Resource item in this.Resources)
        {
            if (item.TypeResource == resourceType)
            {
                flag = true;
            }
        }
        if (flag == true)
        {
            return false;
        }
        else
        {
            Resource res = ScriptableObject.CreateInstance<Resource>();
            res.init(resourceType, quantity);
            this.Resources.Add(res);
            return true;
        }
    }
    public Resource getResource(TypeResource resourceType)
    {
        foreach (Resource item in this.Resources)
        {
            if (item.TypeResource == resourceType)
            {
                return item;
            }
        }
        return null;
    }

    //This function is never call but it's ok
    public IEnumerator initResources()
    {
        //Sync all resource before the start of the game
        //sub resource manager
        this.Client.sendData("@30306");

        //foreach(ResourceManager rm in this.ResourceManagers)
        //{
        //    rm.initResources();
        //}

        //Wait all anwsers from the network to initialize island resource manager
        yield return new WaitForSeconds(5f);

        //Fill this one now
        foreach(ResourceManager rm in this.ResourceManagers)
        {
            foreach (Resource resource in rm.Resources)
            {
                Resource resourceGlobalManager = getResource(resource.TypeResource);
                _ChangeResourceStockEvent(null, new ChangeResourceStockEventArgs { resourceType = resourceGlobalManager.TypeResource, stock = resource.Stock / 4 });
                _ChangeResourceProductionEvent(null, new ChangeResourceProductionEventArgs { resourceType = resourceGlobalManager.TypeResource, production = resource.Production / 4 });
            }
        }
        if(this.MessageInitialized != null)
        {
            this.MessageInitialized(this, new EventArgs());
        }
    }
}