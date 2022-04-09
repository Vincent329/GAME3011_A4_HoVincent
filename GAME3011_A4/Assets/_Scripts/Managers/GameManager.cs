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
    public int slowdownPipesRemaining;


    public DifficultyEnum difficultyEnum;
    
    // ---------- Gameplay Panels -------------
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI DifficultyText;

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
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
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
        fillTime = originalFillTime;
        UpdateDifficultyText();
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Slowdown Pipes Remaining: " + slowdownPipesRemaining;
    }

    // Invoke events here
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

    private void UpdateDifficultyText()
    {
        DifficultyText.text = difficultyEnum.ToString();
    }
    private void ToggleGamePanel(DifficultyEnum difficultyCheck)
    {
        difficultyEnum = difficultyCheck;
        gamePanel.SetActive(true);
    }
    private void TurnOffGamePanel()
    {
        gamePanel.SetActive(false);
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
    }

    private void ToggleWinText()
    {
        Debug.Log("Win");
        winText.gameObject.SetActive(true);
    }

    private void ToggleLoseText()
    {
        Debug.Log("Lose");
        loseText.gameObject.SetActive(true);

    }
}
