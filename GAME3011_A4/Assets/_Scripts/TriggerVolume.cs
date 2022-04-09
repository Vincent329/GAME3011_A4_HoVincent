using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVolume : MonoBehaviour
{
    bool inGame = false;

    private void OnTriggerEnter(Collider other)
    {
        inGame = true;
        GameManager.Instance.ToggleInstructionPanel(inGame);
    }

    private void OnTriggerExit(Collider other)
    {
        inGame = false;
        GameManager.Instance.ToggleInstructionPanel(inGame);

    }
}
