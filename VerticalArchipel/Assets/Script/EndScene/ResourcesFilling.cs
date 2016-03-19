using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourcesFilling : MonoBehaviour
{
    private GlobalResourceManager GRM;
    void Awake()
    {
        GameObject resourceObject = GameObject.Find("Resources");
        resourceObject.transform.position = new Vector3(0, 0, 5);
        this.GRM = resourceObject.GetComponent<GlobalResourceManager>();
    }
	void Start ()
    {
        //fill island stats
        for(int i = 1; i < 5; i++)
        {
            foreach (Resource resource in this.GRM.ResourceManagers[i-1].Resources)
            {
                GameObject.Find(resource.TypeResource.ToString().ToLower() + i.ToString()).GetComponent<Text>().text = resource.Stock.ToString();
            }
        }

        //fill total stats
        foreach (Resource resource in this.GRM.Resources)
        {
            GameObject.Find(resource.TypeResource.ToString().ToLower() + "5").GetComponent<Text>().text = resource.Stock.ToString();
        }

        //fill buildings
        GameObject.Find("buildingScoreValue").GetComponent<Text>().text = GameObject.Find("Game").GetComponent<Score>().BuildingCount.ToString();

        //fill Medals
        GameObject.Find("medalScoreValue").GetComponent<Text>().text = GameObject.Find("Game").GetComponent<Score>().MedalCount.ToString();

        //fill score
        GameObject.Find("totalScoreValue").GetComponent<Text>().text = GameObject.Find("Game").GetComponent<Score>().ScoreCount.ToString();
	}
}
