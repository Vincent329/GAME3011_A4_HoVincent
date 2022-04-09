using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVolume : MonoBehaviour
{
    bool inGame = false;

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.Instance.gameStarted)
        {
            inGame = false;
        }
        else
        {
            inGame = true;
        }
        GameManager.Instance.ToggleInstructionPanel(inGame);
    }

    

    private void OnTriggerExit(Collider other)
    {
        inGame = false;
        GameManager.Instance.ToggleInstructionPanel(inGame);

    }
}
