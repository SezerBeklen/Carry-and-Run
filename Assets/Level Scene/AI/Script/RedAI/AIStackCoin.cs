using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStackCoin : MonoBehaviour
{
    public static AIStackCoin instancee;
    public List<GameObject> aı_coins = new List<GameObject>();
    
    public GameObject aı_Carrier;

    private void Awake()
    {
        if (instancee == null)
        {
            instancee = this;
        }

    }
    

    public void AIStackCoins(GameObject obj, int index)
    {
        obj.transform.parent = aı_Carrier.transform;
        //.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.transform;
        Vector3 coinPos = aı_coins[index].transform.localPosition;
        coinPos.y += 0.2f;
        obj.transform.localPosition = coinPos;
        aı_coins.Add(obj);



    }
}
