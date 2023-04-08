using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public int PlayersOnsceneTargetCount;
    //[SerializeField] private int CheckUI;
    public static UIManager instance;
    [HideInInspector]
    public AudioSource failsound;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        failsound = GetComponent<AudioSource>();

        GameManager._instance.coinSlider.maxValue = PlayersOnsceneTargetCount;
        GameManager._instance.a�_RedcoinSlider.maxValue = PlayersOnsceneTargetCount;
        GameManager._instance.a�_BluecoinSlider.maxValue = PlayersOnsceneTargetCount;
        
    }
   /* void Update()
    {
        if (GameManager._instance.coinSlider.value >= PlayersOnsceneTargetCount)
        {
            CheckUI = 1;
        }

        if (GameManager._instance.a�_RedcoinSlider.value >= PlayersOnsceneTargetCount)
        {
            CheckUI = 2;
        }

        if (GameManager._instance.a�_BluecoinSlider.value >= PlayersOnsceneTargetCount)
        {

            CheckUI = 3;
        }



        switch (CheckUI)
        {
            case 1: PlayerWin();
                break;
            case 2: RedA�Win(); 
                break;
            case 3: BlueA�Win();
                break;
        }


    }*/

    /*void PlayerWin()
    {
        GameManager._instance.gamestate = GameManager.GameState.Next;
        
    }

    void RedA�Win()
    {
        GameManager._instance.gamestate = GameManager.GameState.GameOver;
       
    }

    void BlueA�Win()
    {
        GameManager._instance.gamestate = GameManager.GameState.GameOver;
        

    }*/

}
