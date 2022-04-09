using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonDifficulty : MonoBehaviour
{
    private Button buttonComp;
    [SerializeField] private DifficultyEnum difficulty;
    private void Start()
    {
        buttonComp = GetComponent<Button>();
        buttonComp.onClick.AddListener(SetDifficultyOnClick);
    }

    private void SetDifficultyOnClick()
    {
        GameManager.Instance.DifficultyInitiate(difficulty);
        GameManager.Instance.ToggleInstructionPanel(false);
    }
}
