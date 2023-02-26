using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    private static CustomNetworkManager _instance;

    public static CustomNetworkManager Instance { get { return _instance; } }
   
    [SerializeField] public List<Player> players { get; } = new List<Player>();

    [SerializeField] private int minNumberOfPlayers;

    [SerializeField] private GameObject gameManager = null;

    public static event Action ClientOnDisconnected;
    public static event Action ClientOnConnected;   

    private bool isGameInProgress = false;

    [SerializeField] private Transport steamTransport;
    [SerializeField] private Transport localTransport;

    public override void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        transport = SteamLobby.useSteam() ? steamTransport : localTransport;
    }

    #region Client

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        ClientOnDisconnected?.Invoke();
    }

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    #endregion

    #region Server
    public override void OnStopServer()
    {
        base.OnStopServer();

        players.Clear();

        isGameInProgress = false;
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (!isGameInProgress) { return; }

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Player player = conn.identity.GetComponent<Player>();

        players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        Player player = conn.identity.GetComponent<Player>();

        if (SteamLobby.useSteam())
        {
            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, players.Count);

            player.SetSteamId(steamID.m_SteamID);
        }
        else
        {
            player.SetDisplayName($"Player {players.Count + 1}");
        }

        player.playerID = players.Count;

        player.SetPartyOwner(players.Count == 1);

        player.lobbySlotOccupied = LobbyMenu.Instance.FindEmptySlot();
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName == "MainGame")
        {
            GameObject gameManagerGO = Instantiate(gameManager, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(gameManagerGO);
        }
    }

    #endregion

    #region LobbyRelated

    #endregion

    #region GameRelated
    public void StartLobby()
    {
        if (players.Count < minNumberOfPlayers) { return; }

        isGameInProgress = true;

        ServerChangeScene("MainGame");
    }

    public void LeaveGame()
    {
        if (NetworkClient.isConnected)
        {
            StopClient();
        }

        if (NetworkServer.active)
        {
            StopServer();
        }
    }

    #endregion
}
