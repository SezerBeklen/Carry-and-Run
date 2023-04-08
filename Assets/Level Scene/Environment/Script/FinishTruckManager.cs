using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTruckManager : MonoBehaviour
{

    public GameObject yellow_Turck, red_Truck, blue_Truck;
    private Animator yellow_Turck_anim, red_Truck_anim, blue_Truck_anim;
    private GameObject child1, child2;
    
     
    void Start()
    {
         
        yellow_Turck_anim = yellow_Turck.gameObject.GetComponent<Animator>();
        red_Truck_anim = red_Truck.gameObject.GetComponent<Animator>();
        blue_Truck_anim = blue_Truck.gameObject.GetComponent<Animator>();
        child1 = red_Truck.transform.GetChild(1).gameObject;
        child2 = red_Truck.transform.GetChild(2).gameObject;
    }

   
    void Update()
    {
        if (GameManager._instance.gamestate == GameManager.GameState.Ingame) 
        {
            if (LeaveToCoin.instance.coinToTruck_Counter >= UIManager.instance.PlayersOnsceneTargetCount)
            {
                StartCoroutine(DelayToTruckMove(yellow_Turck, yellow_Turck_anim, "yellow"));

            }

            if (AILeaveToCoin.instance.aý_coinToTruck_Counterblue >= UIManager.instance.PlayersOnsceneTargetCount)
            {
                StartCoroutine(DelayToTruckMove(blue_Truck, blue_Truck_anim, "blue"));

            }


            if (child1.transform.childCount + child2.transform.childCount >= UIManager.instance.PlayersOnsceneTargetCount)
            {
                StartCoroutine(DelayToTruckMove(red_Truck, red_Truck_anim, "red"));


            }
        }
    
        

    }


    IEnumerator DelayToTruckMove(GameObject obj, Animator anim, string animname)
    {
        yield return new WaitForSeconds(2.5f);
        anim.Play(animname);
        //yield return new WaitForSeconds(0.4f);
        yield return new WaitForSeconds(1f);
        SoundManager.instance.WheelSound();
        yield return new WaitForSeconds(1);
        anim.enabled = false;
        obj.transform.Translate(Time.deltaTime * 500 * Vector3.forward);
    }
}
