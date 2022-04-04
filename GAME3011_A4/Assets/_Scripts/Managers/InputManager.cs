using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInputData playerInputActions;
    public static event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        playerInputActions = new PlayerInputData();
    }


    // Start is called before the first frame update
    void Start()
    {
        ToggleActionMap(playerInputActions.Player);
    }

    public static void ToggleActionMap(InputActionMap inputMap)
    {
        Debug.Log("SwitchTo... " + inputMap);
        if (inputMap.enabled)
            return;

        playerInputActions.Disable();
        actionMapChange?.Invoke(inputMap);
        inputMap.Enable();
    }
}
