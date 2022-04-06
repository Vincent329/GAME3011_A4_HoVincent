using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum PipeOpenings
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class Pipe : MonoBehaviour
{
    [SerializeField] private PipeOpenings[] openingPoints;
    [SerializeField] private PipeEnum pipeType;

    [SerializeField] private Image fillImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckOpenings()
    {
        // CHECK OPENING LOGIC HERE
        // depening on its start and end logic, it will dictate the logic in how the pipe is filled
    }

    private void AssignPossibleEntryPoints()
    {

    }
}
