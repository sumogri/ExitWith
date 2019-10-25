using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 行動の選択肢ウィンドウ
/// </summary>
public class ActionWindow : MonoBehaviour
{
    [SerializeField] private GameObject actButtons;
    [SerializeField] private Button find;
    [SerializeField] private Button move;
    [SerializeField] private Button item;
    [SerializeField] private Button ritual;
    [SerializeField] private GameObject yesNoButtons;
    [SerializeField] private Button yes;
    [SerializeField] private Button no;
    [SerializeField] private ItemWindow itemWindow;
    [SerializeField] private MapWindow mapWindow;
    [SerializeField] private TextWindow textWindow;
    [SerializeField] private SoundManager sound;
    
    public IObservable<Unit> OnRetual => onRitualSubject;
    private Subject<Unit> onRitualSubject = new Subject<Unit>();
    public IObservable<bool> OnFinalAnswer => onPressFinalQuestionSubject;
    private Subject<bool> onPressFinalQuestionSubject = new Subject<bool>();

    // Start is called before the first frame update
    void Start()
    {
        find.onClick.AddListener(() => {
            ActionDisActivate();
            sound.SetAndPlaySE(14); //ボタン音
        });
        move.onClick.AddListener(OnClickMoveButton);
        item.onClick.AddListener(OnClickItemButton);
        ritual.onClick.AddListener(OnClickRitualButton);
        PlayerState.Items.ObserveAdd().First(x => x.Value == 8 && PlayerState.IsCharming.Value)
            .Subscribe(_ => ritual.gameObject.SetActive(true))
            .AddTo(gameObject);
        PlayerState.IsCharming.First(b => !b)
            .Subscribe(b => ritual.gameObject.SetActive(false))
            .AddTo(gameObject);
        yes.onClick.AddListener(() => {
            onPressFinalQuestionSubject.OnNext(true);
            yesNoButtons.SetActive(false);
            sound.SetAndPlaySE(14); //ボタン音
        });
        no.onClick.AddListener(() => {
            onPressFinalQuestionSubject.OnNext(false);
            yesNoButtons.SetActive(false);
            sound.SetAndPlaySE(14); //ボタン音
        });
    }

    private void OnClickRitualButton()
    {
        onRitualSubject.OnNext(Unit.Default);
        ActionDisActivate();
        sound.SetAndPlaySE(14); //ボタン音
    }

    private void OnClickMoveButton()
    {
        mapWindow.OnCloseWindow.First().Subscribe(r => {
            if (r == null)
            {
                ActionActivate();
            }
            else if(PlayerState.IsEnd) //エンディングにたどりついてないなら
            {
                textWindow.OnAssetEnd.First().Subscribe(__ =>
                {
                    ActionActivate();
                });
            }
        });
        mapWindow.Activate();
        ActionDisActivate();
        sound.SetAndPlaySE(14); //ボタン音
    }

    private void OnClickItemButton()
    {
        itemWindow.OnCloseWindow.First().Subscribe(_ => ActionActivate());
        itemWindow.Activate();
        ActionDisActivate();
        sound.SetAndPlaySE(14); //ボタン音
    }

    public void ActionActivate()
    {
        actButtons.SetActive(true);
    }
    public void ActionDisActivate()
    {
        actButtons.SetActive(false);
    }

    public void YesNoActivate()
    {
        yesNoButtons.SetActive(true);
    }

    public void ritualActivate()
    {
        ritual.gameObject.SetActive(true);
    }
}
