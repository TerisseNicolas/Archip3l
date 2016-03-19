using UnityEngine;
using System.Collections;

public class SurvivorScript : MonoBehaviour
{
	void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
