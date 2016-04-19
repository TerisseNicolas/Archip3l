using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourcesFilling : MonoBehaviour
{
    private GlobalResourceManager GRM;

	void Start ()
    {
        GameObject resourceObject = GameObject.Find("Resources");
        //resourceObject.transform.position = new Vector3(0, 0, 12);
        resourceObject.GetComponent<Canvas>().enabled = false;
        this.GRM = resourceObject.GetComponent<GlobalResourceManager>();

        //fill island stats
        for (int i = 1; i < 5; i++)
        {
            foreach (Resource resource in this.GRM.ResourceManagers[i-1].Resources)
            {
                GameObject.Find(resource.TypeResource.ToString().ToLower() + i.ToString()).GetComponent<Text>().text = resource.Stock.ToString();
            }
        }

        //fill total stats
        foreach (Resource resource in this.GRM.Resources)
        {
            GameObject.Find(resource.TypeResource.ToString().ToLower() + "5").GetComponent<Text>().text = resource.Stock.ToString("F2");
        }

        //fill challenge success rate
        GameObject game = GameObject.Find("Game");
        float totalSuccessRate = 0;
        float value = 0;
        for(int i = 0; i <game.GetComponent<Score>().ChallengeSuccessRate.Count; i++)
        {
            value = game.GetComponent<Score>().ChallengeSuccessRate[i];
            totalSuccessRate += value;
            GameObject.Find("goodAnswersValue" + (i + 1).ToString()).GetComponent<Text>().text = value.ToString();
        }
        GameObject.Find("goodAnswersValue5").GetComponent<Text>().text = (totalSuccessRate / 4).ToString();

        //fill buildings

        GameObject.Find("buildingScoreValue").GetComponent<Text>().text = game.GetComponent<Score>().BuildingCount.ToString();

        //fill Medals
        GameObject.Find("medalScoreValue").GetComponent<Text>().text = game.GetComponent<Score>().MedalCount.ToString();

        //fill score
        GameObject.Find("totalScoreValue").GetComponent<Text>().text = game.GetComponent<Score>().ScoreCount.ToString();
	}
}
