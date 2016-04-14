using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ResourceView : MonoBehaviour {

    private GlobalResourceManager resourceManager;

    void Start()
    {
        this.resourceManager = gameObject.GetComponent<GlobalResourceManager>();
    }
    void FixedUpdate()
    {
        foreach(Resource res in this.resourceManager.Resources)
        {
            //Debug.Log()
            //GameObject.Find(res.TypeResource.ToString()).GetComponent<RectTransform>().transform.FindChild("Value").gameObject.GetComponent<Text>().text = res.Stock.ToString();
            foreach(Text t in GameObject.Find(res.TypeResource.ToString()).GetComponentsInChildren<Text>())
            {
                if (Enum.IsDefined(typeof(TypeStat), res.TypeResource.ToString()))
                {
                    if (t.name == "Value")
                        t.text = res.Stock.ToString() + " %";
                }
                else
                {
                    if (t.name == "Value")
                        t.text = res.Stock.ToString();
                }
            }
        }
    }
}
