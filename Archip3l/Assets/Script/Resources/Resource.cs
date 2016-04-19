using UnityEngine;
using System.Collections;
using System;

public class Resource : ScriptableObject{

    public TypeResource TypeResource { get; private set; }
    public float Stock { get; private set; }
    public float Production { get; private set; }
    public float ProductionInit { get; private set; }

    public void init(TypeResource TypeResource)
    {
        this.TypeResource = TypeResource;
        this.Stock = 0;
        this.Production = 0;
        this.ProductionInit = 0;
    }
    public void init(TypeResource TypeResource, float quantity)
    {
        init(TypeResource);
        if (quantity > 0)
        {
            this.Stock = quantity;
        }
    }
    public void init(TypeResource TypeResource, float quantity, int production)
    {
        init(TypeResource, quantity);
        this.Production = production;
        this.ProductionInit = production;
    }

    public bool changeProduction(float valueToAdd)
    {
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
    public bool changeStock(float value)
    {
        if (value <= 0)
            Debug.Log("stock decremented");
        if (this.Stock + value >= 0)
        {
            this.Stock += value;
            return true;
        }
        else
        {
            Debug.Log("changeStocks returns false");
            return false;
        }
    }
    public bool checkChangeProductionPossibility(int value)
    {
        return this.Production + value >= 0;
    }
    public bool checkChangeStockPossibility(int value)
    {
        return this.Stock + value >= 0;
    }

    //translation of the resource's name to french
    static public string translateResourceName(string resourceName)
    {
        switch (resourceName)
        {
            case "Gold":
                return "Or";
            case "Stone":
                return "Pierre";
            case "Oil":
                return "Pétrole";
            case "Wood":
                return "Bois";
            case "Manufacture":
                return "Manufacture";
            case "Electricity":
                return "Electricité";
            case "Food":
                return "Nourriture";
            case "Health":
                return "Santé";
            case "Tourism":
                return "Tourisme";
            case "Education":
                return "Education";
            case "Religion":
                return "Religion";
            case "Happiness":
                return "Bonheur";
            default:
                return string.Empty;
        }
    }

    //returns le resource corresponding to the icon's name
    static public string getResourceFromIconName(string iconName)
    {
        switch (iconName)
        {
            case "goldIcon":
                return "Gold";
            case "stoneIcon":
                return "Stone";
            case "oilIcon":
                return "Oil";
            case "woodIcon":
                return "Wood";
            case "manufactureIcon":
                return "Manufacture";
            case "electricityIcon":
                return "Electricity";
            case "foodIcon":
                return "Food";
            case "healthIcon":
                return "Health";
            case "tourismIcon":
                return "Tourism";
            case "educationIcon":
                return "Education";
            case "religionIcon":
                return "Religion";
            case "happinessIcon":
                return "Happiness";
            default:
                return string.Empty;
        }
    }
}
