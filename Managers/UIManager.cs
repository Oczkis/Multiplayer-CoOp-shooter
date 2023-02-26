using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    public Text readyUpButtonTxt;
    public Image buttonImage;
    public GameObject winnersPanel;
    public TMP_Text winnersName;

    public GameObject myPlayerPanel;
    public TMP_Text myHealth;
    public TMP_Text myScore;

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

    private void Start()
    {
        GameManager.ClientOnGameStarted += UIManagerClientOnGameStarted;
        GameManager.ClientOnGameStoped += UIManagerClientOnGameStoped;
    }

    private void OnDisable()
    {
        GameManager.ClientOnGameStarted -= UIManagerClientOnGameStarted;
        GameManager.ClientOnGameStoped -= UIManagerClientOnGameStoped;
    }

    private void UIManagerClientOnGameStarted()
    {
        buttonImage.gameObject.SetActive(false);
        myPlayerPanel.SetActive(true);
        HideWinner();
    }

    private void UIManagerClientOnGameStoped()
    {
        buttonImage.gameObject.SetActive(true);
        myPlayerPanel.SetActive(false);
        ShowWinner();
    }

    private void ShowWinner()
    {
        int score = -1;
        string playerName = "";

        foreach  (Player player in CustomNetworkManager.Instance.players)
        {
            if(player.score > score)
            {
                score = player.score;
                playerName = player.playerName;
            }
                
        }

        winnersName.text = playerName;

        winnersPanel.SetActive(true);
    }

    private void HideWinner()
    {
        winnersPanel.SetActive(false);
    }

    public void ChangeReadyUpButton(bool toggle)
    {
        buttonImage.color = toggle ? Color.blue : Color.green;
        readyUpButtonTxt.text = toggle ? "Stop!" : "Ready Up";
    }
}
