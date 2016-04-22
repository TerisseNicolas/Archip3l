using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class Score : MonoBehaviour {

    private Client Client;
    public int ScoreCount;
    public int BuildingCount;
    public int MedalCount;

    public bool allBuildingBuilt;
    public int numberOfDifferentBuildingBuilt;
    private List<string> buildingBuilt;

    private TrophyManager TrophyManager;

    public List<float> ChallengeSuccessRate;

    /* File structure
        -position
        -team name
        -score
        -medals
        -building
        -challengeSuccessRate
    */

    private string filePath;
    private List<Tuple<int, string, int, int, int, float>> scores;

    void Awake()
    {
        this.buildingBuilt = new List<string>();
        this.numberOfDifferentBuildingBuilt = 0;
    }

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageScoreUpdateEvent += Client_MessageScoreUpdateEvent;
        this.Client.MessageBuildingConstructionEvent += Client_MessageBuildingConstructionEvent;
        this.Client.MessageBuildingDestructionEvent += Client_MessageBuildingDestructionEvent;
        //this.Client.MessageTrophyWonEvent += Client_MessageTrophyWonEvent;
        this.Client.MessageChallengeFinalSuccessRateEvent += Client_MessageChallengeFinalSuccessRateEvent;

        this.TrophyManager = GameObject.Find("Trophies").GetComponent<TrophyManager>();
        this.TrophyManager.TrophyObtained += TrophyManager_TrophyObtained;

        this.ScoreCount = 0;
        this.filePath = "scores.txt";

        this.scores = new List<Tuple<int, string, int, int, int, float>>();
        this.ChallengeSuccessRate = new List<float>();
        for(int i = 0; i<4; i++)
        {
            this.ChallengeSuccessRate.Add(0);
        }
        loadPreviousScores();

        this.BuildingCount = 0;
        this.MedalCount = 0;
    }

    private void Client_MessageChallengeFinalSuccessRateEvent(object sender, MessageEventArgs e)
    {
        int islandNumber = Int32.Parse(e.message.Split('@')[1][1].ToString());
        float rate = float.Parse((string)e.message.Split('@').GetValue(2));
        this.ChallengeSuccessRate[islandNumber - 1] = rate;
    }

    private void Client_MessageScoreUpdateEvent(object sender, MessageEventArgs e)
    {
        int scoreToAdd = Int32.Parse((string)e.message.Split('@').GetValue(2));
        if (scoreToAdd > 0)
        {
            this.ScoreCount += scoreToAdd;
        }
    }
    public void loadPreviousScores()
    {
        string line;
        if (File.Exists(this.filePath))
        {
            StreamReader file = new StreamReader(this.filePath);
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split('@');
                scores.Add(new Tuple<int, string, int, int, int, float>(Int32.Parse(words[0]), words[1], Int32.Parse(words[2]),
                                                                      Int32.Parse(words[3]), Int32.Parse(words[4]), float.Parse(words[5])));
            }
            file.Close();
        }
        else
        {
            StreamWriter file = new StreamWriter(this.filePath);
            file.Close();
        }
    }
    public List<Tuple<int, string, int, int, int, float>> getBestScores(int limit)
    {
        List<Tuple<int, string, int, int, int, float>> retour = new List<Tuple<int, string, int, int, int, float>>();
        int temp = limit;
        foreach (Tuple<int, string, int, int, int, float> item in this.scores)
        {
            if (temp > 0)
            {
                retour.Add(item);
                temp--;
            }
        }
        return retour;
    }
    public List<Tuple<int, string, int, int, int, float>> getFinalResult(string teamName)
    {
        int count = 0;
        int limit = 10;
        bool flag = false;
        List<Tuple<int, string, int, int, int, float>> final = new List<Tuple<int, string, int, int, int, float>>();
        foreach (Tuple<int, string, int, int, int, float> item in this.scores)
        {
            if (count == limit)
            {
                break;
            }
            if (item.Second == teamName)
            {
                flag = true;
            }
            if ((count == limit - 1) && !flag && teamName != string.Empty)
            {
                continue;
            }
            else
            {
                final.Add(item);
                count += 1;
            }
        }
        return final;
    }
    public void addScore(string teamName)
    {
        Debug.Log("Adding score : " + teamName + " = " + this.ScoreCount);
        bool flag = false;
        int count = 1;

        float challengeSuccessRate = 0;
        foreach(float c in this.ChallengeSuccessRate)
        {
            challengeSuccessRate += c;
        }
        challengeSuccessRate /= 4.0f;
        challengeSuccessRate *= 100;

        List<Tuple<int, string, int, int, int, float>> temp = new List<Tuple<int, string, int, int, int, float>>();
        foreach (Tuple<int, string, int, int, int, float> item in this.scores)
        {
            if ((item.Third <= this.ScoreCount) && !flag)
            {
                flag = true;
                temp.Add(new Tuple<int, string, int, int, int, float>(count, teamName, this.ScoreCount, this.MedalCount, this.BuildingCount, challengeSuccessRate));
                count += 1;
                temp.Add(new Tuple<int, string, int, int, int, float>(count, item.Second, item.Third, item.Fourth, item.Fifth, item.Sixth));
            }
            else
            {
                temp.Add(new Tuple<int, string, int, int, int, float>(count, item.Second, item.Third, item.Fourth, item.Fifth, item.Sixth));
            }
            count += 1;
        }
        if (!flag)
        {
            temp.Add(new Tuple<int, string, int, int, int, float>(count, teamName, this.ScoreCount, this.MedalCount, this.BuildingCount, challengeSuccessRate));
        }
        this.scores = temp;
        this.saveScores();
    }
    public void saveScores()
    {
        StreamWriter file = new StreamWriter(this.filePath, false);
        string line;
        foreach (Tuple<int, string, int, int, int, float> item in this.scores)
        {
            line = item.First.ToString() + "@" + item.Second + "@" + item.Third.ToString() + "@" + item.Fourth.ToString() + "@" + item.Fifth.ToString() + "@" + item.Sixth.ToString("F2");
            file.WriteLine(line);
        }
        file.Close();
    }

    //private void Client_MessageTrophyWonEvent(object sender, MessageEventArgs e)
    //{
    //    //Debug.Log("New Medal won");
    //    this.MedalCount += 1;
    //}
    private void TrophyManager_TrophyObtained(object sender, TrophyObtainedEventArgs e)
    {
        //Debug.Log("New Medal won : " + e.Trophy.ToString());
        this.MedalCount += 1;
    }
    private void Client_MessageBuildingConstructionEvent(object sender, MessageEventArgs e)
    {
        //Debug.Log("New building built");
        this.BuildingCount += 1;

        //For medals
        if(this.buildingBuilt.Contains((string)e.message.Split('@').GetValue(2)) == false)
        {
            Debug.Log((string)e.message.Split('@').GetValue(2));
            this.buildingBuilt.Add((string)e.message.Split('@').GetValue(2));
            if(this.buildingBuilt.Count == 15)
            {
                this.allBuildingBuilt = true;
            }
        }
        this.numberOfDifferentBuildingBuilt = this.buildingBuilt.Count;
    }
    private void Client_MessageBuildingDestructionEvent(object sender, MessageEventArgs e)
    {
        if(this.BuildingCount > 0)
        {
            this.BuildingCount -= 1;
        }
    }
}
