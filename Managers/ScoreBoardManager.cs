using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    private static ScoreBoardManager _instance;

    public static ScoreBoardManager Instance { get { return _instance; } }

    [SerializeField] private PlayerPanel[] playerPanels;

    CanvasGroup canvasGroup;

    public bool isScoreBoardHiding, isScoreBoardShowing, isScoreBoardVisible;

    public float transitionSpeed;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void FixedUpdate()
    {
        if (isScoreBoardHiding)
        {
            float visibility = Mathf.MoveTowards(canvasGroup.alpha, 0, transitionSpeed * Time.deltaTime);
            canvasGroup.alpha = visibility;

            if(canvasGroup.alpha <= 0.1f)
            {
                HideScoreBoard();
            }
        }

        if(isScoreBoardShowing)
        {
            float visibility = Mathf.Lerp(canvasGroup.alpha, 1, transitionSpeed * Time.deltaTime);
            canvasGroup.alpha = visibility;

            if (canvasGroup.alpha >= 0.9f)
            {
                ShowScoreBoard();
            }
        }
    }

    public void Start()
    {
        GameManager.ClientOnGameStarted += ScoreBoardClientOnGameStarted;
        GameManager.ClientOnGameStoped += ScoreBoardClientOnGameStoped;
    }

    public void OnDisable()
    {
        GameManager.ClientOnGameStarted -= ScoreBoardClientOnGameStarted;
        GameManager.ClientOnGameStoped -= ScoreBoardClientOnGameStoped;
    }

    public void ScoreBoardClientOnGameStarted()
    {
        foreach (PlayerPanel playerPanel in playerPanels)
        {
            playerPanel.TogglePlayerReadyStatusPanel(false);
        }

        isScoreBoardHiding = true;
    }

    public void ScoreBoardClientOnGameStoped()
    {
        foreach (PlayerPanel playerPanel in playerPanels)
        {
            playerPanel.TogglePlayerReadyStatusPanel(true);
        }

        isScoreBoardShowing = true;
    }

    public void ReadyUpButton()
    {
        PlayerManager.Instance.myPlayer.CmdReadyButton();
    }

    public void InitializeScoreBoard()
    {
        foreach (Player player in CustomNetworkManager.Instance.players)
        {
            PlayerPanel panel = GetFirstNonActivePlayerPanel();
            player.playerScoreBoardPanel = panel;
            panel.InitializePanel(player.playerName, player.colourID);
        }
    }

    public PlayerPanel GetFirstNonActivePlayerPanel()
    {
        foreach (PlayerPanel playerPanel in playerPanels)
        {
            if(!playerPanel.gameObject.activeSelf)
                return playerPanel;
        }

        return playerPanels[0];
    }

    public void ToggleScoreBoard()
    {
        if(!GameManager.Instance.isGameInProgress || isScoreBoardHiding || isScoreBoardShowing) { return; }

        isScoreBoardHiding = isScoreBoardVisible;
        isScoreBoardShowing = !isScoreBoardVisible;
    }

    private void HideScoreBoard()
    {
        isScoreBoardVisible = false;
        isScoreBoardHiding = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowScoreBoard()
    {
        isScoreBoardVisible = true;
        isScoreBoardShowing = false;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
