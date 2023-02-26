using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class LobbyListManager : MonoBehaviour
{
    private static LobbyListManager _instance;

    public static LobbyListManager Instance { get { return _instance; } }

    public GameObject lobbiesMenu;
    public GameObject lobbyDataItemPrefab;
    public GameObject lobbyListContent;

    public Button lobbiesButton;

    public List<GameObject> listOfLobbies = new List<GameObject>();

    [Header("Filters")]
    public bool applyNameFilter;
    public bool applyScoreGoalFilter;
    public bool applyGameTimerFilter;
    public int filterScoreGoal;
    public int filterGameTimer;
    public string filterLobbyName;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void RefreshLobbyList()
    {
        DestroyLobbies();
        GetListOfLobbies();
    }

    public void GetListOfLobbies()
    {
        lobbiesButton.interactable = false;
       
        SteamLobby.Instance.GetLobbiesList();
        MainMenuManager.Instance.LoadLobbyList();
    }

    public void DestroyLobbies()
    {
        foreach (GameObject lobbyItem in listOfLobbies)
        {
            Destroy(lobbyItem);
        }
        listOfLobbies.Clear();
    }

    public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIDs.Count; i++)
        {
            if (lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                
                string lobbyname = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");

                string lobbyScoreGoal = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "scoreGoal");

                string lobbyGameTimer = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "gameTimer");


                if(!lobbyname.Contains(filterLobbyName) && filterLobbyName != "" && applyNameFilter) { return; }

                if (filterScoreGoal.ToString() != lobbyScoreGoal && applyScoreGoalFilter) { return; }

                if (lobbyGameTimer != filterGameTimer.ToString() && applyGameTimerFilter) { return; }


                GameObject createdItem = Instantiate(lobbyDataItemPrefab);

                LobbyDataEntry lobbyData = createdItem.GetComponent<LobbyDataEntry>();

                lobbyData.lobbyID = (CSteamID)lobbyIDs[i].m_SteamID;

                lobbyData.lobbyName = lobbyname;

                lobbyData.numberofGames = lobbyScoreGoal;

                lobbyData.SetLobbyData();
              
                createdItem.transform.SetParent(lobbyListContent.transform);
                createdItem.transform.localScale = Vector3.one;
               
                listOfLobbies.Add(createdItem);
            }
        }
    }
}
