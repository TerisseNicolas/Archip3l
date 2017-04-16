using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loading : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(music());
    }

    IEnumerator music()
    {
        yield return new WaitForSeconds(2f);
        for(;;)
        {
            SoundPlayer.Instance.playLoadingSound();
            yield return new WaitForSeconds(63f);
        }
    }
}
