using UnityEngine;
using System.Collections;
using System;

public class Resource : ScriptableObject {

    public TypeResource TypeResource { get; private set; }
    public float Stock { get; private set; }
    public float Production { get; private set; }

    public void init(TypeResource TypeResource)
    {
        this.TypeResource = TypeResource;
        this.Stock = 0;
        this.Production = 0;
    }
    public void init(TypeResource TypeResource, float quantity)
    {
        init(TypeResource);
        if (quantity > 0)
        {
            this.Stock = quantity;
        }
        this.Production = 0;
    }
    public void init(TypeResource TypeResource, float quantity, float production)
    {
        init(TypeResource, quantity);
        if(production > 0)
        {
            this.Production = production;
        }
    }
    public bool changeStock(float value)
    {
        Debug.Log("Changing stock : " + this.TypeResource.ToString() + " stock " + this.Stock.ToString() + "production " + this.Production.ToString() + " value arg " + value.ToString() );
        //Stat resource
        if (Enum.IsDefined(typeof(TypeStat), this.TypeResource.ToString()))
        {
            float estimated = this.Stock + value * 0.01f;
            if (estimated >= 0 && estimated <= 1)
            {
                this.Stock = estimated;
                return true;
            }
            else
            {
                estimated = this.Stock + value * 0.0001f;
                if (estimated >= 0 && estimated <= 1)
                {
                    this.Stock = estimated;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Concrete resource
        if (this.Stock + value >= 0)
        {
            this.Stock += value;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool changeProduction(float valueToAdd)
    {
        //Stat resource
        if (Enum.IsDefined(typeof(TypeStat), this.TypeResource))
        {
            if (this.Production + valueToAdd >= 0)
            {
                this.Production += valueToAdd / 1000;
                return true;
            }
            else
            {
                return false;
            }
        }
        //Concrete resource
        if (this.Production + valueToAdd >= 0)
        {
            this.Production += valueToAdd;
            return true;
        }
        else
        {
            return false;
        }
    }
}
