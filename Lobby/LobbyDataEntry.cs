using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
public class LobbyDataEntry : MonoBehaviour
{
    public CSteamID lobbyID;
    public string lobbyName;
    public Text lobbyNameText;
    public string numberofGames;

    public void SetLobbyData()
    {
        if(lobbyName == "")
        {
            lobbyNameText.text = "Lobby";
        }
        else
        {
            lobbyNameText.text = lobbyName;
        }
    }

    public void JoinLobby()
    {
        SteamLobby.Instance.JoinLobby(lobbyID);
    }
}
