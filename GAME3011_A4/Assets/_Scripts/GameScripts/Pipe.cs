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
        switch(_entryPoint)
        {
            case PipeOpenings.UP:
                entryPoint = PipeOpenings.DOWN;
                break;
                
            case PipeOpenings.LEFT:
                entryPoint = PipeOpenings.RIGHT;
                break;
                
            case PipeOpenings.RIGHT:
                entryPoint = PipeOpenings.LEFT;
                break;
                
            case PipeOpenings.DOWN:
                entryPoint = PipeOpenings.UP;
                break;
        }
        AssignFillMethod();
    }

    private void AssignFillMethod()
    {
        // CHECK OPENING LOGIC HERE
        // depening on its start and end logic, it will dictate the logic in how the pipe is filled

        if (fillImage.fillMethod == Image.FillMethod.Horizontal || fillImage.fillMethod == Image.FillMethod.Vertical)
        {
            if (entryPoint == PipeOpenings.UP || entryPoint == PipeOpenings.RIGHT)
            {
                fillImage.fillOrigin = 1;
            }
            else
            {
                fillImage.fillOrigin = 0;
            }
        }
        else if (fillImage.fillMethod == Image.FillMethod.Radial90)
        {
            switch (fillImage.fillOrigin)
            {
                case 0: // BOTTOM LEFT
                    if (entryPoint == PipeOpenings.DOWN)
                    {
                        fillImage.fillClockwise = false;
                    }
                    else
                    {
                        fillImage.fillClockwise = true;
                    }
                    break;
                case 1: // TOP LEFT
                    if (entryPoint == PipeOpenings.LEFT)
                    {
                        fillImage.fillClockwise = false;
                    }
                    else
                    {
                        fillImage.fillClockwise = true;
                    }
                    break;
                case 2: // TOP RIGHT
                    if (entryPoint == PipeOpenings.UP)
                    {
                        fillImage.fillClockwise = false;
                    }
                    else
                    {
                        fillImage.fillClockwise = true;
                    }
                    break;
                case 3: // BOTTOM RIGHT
                    if (entryPoint == PipeOpenings.RIGHT)
                    {
                        fillImage.fillClockwise = false;
                    }
                    else
                    {
                        fillImage.fillClockwise = true;
                    }
                    break;
            }
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
