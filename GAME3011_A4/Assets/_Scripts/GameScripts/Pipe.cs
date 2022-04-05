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
    [SerializeField] private PipeOpenings startPoint;
    [SerializeField] private PipeOpenings endPoint;

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

    }

    private void AssignPossibleEntryPoints()
    {

    }
}
