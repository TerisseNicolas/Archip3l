using UnityEngine;
using System.Collections;

public class SurvivorScript : MonoBehaviour
{
	void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
