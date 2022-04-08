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
    public PipeOpenings[] GetPipeOpenings => openingPoints;
    [SerializeField] private PipeEnum pipeType;
    public PipeEnum PipeType
    {
        get => pipeType;
        set { pipeType = value; }
    }

    // Pipe fill times
    [SerializeField] protected float fillAmount;
    [SerializeField] protected float fillProgressionRate;
    [SerializeField] protected bool fillComplete;

    [SerializeField] protected Image fillImage;
    [SerializeField] protected PipeOpenings entryPoint;
    public PipeOpenings GetEntryPoint => entryPoint;
    [SerializeField] protected PipeOpenings exitPoint;
    public PipeOpenings ExitPoint
    { 
        get => exitPoint;
        set { exitPoint = value; }
    }
    [SerializeField] private GridManager gridRef;
    // Start is called before the first frame update

    public void InitPipe(int x, int y, GridManager gridReference, PipeEnum type)
    {
        posX = x;
        posY = y;
        gridRef = gridReference;
        pipeType = type;
        fillComplete = false;

        fillProgressionRate = GameManager.Instance.fillProgressionRate;
        fillImage = transform.GetChild(0).GetComponent<Image>(); // first child is essentially the fill image
    }

    public void SetNewPipePosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public virtual void BeginFilling()
    {
        fillAmount = 0;
        pipeType = PipeEnum.PIPE_USED;
        StartCoroutine(ProgressFilling(GameManager.Instance.fillTime));
    }

    protected virtual IEnumerator ProgressFilling(float duration)
    {
        while (!fillComplete)
        {
            fillAmount += fillProgressionRate / duration;
            fillImage.fillAmount = fillAmount;
            yield return new WaitForSeconds(fillProgressionRate);
            if (fillAmount >= 1)
            {
                fillComplete = true;
                gridRef.CheckExit(this, posX, posY);    
            }
        }
    }

    /// <summary>
    /// pass in the opening of the preceding pipe, 
    /// This will not be called if we find a break in the structure
    /// </summary>
    /// <param name="_entryPoint"></param>
    public virtual void CheckOpenings(PipeOpenings precedingEntryPoint)
    {
        switch(precedingEntryPoint)
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
        BeginFilling(); 
    }

    //check fill origin and fill functionality depening on the fill type
    protected virtual void AssignFillMethod()
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

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (pipeType == PipeEnum.PIPE)
        {
            gridRef.SwapPipe(posX, posY);
        } else
        {
            Debug.Log("Invalid pipe to swap");
        }
    }
}
