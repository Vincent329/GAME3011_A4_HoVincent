using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPipe : Pipe
{
    
    public override void CheckOpenings(PipeOpenings precedingEntryPoint)
    {
        switch (precedingEntryPoint)
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

    public override void BeginFilling()
    {
        fillAmount = 0;
        StartCoroutine(ProgressFilling(GameManager.Instance.fillTime));
    }

    protected override void AssignFillMethod()
    {
        base.AssignFillMethod();
    }

    protected override IEnumerator ProgressFilling(float duration)
    {
        while (!fillComplete)
        {
            fillAmount += fillProgressionRate / duration;
            fillImage.fillAmount = fillAmount;
            yield return new WaitForSeconds(fillProgressionRate);
            if (fillAmount >= 1)
            {
                fillComplete = true;

                if (GameManager.Instance.requiredPipesRemaining <= 0)
                {
                    GameManager.Instance.InvokeWin();
                } else
                {
                    GameManager.Instance.InvokeLose();
                }
                // set win condition here
            }
        }
    }

}
   
