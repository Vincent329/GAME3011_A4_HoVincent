using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateGameBoard : MonoBehaviour
{
    private Button buttonComp;
    private void Start()
    {
        buttonComp = GetComponent<Button>();
        buttonComp.onClick.AddListener(ShutOffGame);
    }

    private void ShutOffGame()
    {
        GameManager.Instance.InvokeTurnOffGame();
    }
}
