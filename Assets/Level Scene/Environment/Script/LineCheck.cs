using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCheck : MonoBehaviour
{
    public static LineCheck Ýnstance_LineCheck;
    public bool CarCollisionCheck;

    private void Awake()
    {
        if (Ýnstance_LineCheck == null)
        {
            Ýnstance_LineCheck = this;
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
