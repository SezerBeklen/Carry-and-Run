using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollisionWithGround : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            StartCoroutine(WaithPickup());
        }
        
    }

    IEnumerator WaithPickup()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        gameObject.tag = "coin";
    }
}
