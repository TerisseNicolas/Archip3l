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
            GameObject.Find(resource.TypeResource.ToString().ToLower() + "5").GetComponent<Text>().text = resource.Stock.ToString();
        }

        //fill challenge success rate
        Score score = GameObject.Find("Game").GetComponent<Score>();
        float totalSuccessRate = 0;
        float value = 0;
        for(int i = 0; i <score.ChallengeSuccessRate.Count; i++)
        {
            value = score.ChallengeSuccessRate[i] * 100;
            totalSuccessRate += value;
            GameObject.Find("goodAnswersValue" + (i + 1).ToString()).GetComponent<Text>().text = value.ToString("F2") + "%";
        }
        float verticalRate = 0;
        if (score.verticalAnswers == 0)
        {
            verticalRate = 1;
        }
        else
        {
            verticalRate = (score.verticalGoodAnwsers / score.verticalAnswers);
        }
        GameObject.Find("goodAnswersValue0").GetComponent<Text>().text = (verticalRate * 100).ToString("F2") + "%";
        totalSuccessRate += verticalRate * 100;
        GameObject.Find("goodAnswersValue5").GetComponent<Text>().text = (totalSuccessRate / 5).ToString("F2") +"%";

        //fill buildings

        GameObject.Find("buildingScoreValue").GetComponent<Text>().text = score.BuildingCount.ToString();

        //fill Medals
        GameObject.Find("medalScoreValue").GetComponent<Text>().text = score.MedalCount.ToString();

        //fill score
        GameObject.Find("totalScoreValue").GetComponent<Text>().text = score.ScoreCount.ToString();
	    SoundPlayer.Instance.playApplauseSound();
    }
}
