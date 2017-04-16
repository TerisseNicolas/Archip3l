using UnityEngine;
using System.Collections;

public class GlobalInfo : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    //Actually do nothing but can be really useful
}
