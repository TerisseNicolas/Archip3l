﻿using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    Timer Timer;

    void Start()
    {
        GameObject game = GameObject.Find("Game");
        this.Timer = game.GetComponent<Timer>();
    }
	void Update ()
    {
        GetComponent<Text>().text = this.Timer.ToString();
    }
}
