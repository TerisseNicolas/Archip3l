using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
	void Start () {
        StartCoroutine(launchGame());
	}
	
    IEnumerator launchGame()
    {
        yield return new WaitForSeconds(0.1f);
        SceneSupervisor.Instance.loadLoadingScene();
    }
}
