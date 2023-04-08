using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    public GameObject CoinPrefab;
    public static CoinSpawn _Spawn›nstance;

    private void Awake()
    {
        if (_Spawn›nstance == null)
        {
            _Spawn›nstance = this;
        }

    }

  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.transform.childCount < 2)
            {
                StartCoroutine(SpawnDelay());
            }
                

        }

        if (collision.gameObject.CompareTag("RedPlayer"))
        {
            if (gameObject.transform.childCount < 2)
            {
                StartCoroutine(SpawnDelay());
            }
                

        }
        
        if (collision.gameObject.CompareTag("BluePlayer"))
        {
            if (gameObject.transform.childCount < 2)
            {
                StartCoroutine(SpawnDelay());
            }
                

        }
    }


    public void CoinSpawner()
    {
        Vector3 childPos = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z);
        Instantiate(CoinPrefab, childPos, Quaternion.identity,transform);

    }

    IEnumerator SpawnDelay()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(2);
        CoinSpawner();
        gameObject.GetComponent<SphereCollider>().enabled = true;
        
        
        
    }
}
