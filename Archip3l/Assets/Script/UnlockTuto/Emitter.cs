using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {

    void Awake()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }

}
