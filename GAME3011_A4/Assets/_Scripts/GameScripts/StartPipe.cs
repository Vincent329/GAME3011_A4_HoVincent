using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPipe : Pipe
{
    private void Start()
    {
        fillProgressionRate = GameManager.Instance.fillProgressionRate;
        fillImage = transform.GetChild(0).GetComponent<Image>();
        CheckOpenings(GetPipeOpenings[0]); // start should only have one opening
        StartCoroutine(InitiateFill());
    }
    //private void OnEnable()
    //{
    //    fillProgressionRate = GameManager.Instance.fillProgressionRate;
    //    fillImage = transform.GetChild(0).GetComponent<Image>(); // first child is essentially the fill image
    //    StartCoroutine(InitiateFill());
    //}
    IEnumerator InitiateFill()
    {
        yield return new WaitForSeconds(GameManager.Instance.delayStartTime);
        BeginFilling();   
    }
    public override void BeginFilling()
    {
        fillAmount = 0;
        StartCoroutine(ProgressFilling(GameManager.Instance.fillTime));
    }

    public override void CheckOpenings(PipeOpenings start)
    {
        Debug.Log(start);
        switch (start)
        {
            case PipeOpenings.UP:
                exitPoint = PipeOpenings.UP;
                break;
            case PipeOpenings.DOWN:
                exitPoint = PipeOpenings.DOWN;
                break;
            case PipeOpenings.LEFT:
                exitPoint = PipeOpenings.LEFT;
                break;
            case PipeOpenings.RIGHT:
                exitPoint = PipeOpenings.RIGHT;
                break;
        }
    }

}
