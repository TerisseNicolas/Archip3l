﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    public List<Resource> Resources;
    public event EventHandler<ChangeResourceStockEventArgs> ChangeResourceStockEvent;
    public event EventHandler<ChangeResourceProductionEventArgs> ChangeResourceProductionEvent;

    private Client client;

    private bool _MessageResourceProductionUpdateEvent = false;
    private MessageEventArgs _MessageResourceProductionUpdateEventEventArgs;

    private bool _MessageResourceStockUpdateEvent = false;
    private MessageEventArgs _MessageResourceStockUpdateEventEventArgs;

    void Start()
    {
        this.Resources = new List<Resource>();
        this.client = GameObject.Find("Network").GetComponent<Client>();
        this.client.MessageResourceStockUpdateEvent += Client_MessageResourceStockUpdateEvent;
        this.client.MessageResourceProductionUpdateEvent += Client_MessageResourceProductionUpdateEvent;

        //Add all resources
        foreach (TypeResource resourceType in Enum.GetValues(typeof(TypeResource)))
        {
            //To be modified to 0;
            this.addResource(resourceType, 0, 0);
        }

        //Todo delete this instruction and the function
        //StartCoroutine("test");
    }

    void Update()
    {
        if(_MessageResourceProductionUpdateEvent)
        {
            changeResourceProduction(null, this._MessageResourceProductionUpdateEventEventArgs, (TypeResource)Enum.Parse(typeof(TypeResource), (string)this._MessageResourceProductionUpdateEventEventArgs.message.Split('@').GetValue(2)), Int32.Parse((string)this._MessageResourceProductionUpdateEventEventArgs.message.Split('@').GetValue(3)));
            this._MessageResourceProductionUpdateEvent = false;
        }
        if(_MessageResourceStockUpdateEvent)
        {
            changeResourceStock(null, this._MessageResourceStockUpdateEventEventArgs, (TypeResource)Enum.Parse(typeof(TypeResource), (string)this._MessageResourceStockUpdateEventEventArgs.message.Split('@').GetValue(2)), Int32.Parse((string)this._MessageResourceStockUpdateEventEventArgs.message.Split('@').GetValue(3)));
            this._MessageResourceStockUpdateEvent = false;
        }
    }

    private void Client_MessageResourceProductionUpdateEvent(object sender, MessageEventArgs e)
    {
        this._MessageResourceProductionUpdateEvent = true;
        this._MessageResourceProductionUpdateEventEventArgs = e;
    }

    private void Client_MessageResourceStockUpdateEvent(object sender, MessageEventArgs e)
    {
        this._MessageResourceStockUpdateEvent = true;
        this._MessageResourceStockUpdateEventEventArgs = e;
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(2);
        changeResourceStock(null, new MessageEventArgs { message = "serverinfo@22355@Gold@45" }, TypeResource.Gold, 45);
        yield return new WaitForSeconds(2);
        changeResourceStock(null, new MessageEventArgs { message = "serverinfo@23355@Tourism@2" }, TypeResource.Tourism, 2);
        changeResourceStock(null, new MessageEventArgs { message = "serverinfo@24355@Tourism@7" }, TypeResource.Tourism, 7);
        changeResourceStock(null, new MessageEventArgs { message = "serverinfo@23355@Tourism@15" }, TypeResource.Tourism, 120);
        yield return new WaitForSeconds(2);
        changeResourceStock(null, new MessageEventArgs { message = "serverinfo@22355@Gold@45" }, TypeResource.Gold, 45);
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
            res.init(resourceType, quantity, production);
            this.Resources.Add(res);
            return true;
        }
    }

    private bool changeResourceProduction(object sender, MessageEventArgs e, TypeResource resourceType, int value)
    {
        //int islandNumber = (Int32.Parse((string)e.message.Split('@').GetValue(1)) % 10000)/100;
        char islandNumber = ((string)e.message.Split('@').GetValue(1))[1];
        if (!gameObject.name.Contains(islandNumber))
        {
            //Island not concerned
            return false;
        }
        Resource resource = this.getResource(resourceType);
        bool result = false;
        if(resource!= null)
        {
            result = resource.changeProduction(value);
        }
        else
        {
            if (value >= 0)
            {
                this.addResource(resourceType, 0, value);
                result = true;
            }
            else
            {
                return false;
            }
        }
        return result;
    }
    public bool changeResourceStock(object sender, MessageEventArgs e, TypeResource resourceType, int value)
    {
        //Debug.Log("Changing resource stock : " + resourceType.ToString() + " - " + value.ToString());
        char islandNumber = ((string)e.message.Split('@').GetValue(1))[1];
        if (!gameObject.name.Contains(islandNumber))
        {
            //Island not concerned
            return false;
        }
        Debug.Log("Stock update : " + e.message);
        Resource resource = this.getResource(resourceType);
        bool result = false;
        if (resource != null)
        {
            result = resource.changeStock(value);
        }
        else
        {
            if (value >= 0)
            {
                this.addResource(resourceType, value);
                result = true;
            }
            else
            {
                return false;
            }
        }
        //The board shouldn't notice the network of an gloabl stock update
        if(this.ChangeResourceStockEvent != null && result)
        {
            this.ChangeResourceStockEvent(this, new ChangeResourceStockEventArgs { resourceType = resourceType, stock = value }); // resource.Stock });
        }
        return result;
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
    public void initResources()
    {
        //Sync all resource from table before the start of the game
        this.client.sendData("@30306");
    }
}

public class ChangeResourceStockEventArgs : EventArgs
{
    public TypeResource resourceType;
    public float stock;
}

public class ChangeResourceProductionEventArgs : EventArgs
{
    public TypeResource resourceType;
    public float production;
}