using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAIStackCoin : MonoBehaviour
{
    public static BlueAIStackCoin blueAInstance;
    public List<GameObject> Blue_Aý_coins = new List<GameObject>();

    public GameObject Blue_aý_Carrier;

    private void Awake()
    {
        if (blueAInstance == null)
        {
            blueAInstance = this;
        }

    }


    public void BlueAýStackCoins(GameObject obj, int index)
    {
        obj.transform.parent = Blue_aý_Carrier.transform;
        //.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.transform;
        Vector3 coinPos = Blue_Aý_coins[index].transform.localPosition;
        coinPos.y += 0.2f;
        obj.transform.localPosition = coinPos;
        Blue_Aý_coins.Add(obj);



    }
}
