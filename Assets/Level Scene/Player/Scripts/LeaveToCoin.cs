using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LeaveToCoin : MonoBehaviour
{
    public Transform PutCoinToTruck;
    public Transform PutCoinToTruck_2;
    ///public Transform CoinsObject;

    [HideInInspector] private float delay, delay2;
    public float Yaxis,Yaxis2;
    public float coinToTruck_Counter;
    public bool triggerCheck;
    public static LeaveToCoin instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    //int CoinsObjectChildCount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            triggerCheck = true;
            if (PutCoinToTruck.childCount > 0)
            {
                Yaxis = PutCoinToTruck.GetChild(PutCoinToTruck.childCount - 1).position.y;
                
            }
            else
            {
                Yaxis = PutCoinToTruck.position.y;
            }
           
            if (PutCoinToTruck_2.childCount > 0)
            {
                Yaxis2 = PutCoinToTruck_2.GetChild(PutCoinToTruck_2.childCount - 1).position.y;

            }
            else
            {
                Yaxis2 = PutCoinToTruck_2.position.y;
            }



            for (int i = StackCoin.instance.coins.Count - 1; i > 0; i--)
            {
                if (triggerCheck == true)
                {
                    if (PlayerCollisionToCoin.coinColl_›nstance.coinCounter >= 11)
                    {
                        PlayerCollisionToCoin.coinColl_›nstance.coinCounter = 0;
                    }

                    PlayerMovement.movement›nstance.anim.enabled = false;


                    PlayerMovement.movement›nstance.speed = 0;
                    Joystick.instance.Moved = false;
                    StackCoin.instance.coins[i].transform.parent = PutCoinToTruck;
                    

                    Vector3 targetPos = new Vector3(PutCoinToTruck.position.x, PutCoinToTruck.position.y, PutCoinToTruck.position.z);
                    StackCoin.instance.coins[i].transform.DOJump(new Vector3(targetPos.x, Yaxis, targetPos.z), 2.5f, 1, 0.5f).SetDelay(delay).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        StartCoroutine(MovementOpening());
                    });
                    StackCoin.instance.coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);
                    if (PutCoinToTruck.childCount >= 13)
                    {
                        StackCoin.instance.coins[i].transform.parent = PutCoinToTruck_2;
                        Vector3 targetPos2 = new Vector3(PutCoinToTruck_2.position.x, PutCoinToTruck_2.position.y, PutCoinToTruck_2.position.z);
                        StackCoin.instance.coins[i].transform.DOJump(new Vector3(targetPos2.x, Yaxis2, targetPos2.z), 2.5f, 1, 0.5f).SetDelay(delay2).SetEase(Ease.Flash);
                        StackCoin.instance.coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);
                        Yaxis2 += 0.2f;
                       
                    }
                }
               
                
                Yaxis += 0.2f;
                delay += 0.1f;
                delay2 += 0.1f;
                coinToTruck_Counter += 1;
                GameManager._instance.coinSlider.DOValue(coinToTruck_Counter, 1f);
                StackCoin.instance.coins.RemoveAt(i);
                if (PlayerCollisionToCoin.coinColl_›nstance.coinCounter > 0)
                {
                    PlayerCollisionToCoin.coinColl_›nstance.coinCounter -= 1;
                }
                
                //Debug.Log(coinToTruck_Counter);

            }
        }
    }


    IEnumerator MovementOpening()
    {
        yield return new WaitForSeconds(1f);
        Joystick.instance.Moved = true;
        triggerCheck = false;
        PlayerMovement.movement›nstance.speed = 5;
        PlayerMovement.movement›nstance.anim.enabled = true;

    }


    private void Update()
    {
        //CoinsObjectChildCount = CoinsObject.childCount;
        //GameManager._instance.coinSlider.maxValue = CoinsObjectChildCount;
        //coinToTruck_Counter = TruckToPutCoin.childCount;
        //PlayerCollisionToCoin.coinColl_›nstance.coinCounter = StackCoin.instance.Player.transform.childCount - 1;

    }

}
