using UnityEngine;
using System.Collections;

public class Anim_BuildingConstruction : MonoBehaviour
{
    private Quaternion startQuaternion;
    private float interval;

    void Start()
    {
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,44);
        this.startQuaternion = transform.GetChild(0).rotation;
        this.interval = 0.02f;

        StartCoroutine("Rotate");
    }
    IEnumerator Rotate()
    {
        for (;;)
        {
            SoundPlayer.Instance.playConstructionSound();
            for (int i = 0; i < 23; i++)
            {
                RotateRight();
                yield return new WaitForSeconds(this.interval);
            }
            RotateInit();
            yield return new WaitForSeconds(this.interval);
        }

    }
    void RotateRight()
    {
        transform.GetChild(0).transform.Rotate(Vector3.forward * (-2));
    }
    void RotateInit()
    {
        transform.GetChild(0).transform.rotation = this.startQuaternion;
    }
}
