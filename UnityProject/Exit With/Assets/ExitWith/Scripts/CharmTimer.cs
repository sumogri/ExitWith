using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharmTimer : MonoBehaviour
{
    [SerializeField] private Transform shortNeedle;
    [SerializeField] private Transform longNeedle;
    [SerializeField] private TextAsset[] charmTexts;
    private const int HOUR_PER_STEP = 12;  //一時間あたりの行動回数
    private static readonly int[] STEP_TH = { HOUR_PER_STEP*2,HOUR_PER_STEP*4,HOUR_PER_STEP*6 };
    private Quaternion shortRotation;
    private Quaternion longRotation;

    private void Start()
    {
        PlayerState.TimeStep.Subscribe(OnStepChange);
        longRotation = Quaternion.Euler(0,0, -360 / HOUR_PER_STEP);
        shortRotation = Quaternion.Euler(0, 0, -360 / (HOUR_PER_STEP * HOUR_PER_STEP));
    }

    private void OnStepChange(int step)
    {
        if (step == 0)
        {
            Debug.Log("NeedleMove!");
            shortNeedle.rotation = Quaternion.Euler(0,0,180); //6時スタート
            return;
        }
        shortNeedle.rotation *= shortRotation;
        longNeedle.rotation *= longRotation;
    }
}
