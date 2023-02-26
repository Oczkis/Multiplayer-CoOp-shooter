using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System;
using Mirror;

public class Player : NetworkBehaviour
{

    [SyncVar(hook = nameof(HandleSteamIdUpdated))]
    private ulong steamId;

    [SyncVar(hook = nameof(AuthorityHandleLobbyOwnerStateUpdated))]
    private bool isLobbyOwner = false;

    [SyncVar(hook = nameof(ClientHandleLobbySlotUpdated))]
    [HideInInspector] public int lobbySlotOccupied;

    [SyncVar(hook = nameof(ClientHandleChosenColorUpdated))]
    [HideInInspector] public int colourID;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    [HideInInspector] public string playerName;

    [SyncVar(hook = nameof(ClientHandlePlayerReadyUpdated))]
    public bool isReady;

    [SyncVar(hook = nameof(ClientHandlePlayerScoreUpdated))]
    public int score;

    [SyncVar]
    public int playerID;

    public PlayerPanel playerScoreBoardPanel;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action<int, string> ClientOnNameUpdated;
    public static event Action<int, int> ClientOnChosenColorUpdated;

    #region Client

    // Starts and stops

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        CustomNetworkManager.Instance.players.Add(this);
             
        ClientOnInfoUpdated?.Invoke();       

        if(!hasAuthority) { return; }

        PlayerManager.Instance.myPlayer = this;
        LobbyMenu.ClientOnLobbySlotClicked += HandleClientLobbySlotClicked;
        LobbyMenu.ClientOnColorClicked += HandleClientLobbyColorClicked;
    }

    public override void OnStopClient()
    {
        CustomNetworkManager.Instance.players.Remove(this);

        ClientOnInfoUpdated?.Invoke();

        if (!hasAuthority) { return; }

        LobbyMenu.ClientOnLobbySlotClicked -= HandleClientLobbySlotClicked;
        LobbyMenu.ClientOnColorClicked -= HandleClientLobbyColorClicked;
    }

    // SyncVar handlers

    private void HandleClientLobbySlotClicked(int i)
    {
        CmdOccupyLobbySlot(i);
    }

    private void HandleClientLobbyColorClicked(int i)
    {
        CmdChooseColor(i);
    }

    private void ClientHandleLobbySlotUpdated(int oldValue, int newValue)
    {
        ClientOnInfoUpdated?.Invoke();

        if (hasAuthority)
            LobbyMenu.Instance.SetPanelsInteractable(lobbySlotOccupied);
    }

    private void ClientHandleChosenColorUpdated(int oldValue, int newValue)
    {
        ClientOnChosenColorUpdated?.Invoke(lobbySlotOccupied, newValue);
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnNameUpdated?.Invoke(lobbySlotOccupied, newDisplayName);
    }

    private void ClientHandlePlayerScoreUpdated(int oldValue, int newValue)
    {
        if (hasAuthority)
            UIManager.Instance.myScore.text = score.ToString();

        playerScoreBoardPanel.SetPlayerScore(newValue);

        if(!NetworkServer.active) { return; }

        if (score >= SteamLobby.Instance.scoreGoal)
            GameManager.Instance.StopGame();
    }

    private void ClientHandlePlayerReadyUpdated(bool oldValue, bool newValue)
    {
        playerScoreBoardPanel.SetPlayerReadyStatus(newValue);

        if (hasAuthority)
            UIManager.Instance.ChangeReadyUpButton(newValue);
    }

    private void HandleSteamIdUpdated(ulong oldSteamId, ulong newSteamid)
    {
        var CSteamId = new CSteamID(newSteamid);

        playerName = SteamFriends.GetFriendPersonaName(CSteamId);
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) { return; }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    private void AuthorityHandleLobbyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) { return; }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    //

    public bool GetIsLobbyOwner()
    {
        return isLobbyOwner;
    }

    public string GetDisplayName()
    {
        return playerName;
    }

    // Commands

    [Command]
    public void CmdStartGame()
    {
        if (!isLobbyOwner) { return; }

        CustomNetworkManager.Instance.StartLobby();
    }

    [Command]
    public void CmdOccupyLobbySlot(int i)
    {
        lobbySlotOccupied = i;
    }

    [Command]
    public void CmdChooseColor(int i)
    {
        colourID = i;
    }

    [Command]
    public void CmdReadyButton()
    {
        isReady = !isReady;
        GameManager.Instance.ReadyCheck();
    }

    #endregion

    #region Server

    [Server]
    public void SetPartyOwner(bool state)
    {
        isLobbyOwner = state;
    }

    [Server]
    public void SetDisplayName(string displayname)
    {
        playerName = displayname;
    }

    #endregion

    [Server]
    public void SetSteamId(ulong steamid)
    {
        steamId = steamid;
    }
}
