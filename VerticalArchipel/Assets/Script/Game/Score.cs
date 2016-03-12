﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class Score : MonoBehaviour {

    private Client Client;
    public int ScoreCount;

    private string filePath;
    private List<Tuple<int, string, int>> scores;
    //private int size;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageScoreUpdateEvent += Client_MessageScoreUpdateEvent;

        this.ScoreCount = 0;
        this.filePath = "scores.txt"; // C:/tempConcours/scores.txt";

        this.scores = new List<Tuple<int, string, int>>();
        //this.size = 0;
        loadPreviousScores();
    }

    private void Client_MessageScoreUpdateEvent(object sender, MessageEventArgs e)
    {
        int scoreToAdd = Int32.Parse((string)e.message.Split('@').GetValue(2));
        if(scoreToAdd > 0)
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
                scores.Add(new Tuple<int, string, int>(Int32.Parse(words[0]), words[1], Int32.Parse(words[2])));
                //this.size++;
            }
            file.Close();
        }
        else
        {
            StreamWriter file = new StreamWriter(this.filePath);
            file.Close();
        }
    }
    public List<Tuple<int, string, int>> getBestScores(int limit)
    {
        List<Tuple<int, string, int>> retour = new List<Tuple<int, string, int>>();
        int temp = limit;
        foreach (Tuple<int, string, int> item in this.scores)
        {
            if (temp > 0)
            {
                retour.Add(item);
                temp--;
            }
        }
        return retour;
    }
    public List<Tuple<int, string, int>> getFinalResult(string teamName)
    {
        int count = 0;
        int limit = 10;
        bool flag = false;
        List<Tuple<int, string, int>> final = new List<Tuple<int, string, int>>();
        foreach (Tuple<int, string, int> item in this.scores)
        {
            if (count == limit)
            {
                break;
            }
            if (item.Second == teamName)
            {
                flag = true;
            }
            if ((count == limit - 1) && !flag)
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
        List<Tuple<int, string, int>> temp = new List<Tuple<int, string, int>>();
        foreach (Tuple<int, string, int> item in this.scores)
        {
            if ((item.Third <= this.ScoreCount) && !flag)
            {
                flag = true;
                temp.Add(new Tuple<int, string, int>(count, teamName, this.ScoreCount));
                count += 1;
                temp.Add(new Tuple<int, string, int>(count, item.Second, item.Third));
            }
            else
            {
                temp.Add(new Tuple<int, string, int>(count, item.Second, item.Third));
            }
            count += 1;
        }
        if (!flag)
        {
            temp.Add(new Tuple<int, string, int>(count, teamName, this.ScoreCount));
        }
        this.scores = temp;
        this.saveScores();
    }
    public void saveScores()
    {
        StreamWriter file = new StreamWriter(this.filePath, false);
        string line;
        foreach (Tuple<int, string, int> item in this.scores)
        {
            line = item.First.ToString() + "@" + item.Second + "@" + item.Third.ToString();
            file.WriteLine(line);
        }
        file.Close();
    }
}
