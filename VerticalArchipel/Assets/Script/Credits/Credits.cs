using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        SoundPlayer.Instance.playBriefingLetterSound();
	}
}
