using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [SyncVar]
    public bool isGameInProgress;

    public static Action ServerOnGameStarted;
    public static Action ServerOnGameStoped;

    public static Action ClientOnGameStarted;
    public static Action ClientOnGameStoped;


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

    public override void OnStartClient()
    {
        ScoreBoardManager.Instance.InitializeScoreBoard();
    }

    public bool IsServer()
    {
        return NetworkServer.active;
    }
   
    public void ReadyCheck()
    {
        if(isGameInProgress) { return; }

        bool allReady = true;

        foreach (Player player in CustomNetworkManager.Instance.players)
        {
            if(!player.isReady)
                allReady = false;
        }

        if (allReady)
        {
            CountDownManager.Instance.StartCountDown();
        }
        else if(CountDownManager.Instance.countDownTimerStarted)
        {
            CountDownManager.Instance.StopCountDown();
        }           
    }
    
    private void UnReadyAllPlayers()
    {
        foreach (Player player in CustomNetworkManager.Instance.players)
        {
            player.isReady = false;
            player.score = 0;
        }
    }

    public void StartGame()
    {
        isGameInProgress = true;
        ServerOnGameStarted?.Invoke();
        UnReadyAllPlayers();
        RpcGameStarted();
    }
    public void StopGame()
    {
        isGameInProgress = false;
        ServerOnGameStoped?.Invoke();
        RpcGameStoped();
    }

    [ClientRpc]
    private void RpcGameStarted()
    {
        ClientOnGameStarted?.Invoke();
    }

    [ClientRpc]
    private void RpcGameStoped()
    {
        ClientOnGameStoped?.Invoke();
    }
}
