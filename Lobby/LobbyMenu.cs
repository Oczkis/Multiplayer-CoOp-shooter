using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    private static LobbyMenu _instance;

    public static LobbyMenu Instance { get { return _instance; } }
    [SerializeField] private TMP_InputField ipAddressInputField;
    [SerializeField] private Button joinLocalLobbyButton;
    [SerializeField] private GameObject startGameButton = null;
    [SerializeField] private LobbySlotPanel[] lobbySlotPanels = new LobbySlotPanel[0];
    [SerializeField] private Color[] colors = new Color[0];

    public static event Action<int> ClientOnColorClicked;
    public static event Action<int> ClientOnLobbySlotClicked;

    public SteamManager steamManager;

    [SerializeField] InputField lobbySettingsLobbyNameInputField;
    [SerializeField] Dropdown lobbySettingsGameTimerDropdown;
    [SerializeField] Dropdown lobbySettingsScoreGoalDropdown;

    #region Unity Initializations
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        CustomNetworkManager.ClientOnConnected += HandleClientConnected;        
        CustomNetworkManager.ClientOnDisconnected += HandleClientDisconnected;

        Player.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        Player.ClientOnInfoUpdated += ClientHandleInfoUpdated;
        Player.ClientOnNameUpdated += HandleClientNameUpdated;
        Player.ClientOnChosenColorUpdated += HandleClientColorUpdated;
    }

    private void OnDisable()
    {
        CustomNetworkManager.ClientOnConnected -= HandleClientConnected;
        CustomNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;

        Player.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        Player.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
        Player.ClientOnNameUpdated -= HandleClientNameUpdated;
        Player.ClientOnChosenColorUpdated -= HandleClientColorUpdated;
    }

    #endregion

    #region Client
    // Handlers

    private void HandleClientConnected()
    {
        MainMenuManager.Instance.ShowLobbyUI();
        MainMenuManager.Instance.HideLobbyList();
        MainMenuManager.Instance.HideMainMenuButtons();
        joinLocalLobbyButton.interactable = true;
    }

    private void HandleClientDisconnected()
    {
        MainMenuManager.Instance.HideLobbyUI();
        MainMenuManager.Instance.ShowMainMenuButtons();
        joinLocalLobbyButton.interactable = true;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        startGameButton.SetActive(state);
    }

    private void HandleClientNameUpdated(int slot, string name)
    {
        if (slot >= 0)
            lobbySlotPanels[slot].playerName.text = name;
    }

    private void HandleClientColorUpdated(int slot, int color)
    {
        if(slot >= 0 && color >= 0)
            lobbySlotPanels[slot].chosenColor.color = colors[color];
    }

    private void ClientHandleInfoUpdated()
    {
        List<Player> serverPlayers = CustomNetworkManager.Instance.players;

        for (int i = 0; i < lobbySlotPanels.Length; i++)
        {
            lobbySlotPanels[i].isSlotEmpty = true;
        }

        foreach (Player player in serverPlayers)
        {
            if(player.lobbySlotOccupied >= 0)
                lobbySlotPanels[player.lobbySlotOccupied].isSlotEmpty = false;
        }


        for (int i = 0; i < lobbySlotPanels.Length; i++)
        {
            if (lobbySlotPanels[i].isSlotEmpty)
            {
                lobbySlotPanels[i].playerName.text = "Open";

                lobbySlotPanels[i].Open();
            }
            else
            {
                foreach (Player player in serverPlayers)
                {
                    if (player.lobbySlotOccupied == i)
                    {
                        lobbySlotPanels[i].Occupy();

                        lobbySlotPanels[i].playerName.text = player.GetDisplayName();

                        if(player.colourID >= 0)
                            lobbySlotPanels[i].chosenColor.color = colors[player.colourID];
                    }                   
                }
            }
        }
    }

    public void SetPanelsInteractable(int index)
    {
        foreach (LobbySlotPanel lobbySlot in lobbySlotPanels)
        {
            lobbySlot.SetInteractable(lobbySlot.slotIndex == index);
        }
    }

    // Clicks

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            CustomNetworkManager.Instance.StopHost();
        }
        else
        {
            CustomNetworkManager.Instance.StopClient();         
        }

        MainMenuManager.Instance.HideLobbyUI();
    }

    public void HostSteamLobby()
    {
        steamManager.enabled = true;
        CustomNetworkManager.Instance.GetComponent<SteamLobby>().HostLobby();
    }

    public void HostLocalLobby()
    {
        steamManager.enabled = false;
        CustomNetworkManager.Instance.StartHost();
    }

    public void JoinLocalLobby()
    {
        steamManager.enabled = false;
        CustomNetworkManager.Instance.networkAddress = ipAddressInputField.text;
        CustomNetworkManager.Instance.StartClient();
        joinLocalLobbyButton.interactable = false;
    }

    public void StartGame()
    {
        CustomNetworkManager.Instance.StartLobby();
    }

    public void ClickedOnLobbySlotPanel(int slotIndex)
    {
        ClientOnLobbySlotClicked?.Invoke(slotIndex);
    }
    public void ChooseColor(int i)
    {
        ClientOnColorClicked?.Invoke(i);
    }

    #endregion

    public int FindEmptySlot()
    {
        for (int i = 0; i < lobbySlotPanels.Length; i++)
        {
            if (lobbySlotPanels[i].isSlotEmpty)
            {
                return i;
            }
        }

        return 0;
    }

    public int FindAvailableColor()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            bool isAvailable = true;

            foreach (Player player in CustomNetworkManager.Instance.players)
            {
                if (player.colourID == i)
                {
                    isAvailable = false;
                    continue;
                }
            }

            if (isAvailable == true)
                return i;
        }
        return 0;
    }
}
