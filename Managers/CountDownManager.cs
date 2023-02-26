using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class CountDownManager : NetworkBehaviour
{
    private static CountDownManager _instance;

    public static CountDownManager Instance { get { return _instance; } }

    [SyncVar(hook = nameof(ClientHandleCountDownTimerVisible))]
    public bool countDownTimerStarted;
    [SyncVar(hook = nameof(ClientHandleGameTimerVisible))]
    public bool gameTimerStarted;
    [SyncVar(hook = nameof(ClientHandleCountDownTimer))]
    public int gameStartCountDownTimer;
    [SyncVar(hook = nameof(ClientHandleGameTimer))]
    public int gameTimer;

    private int countDownTicksPassed;
    private int maxGameTimer;

    [Header("UI")]
    public GameObject countDownTimerPanel;
    public GameObject gameTimerPanel;
    public TMP_Text countDownTimerTxt;
    public TMP_Text gameTimerTxt;

    [Header("Countdown Settings")]
    public int setCountDownTimer;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public override void OnStartServer()
    {
        GameManager.ServerOnGameStarted += StartGameTimer;
        GameManager.ServerOnGameStoped += StopGameTimer;
        GlobalTimer.clockTicks += TimeTick;
    }

    public override void OnStopServer()
    {
        GameManager.ServerOnGameStarted -= StartGameTimer;
        GameManager.ServerOnGameStoped -= StopGameTimer;
        GlobalTimer.clockTicks -= TimeTick;
    }

    private void ClientHandleCountDownTimer(int oldValue, int newValue)
    {
        countDownTimerTxt.text = gameStartCountDownTimer.ToString();
        AnnouncerManager.Instance.AnnounceCountdown(newValue);
    }

    private void ClientHandleCountDownTimerVisible(bool oldValue, bool newValue)
    {
        countDownTimerPanel.SetActive(newValue);
    }

    private void ClientHandleGameTimer(int oldValue, int newValue)
    {
        gameTimerTxt.text = gameTimer.ToString();
    }

    private void ClientHandleGameTimerVisible(bool oldValue, bool newValue)
    {
        gameTimerPanel.SetActive(newValue);
    }

    private void TimeTick()
    {
        countDownTicksPassed++;

        if(gameTimerStarted)
        {
            if (countDownTicksPassed % GlobalTimer.Instance.ticksPerSecond == 0)
            {
                gameTimer++;
            }

            if (gameTimer >= maxGameTimer)
            {
                GameTimeFinished();
            }
        }

        if(countDownTimerStarted)
        {
            if (countDownTicksPassed % GlobalTimer.Instance.ticksPerSecond == 0)
            {
                gameStartCountDownTimer--;
            }

            if (gameStartCountDownTimer <= 0)
            {
                CountDownCompleted();
            }
        }       
    }

    private void ResetTimers()
    {
        gameStartCountDownTimer = setCountDownTimer;
        maxGameTimer = SteamLobby.Instance.gameTimer;
        gameTimer = 0;
        countDownTicksPassed = 0;
    }

    public void StartCountDown()
    {
        ResetTimers();
        countDownTimerStarted = true;      
    }
 
    public void StopCountDown()
    {               
        countDownTimerStarted = false;
        ResetTimers();
    }

    public void StartGameTimer()
    {
        gameTimerStarted = true;
    }

    public void StopGameTimer()
    {
        gameTimerStarted = false;
        ResetTimers();
    }

    private void GameTimeFinished()
    {
        StopGameTimer();
        GameManager.Instance.StopGame();
    }

    private void CountDownCompleted()
    {
        StopCountDown();
        GameManager.Instance.StartGame();
    }
}
