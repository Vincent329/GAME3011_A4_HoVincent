using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public float fillTime; // the duration it takes to fill
    public float fillProgressionRate;
    public float delayStartTime; // How long before the game initiates
    public bool gameStarted;

    public DifficultyEnum difficultyEnum;

    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject gamePanel;

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

    public void SwitchToMinigame()
    {
        InputManager.ToggleActionMap(InputManager.playerInputActions.Minigame);
    }

    public void ReturnToPlayer()
    {
        InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
    }

    public void DifficultyInitiate(DifficultyEnum difficulty)
    {
        StartWithDifficulty?.Invoke(difficulty);
    }

    public void InvokeTurnOffGame()
    {
        DeactivateTheGame?.Invoke();
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
