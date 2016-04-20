using UnityEngine;
using System.Collections;
using System;

public class Resource : ScriptableObject {

    public TypeResource TypeResource { get; private set; }
    public int Stock { get; private set; }
    public int Production { get; private set; }

    public void init(TypeResource TypeResource)
    {
        this.TypeResource = TypeResource;
        this.Stock = 0;
        this.Production = 0;
    }
    public void init(TypeResource TypeResource, int quantity)
    {
        init(TypeResource);
        if (quantity > 0)
        {
            this.Stock = (int)quantity;
        }
        this.Production = 0;
    }
    public void init(TypeResource TypeResource, int quantity, int production)
    {
        init(TypeResource, quantity);
        if(production > 0)
        {
            this.Production = (int)production;
        }
    }
    public bool changeStock(int value)
    {
        //Debug.Log("Changing stock : " + this.TypeResource.ToString() + " stock " + this.Stock.ToString() + "production " + this.Production.ToString() + " value arg " + value.ToString() );
        if (this.Stock + value >= 0)
        {
            this.Stock += (int)value;
            return true;
        }
        else
        {
            this.Stock = 0;
            return false;
        }
    }
    public bool changeProduction(int valueToAdd)
    {
        if (this.Production + valueToAdd >= 0)
        {
            this.Production += (int)valueToAdd;
            return true;
        }
        else
        {
            this.Production = 0;
            return false;
        }
    }
}
