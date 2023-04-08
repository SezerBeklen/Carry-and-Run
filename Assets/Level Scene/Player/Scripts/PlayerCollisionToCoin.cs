using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCollisionToCoin : MonoBehaviour
{
    public int coinCounter;
    public static PlayerCollisionToCoin coinColl_›nstance;
    private float CoinYAxis;
    public bool ›sH›tCar;
    public float camShakeDuration, camShakeMagnitude;
    public ParticleSystem spark;
     
    private void Awake()
    {
        if (coinColl_›nstance == null)
        {
            coinColl_›nstance = this;
        }
    }
    private void Update()
    {
        if (›sH›tCar)
        {
            PlayerMovement.movement›nstance.speed = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            if (AImovement.instanceAI.fallingsCoins.Contains(other.gameObject))
            {
                AImovement.instanceAI.fallingsCoins.Remove(other.gameObject);
            }
            if (!StackCoin.instance.coins.Contains(other.gameObject))
            {

                if (coinCounter < 12)
                {
                    SoundManager.instance.CollectSound();
                    //CoinSpawn._Spawn›nstance.CoinSpawner();
                    if (PlayerMovement.movement›nstance.speed > 0)
                    {
                        PlayerMovement.movement›nstance.speed -= 0.2f;
                    }
                    other.gameObject.transform.parent = StackCoin.instance.Player_Carrier.transform;
                    other.GetComponent<BoxCollider>().enabled = false;
                    other.gameObject.tag = "Untagged";
                    other.gameObject.layer = LayerMask.NameToLayer("Default");
                    coinCounter += 1;

                    Vector3 playerPosOffset = new Vector3(transform.position.x, CoinYAxis, transform.position.z);
                    other.gameObject.transform.DOMove(playerPosOffset, 0.2f).OnComplete(() =>
                    {StackCoin.instance.StackCoins(other.gameObject, StackCoin.instance.coins.Count - 1);});

                    Vector3 coinRotate = new Vector3(0, 0, 90);
                    other.gameObject.transform.DOLocalRotate(coinRotate, 0.3f).OnComplete(() =>
                    { other.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);});

                }


            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashingcar"))
        {
            
            coinCounter = 0;
            PlayerMovement.movement›nstance.anim.Play("HitByCar");
            ›sH›tCar = true;
             
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude));
            StartCoroutine(WaithAnimProces(collision.gameObject));
            SoundManager.instance.CrashSound();
             
            spark.Play();
            for (int i = StackCoin.instance.coins.Count - 1; i > 0; i--)
            {
                RigidbodyState(i, false, true);
                CollisionState(i, true, false);

                Rigidbody rb;
                rb = StackCoin.instance.coins[i].GetComponent<Rigidbody>();
                int directionValue = Random.Range(-2, 3);
                rb.AddForce(new Vector3(directionValue, 2, directionValue));

                StackCoin.instance.coins[i].transform.parent = null;

                StackCoin.instance.coins.RemoveAt(i);
                PlayerMovement.movement›nstance.speed = 5;
            }

        }
    }
  
    void RigidbodyState(int index,bool state1, bool state2)
    {
        StackCoin.instance.coins[index].GetComponent<Rigidbody>().isKinematic = state1;
        StackCoin.instance.coins[index].GetComponent<Rigidbody>().useGravity = state2;

    }
    void CollisionState(int index,bool state1, bool state2)
    {
        StackCoin.instance.coins[index].GetComponent<BoxCollider>().enabled = state1;
        StackCoin.instance.coins[index].GetComponent<BoxCollider>().isTrigger = state2;

    }

    IEnumerator WaithAnimProces(GameObject obj)
    {
        yield return new WaitForSeconds(0.4f);
        if (obj.name != "obstacle")
        {
            SoundManager.instance.HornSound();
        }
        
        yield return new WaitForSeconds(3f);
        ›sH›tCar = false;
        PlayerMovement.movement›nstance.speed = 5;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
