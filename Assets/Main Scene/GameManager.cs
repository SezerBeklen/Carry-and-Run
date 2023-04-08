using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject StartP, IngameP, NextP, GameOverP;
    public float CountDown = 2f;
    private float NextCountDown = 3f;
    [SerializeField] private int asynScne›ndex = 1;
    [SerializeField] public Slider coinSlider;
    [SerializeField] public Slider a˝_RedcoinSlider;
    [SerializeField] public Slider a˝_BluecoinSlider;
    public static GameManager _instance;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
         
    }

    public enum GameState
    {
        Start,
        Ingame,
        Next,
        GameOver
    }

    public GameState gamestate;


    public enum Panels
    {
        StartP,
        NextP,
        GameOverP,
        IngameP


    }

    private void Start()
    {
        gamestate = GameState.Start;
       
    }
    private void Update()
    {
        if (coinSlider.value >= UIManager.instance.PlayersOnsceneTargetCount)
        {
        
            gamestate = GameState.Next;
            SoundManager.instance.WinSound();
            a˝_RedcoinSlider.value = 0;
            a˝_BluecoinSlider.value = 0;
        }
        
        if (a˝_RedcoinSlider.value >= UIManager.instance.PlayersOnsceneTargetCount || a˝_BluecoinSlider.value >= UIManager.instance.PlayersOnsceneTargetCount && coinSlider.value < UIManager.instance.PlayersOnsceneTargetCount)
        {
            gamestate = GameState.GameOver;
            SoundManager.instance.FailSound();

        }




        switch (gamestate)
        {
            case GameState.Start: GameStart();
                break;
            case GameState.Ingame: GameIngame();
                break;
            case GameState.Next: GameFinish();
                break;
            case GameState.GameOver: GameOver();
                break;

        }

    }
    void PanelController(Panels currentPanel)
    {
        StartP.SetActive(false);
        IngameP.SetActive(false);
        NextP.SetActive(false);
        GameOverP.SetActive(false);

        switch (currentPanel)
        {
            case Panels.StartP: StartP.SetActive(true);
                break;
            case Panels.IngameP: IngameP.SetActive(true);
                break;
            case Panels.NextP: NextP.SetActive(true);
                break;
            case Panels.GameOverP: GameOverP.SetActive(true);
                break;
              
        }
    }

    void GameStart()
    {
        PanelController(Panels.StartP);
        PlayerMovement.movement›nstance.speed = 0;
       
        if (Input.anyKeyDown)
        {
            gamestate = GameState.Ingame;
        }
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadSceneAsync(asynScne›ndex, LoadSceneMode.Additive);
        }
    }
    void GameIngame()
    {
        PanelController(Panels.IngameP);
        if (PlayerMovement.movement›nstance.speed == 0) 
        {
            PlayerMovement.movement›nstance.speed = 5;
        }
    }
    void GameFinish()
    {
        NextCountDown -= Time.deltaTime;
        if (NextCountDown < 0)
        {
            PanelController(Panels.NextP);

        }
        
        PlayerMovement.movement›nstance.speed = 0;
        BlueAIMove.instance_AI.a˝_player.speed = 0;
        AImovement.instanceAI.a˝_player.speed = 0;
        coinSlider.value = 0;
        a˝_RedcoinSlider.value = 0;
        a˝_BluecoinSlider.value = 0;
        
    }
    void GameOver()
    {
        
        CountDown -= Time.deltaTime;
        if (CountDown < 0)
        {
            PanelController(Panels.GameOverP);
        }

        PlayerMovement.movement›nstance.speed = 0;
        BlueAIMove.instance_AI.a˝_player.speed = 0;
        AImovement.instanceAI.a˝_player.speed = 0;
        coinSlider.value = 0;
        a˝_RedcoinSlider.value = 0;
        a˝_BluecoinSlider.value = 0;
        

    }

    public void RestartButton()
    {
        SceneManager.UnloadSceneAsync(asynScne›ndex);
        SceneManager.LoadSceneAsync(asynScne›ndex, LoadSceneMode.Additive);
        gamestate = GameState.Start;
        PlayerMovement.movement›nstance.speed = 5;
        a˝_RedcoinSlider.value = 0;
        a˝_BluecoinSlider.value = 0;
        coinSlider.value = 0;
        CountDown = 2;
        NextCountDown = 3;
        SoundManager.instance.ButtonSound();
        SoundManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);



    }
    public void NextLevelButton()
    {
        if (SceneManager.sceneCountInBuildSettings == asynScne›ndex + 1)
        {
            
            SceneManager.UnloadSceneAsync(asynScne›ndex);
            asynScne›ndex = 1;
            SceneManager.LoadSceneAsync(asynScne›ndex, LoadSceneMode.Additive);

        }
        else
        {
            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync(asynScne›ndex);
                asynScne›ndex++;
            }
            
            SceneManager.LoadSceneAsync(asynScne›ndex, LoadSceneMode.Additive);
        }
        PlayerMovement.movement›nstance.speed = 5;
        gamestate = GameState.Start;
        CountDown = 2;
        NextCountDown = 3;
        SoundManager.instance.ButtonSound();
        SoundManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
