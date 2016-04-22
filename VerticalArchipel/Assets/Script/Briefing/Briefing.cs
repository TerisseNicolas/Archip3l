using UnityEngine;
using System.Collections;

public class Briefing : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        SoundPlayer.Instance.playBriefingLetterSound();
	}
}
