using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyFilters : MonoBehaviour
{
    [SerializeField] private InputField searchLobbyName;
    [SerializeField] private Dropdown searchLobbyScoreGoal;
    [SerializeField] private Dropdown searchLobbyGameTimer;

    [SerializeField] private GameObject filterNameCheckMark;
    [SerializeField] private GameObject filterScoreCheckMark;
    [SerializeField] private GameObject filterTimerCheckMark;

    public void OnFilterLobbyNameChanged()
    {
        LobbyListManager.Instance.filterLobbyName = searchLobbyName.text;
        LobbyListManager.Instance.RefreshLobbyList();
    }

    public void OnFilterLobbyScoreGoalChanged()
    {
        LobbyListManager.Instance.filterScoreGoal = searchLobbyScoreGoal.value;
        LobbyListManager.Instance.RefreshLobbyList();
    }

    public void OnFilterLobbyGameTimerChanged()
    {
        LobbyListManager.Instance.filterGameTimer = searchLobbyGameTimer.value;
        LobbyListManager.Instance.RefreshLobbyList();
    }

    public void ButtonEnableFilterName()
    {
        LobbyListManager.Instance.applyNameFilter = !LobbyListManager.Instance.applyNameFilter;
        filterNameCheckMark.SetActive(LobbyListManager.Instance.applyNameFilter);
        LobbyListManager.Instance.RefreshLobbyList();
    }

    public void ButtonEnableFilterScoreGoal()
    {
        LobbyListManager.Instance.applyScoreGoalFilter = !LobbyListManager.Instance.applyScoreGoalFilter;
        filterScoreCheckMark.SetActive(LobbyListManager.Instance.applyScoreGoalFilter);
        LobbyListManager.Instance.RefreshLobbyList();
    }

    public void ButtonEnableFilterGameTimer()
    {
        LobbyListManager.Instance.applyGameTimerFilter = !LobbyListManager.Instance.applyGameTimerFilter;
        filterTimerCheckMark.SetActive(LobbyListManager.Instance.applyGameTimerFilter);
        LobbyListManager.Instance.RefreshLobbyList();
    }
}
    
