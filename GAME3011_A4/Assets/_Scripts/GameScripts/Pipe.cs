using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public enum PipeOpenings
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class Pipe : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int posX;
    [SerializeField] private int posY;
    
    [SerializeField] private PipeOpenings[] openingPoints;
    [SerializeField] private PipeEnum pipeType;

    [SerializeField] private Image fillImage;
    // Start is called before the first frame update
    void Start()
    {
        fillImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void InitPipe(int x, int y, PipeEnum type)
    {
        posX = x;
        posY = y;
        pipeType = type;
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

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Debug.Log("Pipe: " + posX + ", " + posY);
    }
}
