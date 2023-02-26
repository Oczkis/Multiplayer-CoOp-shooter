using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text playerReadyStatus;
    [SerializeField] private GameObject playerReadyStatusPanel;
    [SerializeField] private Color[] colours;

    public void InitializePanel(string playerName, int colourId)
    {
        gameObject.SetActive(true);
        SetPlayerName(playerName);
        SetPlayerNameColour(colourId);
    }

    public void TogglePlayerReadyStatusPanel(bool toggle)
    {
        playerReadyStatusPanel.SetActive(toggle);
    }

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;       
    }

    public void SetPlayerNameColour(int colourID)
    {
        playerNameText.color = colours[colourID];
    }

    public void SetPlayerReadyStatus(bool status)
    {
        playerReadyStatus.text = status ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
    }

    public void SetPlayerScore(int score)
    {
        playerScoreText.text = "Score : " + score.ToString();
    }
}
