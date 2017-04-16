using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisturbanceTimerView : MonoBehaviour {

    DisturbanceTimer DisturbanceTimer;

    void Start()
    {
        GameObject game = GameObject.Find("Game");
        this.DisturbanceTimer = game.GetComponent<DisturbanceTimer>();
    }
    void Update()
    {
        GetComponent<Text>().text = this.DisturbanceTimer.ToString();
    }
}
