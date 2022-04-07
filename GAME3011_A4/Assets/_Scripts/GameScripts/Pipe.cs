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
    
    [SerializeField] private PipeOpenings[] openingPoints; // should only be two opening points
    [SerializeField] private PipeEnum pipeType;
    public PipeEnum PipeType
    {
        get => pipeType;
        set
        {
            pipeType = value;
        }
    }

    [SerializeField] private Image fillImage;

    [SerializeField] private PipeOpenings entryPoint;
    [SerializeField] private GridManager gridRef;
    // Start is called before the first frame update
    void Start()
    {
        fillImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void InitPipe(int x, int y, GridManager gridReference, PipeEnum type)
    {
        posX = x;
        posY = y;
        gridRef = gridReference;
        pipeType = type;
    }

    public void SetNewPipePosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Depending on the entry point, you'd call this function if it's the pipe to be entered
    /// This will not be called if we find a break in the structure
    /// </summary>
    /// <param name="_entryPoint"></param>
    private void CheckOpenings(PipeOpenings _entryPoint)
    {
        // CHECK OPENING LOGIC HERE
        // depening on its start and end logic, it will dictate the logic in how the pipe is filled

        switch(_entryPoint)
        {

        }
    }

    private void AssignPossibleEntryPoints()
    {

    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (pipeType == PipeEnum.PIPE)
        {
            gridRef.SwapPipe(posX, posY);
        }
    }
}
