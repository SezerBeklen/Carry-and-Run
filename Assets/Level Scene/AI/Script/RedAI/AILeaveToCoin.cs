using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AILeaveToCoin : MonoBehaviour
{
    public Transform a�_PutCoinToTruck;
    public Transform a�_PutCoinToTruck_2;
    

    [HideInInspector] 
    private float a�_delay;
    private float a�_Yaxis, a�_Yaxis2;
    private float a�_delayblue;
    private float a�_Yaxisb, a�_Yaxis2b;

    public float a�_coinToTruck_Counter;
    public float a�_coinToTruck_Counterblue;

    public bool a�_triggerCheck;
    public bool a�_triggerCheckblue;

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

            a�_triggerCheck = true;
            if (a�_PutCoinToTruck.childCount > 0)
            {
                a�_Yaxis = a�_PutCoinToTruck.GetChild(a�_PutCoinToTruck.childCount - 1).position.y;

            }
            else
            {
                a�_Yaxis = a�_PutCoinToTruck.position.y;
            }

            if (a�_PutCoinToTruck_2.childCount > 0)
            {
                a�_Yaxis2 = a�_PutCoinToTruck_2.GetChild(a�_PutCoinToTruck_2.childCount - 1).position.y;

            }
            else
            {
                a�_Yaxis2 = a�_PutCoinToTruck_2.position.y;
            }



            for (int i = AIStackCoin.instancee.a�_coins.Count - 1; i > 0; i--)
            {
                if (a�_triggerCheck == true) 
                {
                    if (AImovement.instanceAI.a�CoinCounter >= AImovement.instanceAI.a�_CoinCounter_Limit - 1) 
                    {
                        AImovement.instanceAI.a�CoinCounter = 0;
                    }

                    AImovement.instanceAI.a�_anim.enabled = false;
                    AImovement.instanceAI.a�_�smove = false;
                    AImovement.instanceAI.a�_player.speed = 0;
                    AIStackCoin.instancee.a�_coins[i].transform.parent = a�_PutCoinToTruck;


                    Vector3 targetPos = new Vector3(a�_PutCoinToTruck.position.x, a�_PutCoinToTruck.position.y, a�_PutCoinToTruck.position.z);
                    AIStackCoin.instancee.a�_coins[i].transform.DOJump(new Vector3(targetPos.x, a�_Yaxis, targetPos.z), 2.5f, 1, 0.5f).SetDelay(a�_delay).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        StartCoroutine(MovementOpening());
                    });
                    AIStackCoin.instancee.a�_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);

                    if (a�_PutCoinToTruck.childCount >= AImovement.instanceAI.a�_CoinCounter_Limit + 4)
                    {
                        AIStackCoin.instancee.a�_coins[i].transform.parent = a�_PutCoinToTruck_2;
                        Vector3 targetPos2 = new Vector3(a�_PutCoinToTruck_2.position.x, a�_PutCoinToTruck_2.position.y, a�_PutCoinToTruck_2.position.z);
                        AIStackCoin.instancee.a�_coins[i].transform.DOJump(new Vector3(targetPos2.x, a�_Yaxis2, targetPos2.z), 2.5f, 1, 0.5f).SetDelay(a�_delay).SetEase(Ease.Flash);
                        AIStackCoin.instancee.a�_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);
                        a�_Yaxis2 += 0.2f;

                    }
                }


                a�_Yaxis += 0.2f;
                a�_delay += 0.1f;
                a�_coinToTruck_Counter += 1;
                GameManager._instance.a�_RedcoinSlider.DOValue(a�_coinToTruck_Counter, 1f);
                AIStackCoin.instancee.a�_coins.RemoveAt(i);

                if (AImovement.instanceAI.a�CoinCounter > 0)
                {
                    AImovement.instanceAI.a�CoinCounter -= 1;
                }
                
                //Debug.Log(coinToTruck_Counter);

            }
        }
        
        if (coll.gameObject.tag == "BluePlayer")
        {

            a�_triggerCheckblue = true;
            if (a�_PutCoinToTruck.childCount > 0)
            {
                a�_Yaxisb = a�_PutCoinToTruck.GetChild(a�_PutCoinToTruck.childCount - 1).position.y;

            }
            else
            {
                a�_Yaxisb = a�_PutCoinToTruck.position.y;
            }

            if (a�_PutCoinToTruck_2.childCount > 0)
            {
                a�_Yaxis2b = a�_PutCoinToTruck_2.GetChild(a�_PutCoinToTruck_2.childCount - 1).position.y;

            }
            else
            {
                a�_Yaxis2b = a�_PutCoinToTruck_2.position.y;
            }



            for (int i = BlueAIStackCoin.blueAInstance.Blue_A�_coins.Count - 1; i > 0; i--)
            {
                if (a�_triggerCheckblue == true) 
                {
                    if (BlueAIMove.instance_AI.Bluea�CoinCounter >= BlueAIMove.instance_AI.Bluea�_CoinCounter_Limit - 1) 
                    {
                        BlueAIMove.instance_AI.Bluea�CoinCounter = 0;
                    }

                    BlueAIMove.instance_AI.a�_anim.enabled = false;
                    BlueAIMove.instance_AI.a�_�smove = false;
                    BlueAIMove.instance_AI.a�_player.speed = 0;
                    BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].transform.parent = a�_PutCoinToTruck;


                    Vector3 targetPos = new Vector3(a�_PutCoinToTruck.position.x, a�_PutCoinToTruck.position.y, a�_PutCoinToTruck.position.z);
                    BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].transform.DOJump(new Vector3(targetPos.x, a�_Yaxisb, targetPos.z), 2.5f, 1, 0.5f).SetDelay(a�_delayblue).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        StartCoroutine(BlueA�MovementOpening());
                    });
                    BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);

                    if (a�_PutCoinToTruck.childCount >= AImovement.instanceAI.a�_CoinCounter_Limit + 4)
                    {
                        BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].transform.parent = a�_PutCoinToTruck_2;
                        Vector3 targetPos2 = new Vector3(a�_PutCoinToTruck_2.position.x, a�_PutCoinToTruck_2.position.y, a�_PutCoinToTruck_2.position.z);
                        BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].transform.DOJump(new Vector3(targetPos2.x, a�_Yaxis2b, targetPos2.z), 2.5f, 1, 0.5f).SetDelay(a�_delayblue).SetEase(Ease.Flash);
                        BlueAIStackCoin.blueAInstance.Blue_A�_coins[i].transform.DOLocalRotate(Vector3.zero, 0.1f);
                        a�_Yaxis2b += 0.2f;

                    }
                }


                a�_Yaxisb += 0.2f;
                a�_delayblue += 0.1f;
                a�_coinToTruck_Counterblue += 1;
                GameManager._instance.a�_BluecoinSlider.DOValue(a�_coinToTruck_Counterblue, 1f);
                BlueAIStackCoin.blueAInstance.Blue_A�_coins.RemoveAt(i);

                if (BlueAIMove.instance_AI.Bluea�CoinCounter > 0)
                {
                    BlueAIMove.instance_AI.Bluea�CoinCounter -= 1;
                }
                 

            }
        }
    }


    IEnumerator MovementOpening()
    {
        yield return new WaitForSeconds(1f);
        a�_triggerCheck = false;
        AImovement.instanceAI.a�_player.speed = 5;
        AImovement.instanceAI.a�_�smove = true;
        AImovement.instanceAI.a�_anim.enabled = true;


    }
    IEnumerator BlueA�MovementOpening()
    {
        yield return new WaitForSeconds(1f);
        a�_triggerCheckblue = false;
        BlueAIMove.instance_AI.a�_player.speed = 5;
        BlueAIMove.instance_AI.a�_�smove = true;
        BlueAIMove.instance_AI.a�_anim.enabled = true;


    }
}
