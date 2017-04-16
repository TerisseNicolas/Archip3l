﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultFilling : MonoBehaviour {

    private string teamName;
    private string filePath;

    private List<Tuple<int, string, int, int, int, float>> scores;

    /* File structure
        -position
        -team name
        -score
        -medals
        -building
        -challengeSuccessRate
    */

    void Start()
    {
        this.filePath = "scores.txt";
        this.teamName = GameObject.Find("GlobalInfo").GetComponent<GlobalInfo>().teamName;
        this.scores = new List<Tuple<int, string, int, int, int, float>>();

        this.loadPreviousScores();

        //fill the tab
        int count = 1;
        foreach(Tuple<int, string, int, int, int, float> item in getFinalResult(this.teamName))
        {
            GameObject.Find(count.ToString()).GetComponent<Text>().text = item.First.ToString();
            //fill team name
            GameObject.Find("team" + count.ToString()).GetComponent<Text>().text = item.Second;
            //fill score
            GameObject.Find("score" + count.ToString()).GetComponent<Text>().text = item.Third.ToString();
            //fill medals
            GameObject.Find("medal" + count.ToString()).GetComponent<Text>().text = item.Fourth.ToString();
            //fill buildings
            GameObject.Find("building" + count.ToString()).GetComponent<Text>().text = item.Fifth.ToString();
            //fill challenge success rate
            GameObject.Find("challenge" + count.ToString()).GetComponent<Text>().text = item.Sixth.ToString("F2") + " %";


            count += 1;
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
}
