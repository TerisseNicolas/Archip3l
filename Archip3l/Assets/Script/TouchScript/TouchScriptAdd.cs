using UnityEngine;
using System.Collections;

public class TouchScriptAdd : MonoBehaviour {

	void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
