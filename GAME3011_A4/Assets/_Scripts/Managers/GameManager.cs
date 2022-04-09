using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [Header("Gameplay Values")]
    private float originalFillTime;
    public float fillTime; // the duration it takes to fill
    public float fillProgressionRate;
    public float delayStartTime; // How long before the game initiates
    public bool gameStarted;
    public int requiredPipesRemaining;


    public DifficultyEnum difficultyEnum;
    
    // ---------- Gameplay Panels -------------
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TextMeshProUGUI scoreText;

    // ---------- Events and Delegates ---------------
    public delegate void DifficultySet(DifficultyEnum difficulty);
    public event DifficultySet StartWithDifficulty;

    public delegate void DeactivateGame();
    public event DeactivateGame DeactivateTheGame;

    public delegate void WinGame();
    public event WinGame Win;

    public delegate void LoseGame();
    public event LoseGame Lose;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        gameStarted = false;
        originalFillTime = fillTime;
        StartWithDifficulty += ToggleGamePanel;
        Win += ToggleWinText;
        Lose += ToggleLoseText;
        DeactivateTheGame += TurnOffGamePanel;

    }

    private void OnDisable()
    {
        StartWithDifficulty -= ToggleGamePanel;
        Win -= ToggleWinText;
        Lose -= ToggleLoseText;
        DeactivateTheGame -= TurnOffGamePanel;
    }

    // functions to invoke delegates
    public void DifficultyInitiate(DifficultyEnum difficulty)
    {
        StartWithDifficulty?.Invoke(difficulty);
        UpdateScoreText();
    }
    public void UpdateScoreText()
    {
        scoreText.text = "Remaining Pipes: " + requiredPipesRemaining;
    }

    public void InvokeTurnOffGame()
    {
        DeactivateTheGame?.Invoke();
    }

    public void InvokeWin()
    {
        Win?.Invoke();
    }

    public void InvokeLose()
    {
        Lose?.Invoke();
    }

    public void ToggleInstructionPanel(bool inTrigger)
    {
        instructionPanel.SetActive(inTrigger);
    }
    private void ToggleGamePanel(DifficultyEnum difficultyCheck)
    {
        difficultyEnum = difficultyCheck;
        gamePanel.SetActive(true);
    }
    private void TurnOffGamePanel()
    {
        gamePanel.SetActive(false);
    }

    private void ToggleWinText()
    {

    }

    private void ToggleLoseText()
    {

    }
}
