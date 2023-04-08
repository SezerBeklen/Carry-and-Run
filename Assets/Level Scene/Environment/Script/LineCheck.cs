using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCheck : MonoBehaviour
{
    public static LineCheck �nstance_LineCheck;
    public bool CarCollisionCheck;

    private void Awake()
    {
        if (�nstance_LineCheck == null)
        {
            �nstance_LineCheck = this;
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashingcar"))
        {
            CarCollisionCheck = true;

        }

    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("crashingcar"))
        {
            CarCollisionCheck = true;

        }
    }
}
