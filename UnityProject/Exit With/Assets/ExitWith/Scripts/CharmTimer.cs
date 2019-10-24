using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharmTimer : MonoBehaviour
{
    [SerializeField] private Transform shortNeedle;
    [SerializeField] private Transform longNeedle;
    public TextAsset[] CharmTexts => charmTexts;
    [SerializeField] private TextAsset[] charmTexts;
    private const int HOUR_PER_STEP = 12;  //一時間あたりの行動回数
    public static readonly int[] STEP_TH = { HOUR_PER_STEP*2,HOUR_PER_STEP*5,HOUR_PER_STEP*6 };
    private Quaternion shortRotation;
    private Quaternion longRotation;
    public enum CharmDepth { none,normal,hard,dead }
    public CharmDepth NowDepth => nowDepth;
    private CharmDepth nowDepth = CharmDepth.none;

    private void Start()
    {
        PlayerState.TimeStep.Subscribe(OnStepChange);
        longRotation = Quaternion.Euler(0,0, -360 / HOUR_PER_STEP);
        shortRotation = Quaternion.Euler(0, 0, -360 / (HOUR_PER_STEP * 12));
    }

    public TextAsset GetText(CharmDepth depth)
    {
        switch (depth)
        {
            case CharmDepth.normal:
                return charmTexts[0];
            case CharmDepth.hard:
                return charmTexts[1];
            case CharmDepth.dead:
                return charmTexts[2];
            default:
                return null;
        }
    }

    private void OnStepChange(int step)
    {
        if (step == 0)
        {
            Debug.Log("NeedleMove!");
            shortNeedle.rotation = Quaternion.Euler(0,0,180); //6時スタート
            return;
        }
        /*
        shortNeedle.rotation *= shortRotation;
        longNeedle.rotation *= longRotation;
        */
        longNeedle.rotation = Quaternion.Euler(0, 0,step * -360/HOUR_PER_STEP);
        shortNeedle.rotation = Quaternion.Euler(0, 0,step * -360 / (HOUR_PER_STEP * 12 ) + 180);

        nowDepth = StepToCharmDepth(step);
    }

    public static CharmDepth StepToCharmDepth(int step)
    {
        if(step < STEP_TH[0])
        {
            return CharmDepth.none;
        }
        else if (step < STEP_TH[1])
        {
            return CharmDepth.normal;
        }
        else if(step < STEP_TH[2])
        {
            return CharmDepth.hard;
        }
        else
        {
            return CharmDepth.dead;
        }
    }
}
