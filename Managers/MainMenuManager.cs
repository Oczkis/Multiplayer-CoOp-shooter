using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager _instance;

    public static MainMenuManager Instance { get { return _instance; } }

    [SerializeField] private GameObject lobbyList;
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject enterAdressPanel;
    [SerializeField] private GameObject landingPageButtons;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {                     
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }          
    }

    //Buttons
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowMainMenuButtons()
    {
        landingPageButtons.SetActive(true);
        enterAdressPanel.SetActive(true);
    }

    public void HideMainMenuButtons()
    {
        landingPageButtons.SetActive(false);
        enterAdressPanel.SetActive(false);
    }   
    //Show

    public void ShowLobbyUI()
    {
        lobbyUI.SetActive(true);
    }

    public void LoadLobbyList()
    {
        lobbyList.SetActive(true);
    }

    //Hide

    public void HideLobbyUI()
    {
        lobbyUI.SetActive(true);
    }

    public void HideLobbyList()
    {
        lobbyList.SetActive(false);
    }
}
