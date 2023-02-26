using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    private static SteamLobby _instance;

    public static SteamLobby Instance { get { return _instance; } }

    public GameObject gameSettings;

    public bool isGameSettingsOpen;

    public static bool useSteam() => SteamManager.Initialized;

    private CustomNetworkManager networkManager;

    protected Callback<LobbyCreated_t> lobbyCreated;

    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;

    protected Callback<LobbyEnter_t> lobbyEntered;

    protected Callback<LobbyMatchList_t> lobbyList;
    protected Callback<LobbyDataUpdate_t> lobbyDataUpdated;

    public List<CSteamID> lobbyIDs = new List<CSteamID>();

    public static CSteamID LobbyId { get; private set; }

    private const string HostAddressKey = "HostAddress";

    [Header("Lobby Settings")]
    public int gameTimer;
    public int scoreGoal;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        networkManager = GetComponent<CustomNetworkManager>();

        if (!useSteam() || !SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        lobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    public void SettingsButton()
    {
        isGameSettingsOpen = !isGameSettingsOpen;
        gameSettings.SetActive(isGameSettingsOpen);
    }

    public void CloseGameSettings()
    {
        isGameSettingsOpen = false;
        gameSettings.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HostLobby()
    {
        if (useSteam())
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
            return;
        }

        networkManager.StartHost();
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            MainMenuManager.Instance.ShowMainMenuButtons();
            return;
        }

        networkManager.StartHost();

        LobbyId = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(LobbyId, HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(LobbyId, "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();

        MainMenuManager.Instance.HideMainMenuButtons();
    }

    public void JoinLobby(CSteamID lobbyID)
    {
        SteamMatchmaking.JoinLobby(lobbyID);
    }

    public void GetLobbiesList()
    {
        if (lobbyIDs.Count > 0) { lobbyIDs.Clear(); }

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        SteamMatchmaking.RequestLobbyList();
    }

    void OnGetLobbyList(LobbyMatchList_t result)
    {
        if (LobbyListManager.Instance.listOfLobbies.Count > 0) { LobbyListManager.Instance.DestroyLobbies(); }

        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
    }

    void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbyListManager.Instance.DisplayLobbies(lobbyIDs, result);
    }

    public int GetLobbyTimer()
    {
        if (SteamManager.Initialized)
            return Convert.ToInt32(SteamMatchmaking.GetLobbyData(LobbyId, "gameTimer"));

        return gameTimer;
    }

    public int GetLobbyScoreGoal()
    {
        if (SteamManager.Initialized)
            return Convert.ToInt32(SteamMatchmaking.GetLobbyData(LobbyId, "scoreGoal"));

        return scoreGoal;
    }

    public void UpdateMyLobbyName(InputField inputField)
    {
        if (SteamManager.Initialized)
            SteamMatchmaking.SetLobbyData(LobbyId, "name", inputField.text);
    }

    public void UpdateMyLobbyGameTimer(Dropdown dropdown)
    {
        int value = dropdown.value + 1 * 60;

        if(SteamManager.Initialized)
            SteamMatchmaking.SetLobbyData(LobbyId, "gameTimer", value.ToString());

        gameTimer = value;
    }

    public void UpdateMyLobbyScoreGoal(Dropdown dropdown)
    {
        int value = dropdown.value + 1 * 5;

        if (SteamManager.Initialized)
            SteamMatchmaking.SetLobbyData(LobbyId, "scoreGoal", value.ToString());

        scoreGoal = value;
    }
}

