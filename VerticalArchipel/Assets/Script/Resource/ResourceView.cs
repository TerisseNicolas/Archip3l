using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour {

    private GlobalResourceManager resourceManager;

    void Awake()
    {
        this.resourceManager = gameObject.GetComponent<GlobalResourceManager>();
    }
    void FixedUpdate()
    {
        foreach(Resource res in this.resourceManager.Resources)
        {
            //Debug.Log()
            //GameObject.Find(res.TypeResource.ToString()).GetComponent<RectTransform>().transform.FindChild("Value").gameObject.GetComponent<Text>().text = res.Stock.ToString();
            foreach(Text t in GameObject.Find(res.TypeResource.ToString()).GetComponentsInChildren<Text>()){
                if (t.name == "Value"){
                    t.text = res.Stock.ToString();
                    Debug.Log("ok");
                } 

            }
        }
    }
}
