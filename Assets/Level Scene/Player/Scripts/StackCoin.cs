using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackCoin : MonoBehaviour
{
    public static StackCoin instance;
    public List<GameObject> coins = new List<GameObject>();
    //public float movement_Delay;
    public GameObject Player_Carrier;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void StackCoins(GameObject obj, int index)
    {
        obj.transform.parent = Player_Carrier.transform;
        Vector3 coinPos = coins[index].transform.localPosition;
        coinPos.y += 0.2f;
        obj.transform.localPosition = coinPos;
        coins.Add(obj);

       
        
    }

   /* public void CoinsMovement()
    {
        for (int i = 1; i < coins.Count; i++)
        {
            
            Vector3 pos = coins[i].transform.localPosition;
            pos.z=coins[0].transform.localPosition.z;
            pos.x=coins[0].transform.localPosition.x;
            coins[i-1].transform.DOLocalMove(pos, movement_Delay);
        }
    }*/
   
    /*public void MoveOrigin()
    {
        for (int i = 1; i < coins.Count; i++)
        {
            Vector3 pos = coins[i].transform.localPosition;
            pos.z= coins[0].transform.localPosition.z;
            coins[i].transform.DOLocalMove(pos, 0.90f);

            Vector3 poss = coins[i].transform.localPosition;
            poss.x = coins[i - 1].transform.localPosition.x;
            coins[i].transform.DOLocalMove(poss, movement_Delay);
        }
    }*/

}
