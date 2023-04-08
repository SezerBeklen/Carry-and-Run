using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AILeaveToCoin : MonoBehaviour
{
    public Transform aý_PutCoinToTruck;
    public Transform aý_PutCoinToTruck_2;
    

    [HideInInspector] 
    private float aý_delay;
    private float aý_Yaxis, aý_Yaxis2;
    private float aý_delayblue;
    private float aý_Yaxisb, aý_Yaxis2b;

    public float aý_coinToTruck_Counter;
    public float aý_coinToTruck_Counterblue;

    public bool aý_triggerCheck;
    public bool aý_triggerCheckblue;

    public static AILeaveToCoin instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "RedPlayer")
        {

            aý_triggerCheck = true;
            if (aý_PutCoinToTruck.childCount > 0)
            {
                aý_Yaxis = aý_PutCoinToTruck.GetChild(aý_PutCoinToTruck.childCount - 1).position.y;

            }
            else
            {
                aý_Yaxis = aý_PutCoinToTruck.position.y;
            }

            if (aý_PutCoinToTruck_2.childCount > 0)
            {
                aý_Yaxis2 = aý_PutCoinToTruck_2.GetChild(aý_PutCoinToTruck_2.childCount - 1).position.y;

            }
            else
            {
                aý_Yaxis2 = aý_PutCoinToTruck_2.position.y;
            }



            for (int i = AIStackCoin.instancee.aý_coins.Count - 1; i > 0; i--)
            {
                if (aý_triggerCheck == true) 
                {
                    if (AImovement.instanceAI.aýCoinCounter >= AImovement.instanceAI.aý_CoinCounter_Limit - 1) 
                    {
                        AImovement.instanceAI.aýCoinCounter = 0;
                    }

                    AImovement.instanceAI.aý_anim.enabled = false;
                    AImovement.instanceAI.aý_Ýsmove = false;
                    AImovement.instanceAI.aý_player.speed = 0;
                    AIStackCoin.instancee.aý_coins[i].transform.parent = aý_PutCoinToTruck;


                    Vector3 targetPos = new Vector3(aý_PutCoinToTruck.position.x, aý_PutCoinToTruck.position.y, aý_PutCoinToTruck.position.z);
                    AIStackCoin.instancee.aý_coins[i].transform.DOJump(new Vector3(targetPos.x, aý_Yaxis, targetPos.z), 2.5f, 1, 0.5f).SetDelay(aý_delay).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        StartCoroutine(MovementOpening());
                    });
                    AIStackCoin.instancee.aý_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);

                    if (aý_PutCoinToTruck.childCount >= AImovement.instanceAI.aý_CoinCounter_Limit + 4)
                    {
                        AIStackCoin.instancee.aý_coins[i].transform.parent = aý_PutCoinToTruck_2;
                        Vector3 targetPos2 = new Vector3(aý_PutCoinToTruck_2.position.x, aý_PutCoinToTruck_2.position.y, aý_PutCoinToTruck_2.position.z);
                        AIStackCoin.instancee.aý_coins[i].transform.DOJump(new Vector3(targetPos2.x, aý_Yaxis2, targetPos2.z), 2.5f, 1, 0.5f).SetDelay(aý_delay).SetEase(Ease.Flash);
                        AIStackCoin.instancee.aý_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);
                        aý_Yaxis2 += 0.2f;

                    }
                }


                aý_Yaxis += 0.2f;
                aý_delay += 0.1f;
                aý_coinToTruck_Counter += 1;
                GameManager._instance.aý_RedcoinSlider.DOValue(aý_coinToTruck_Counter, 1f);
                AIStackCoin.instancee.aý_coins.RemoveAt(i);

                if (AImovement.instanceAI.aýCoinCounter > 0)
                {
                    AImovement.instanceAI.aýCoinCounter -= 1;
                }
                
                //Debug.Log(coinToTruck_Counter);

            }
        }
        
        if (coll.gameObject.tag == "BluePlayer")
        {

            aý_triggerCheckblue = true;
            if (aý_PutCoinToTruck.childCount > 0)
            {
                aý_Yaxisb = aý_PutCoinToTruck.GetChild(aý_PutCoinToTruck.childCount - 1).position.y;

            }
            else
            {
                aý_Yaxisb = aý_PutCoinToTruck.position.y;
            }

            if (aý_PutCoinToTruck_2.childCount > 0)
            {
                aý_Yaxis2b = aý_PutCoinToTruck_2.GetChild(aý_PutCoinToTruck_2.childCount - 1).position.y;

            }
            else
            {
                aý_Yaxis2b = aý_PutCoinToTruck_2.position.y;
            }



            for (int i = BlueAIStackCoin.blueAInstance.Blue_Aý_coins.Count - 1; i > 0; i--)
            {
                if (aý_triggerCheckblue == true) 
                {
                    if (BlueAIMove.instance_AI.BlueaýCoinCounter >= BlueAIMove.instance_AI.Blueaý_CoinCounter_Limit - 1) 
                    {
                        BlueAIMove.instance_AI.BlueaýCoinCounter = 0;
                    }

                    BlueAIMove.instance_AI.aý_anim.enabled = false;
                    BlueAIMove.instance_AI.aý_Ýsmove = false;
                    BlueAIMove.instance_AI.aý_player.speed = 0;
                    BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].transform.parent = aý_PutCoinToTruck;


                    Vector3 targetPos = new Vector3(aý_PutCoinToTruck.position.x, aý_PutCoinToTruck.position.y, aý_PutCoinToTruck.position.z);
                    BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].transform.DOJump(new Vector3(targetPos.x, aý_Yaxisb, targetPos.z), 2.5f, 1, 0.5f).SetDelay(aý_delayblue).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        StartCoroutine(BlueAýMovementOpening());
                    });
                    BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);

                    if (aý_PutCoinToTruck.childCount >= AImovement.instanceAI.aý_CoinCounter_Limit + 4)
                    {
                        BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].transform.parent = aý_PutCoinToTruck_2;
                        Vector3 targetPos2 = new Vector3(aý_PutCoinToTruck_2.position.x, aý_PutCoinToTruck_2.position.y, aý_PutCoinToTruck_2.position.z);
                        BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].transform.DOJump(new Vector3(targetPos2.x, aý_Yaxis2b, targetPos2.z), 2.5f, 1, 0.5f).SetDelay(aý_delayblue).SetEase(Ease.Flash);
                        BlueAIStackCoin.blueAInstance.Blue_Aý_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);
                        aý_Yaxis2b += 0.2f;

                    }
                }


                aý_Yaxisb += 0.2f;
                aý_delayblue += 0.1f;
                aý_coinToTruck_Counterblue += 1;
                GameManager._instance.aý_BluecoinSlider.DOValue(aý_coinToTruck_Counterblue, 1f);
                BlueAIStackCoin.blueAInstance.Blue_Aý_coins.RemoveAt(i);

                if (BlueAIMove.instance_AI.BlueaýCoinCounter > 0)
                {
                    BlueAIMove.instance_AI.BlueaýCoinCounter -= 1;
                }
                 

            }
        }
    }


    IEnumerator MovementOpening()
    {
        yield return new WaitForSeconds(1f);
        aý_triggerCheck = false;
        AImovement.instanceAI.aý_player.speed = 5;
        AImovement.instanceAI.aý_Ýsmove = true;
        AImovement.instanceAI.aý_anim.enabled = true;


    }
    IEnumerator BlueAýMovementOpening()
    {
        yield return new WaitForSeconds(1f);
        aý_triggerCheckblue = false;
        BlueAIMove.instance_AI.aý_player.speed = 5;
        BlueAIMove.instance_AI.aý_Ýsmove = true;
        BlueAIMove.instance_AI.aý_anim.enabled = true;


    }
}
