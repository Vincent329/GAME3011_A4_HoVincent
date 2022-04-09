using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpButton : MonoBehaviour
{
    private Button buttonComp;
    private void Start()
    {
        buttonComp = GetComponent<Button>();
        buttonComp.onClick.AddListener(SpeedUp);
    }

    private void SpeedUp()
    {
        GameManager.Instance.fillTime = 0.05f;
    }
}
