using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Async;
using TMPro;

public class TextWindow : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TMP_Text text;
    private TextAsset textAsset;
    private int cnt = 0;
    private bool nowFeeding = false; //現在テキスト送り中か
    private bool skipFeeding; //テキスト送りをスキップして即座に文を出す
    private const int MAX_ROWS = 3; //テキストで一度に出せる行数
    private const int FEED_DELAY = 10; //文字送りディレイ(ミリ秒)
    [SerializeField] private TextLogWindow logWindow;
    [SerializeField] private GameObject feedEndMark;
    public IObservable<Unit> OnFeedEnd { get { return onFeedEnd; } }
    private Subject<Unit> onFeedEnd = new Subject<Unit>();
    public IObservable<Unit> OnAssetEnd => onAssetEnd;
    private Subject<Unit> onAssetEnd = new Subject<Unit>();


    public void SetText(TextAsset asset)
    {
        textAsset = asset;
        cnt = 0;
        TextFeed();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TextFeed();
    }

    private void TextFeed()
    {
        if (textAsset == null)
            return;

        if (nowFeeding)
        {
            skipFeeding = true;
            return;
        }

        if (cnt >= textAsset.SplitedText.Length)
        {
            return;
        }

        feedEndMark.SetActive(false);
        nowFeeding = true;
        TextFeedAsync();
    }

    private async UniTask TextFeedAsync()
    {
        //TODO:制御文字の実装
        //たとえば、SEを鳴らしたいところに<SE ID=0>みたいに入れれるとそこを読んだときにSEが鳴る

        //文字送りで表示
        text.text = "";
        for (int j = 0; j < MAX_ROWS; j++)
        {
            if (cnt + j >= textAsset.SplitedText.Length)
                break;
            
            for (int i = 0; i < textAsset.SplitedText[cnt+j].Length && !skipFeeding; i++)
            {
                await UniTask.Delay(FEED_DELAY);
                text.text += textAsset.SplitedText[cnt+j][i];
            }
            text.text += '\n';
        }

        //全文を表示
        text.text = "";
        for(int j = 0; j < MAX_ROWS; j++) {
            if (cnt + j >= textAsset.SplitedText.Length)
                break;

            text.text += textAsset.SplitedText[cnt+j] + '\n';
        }

        skipFeeding = false;
        nowFeeding = false;
        cnt += MAX_ROWS;
        logWindow.SetLogText(text.text); //ログに流す        
        feedEndMark.SetActive(true);
        onFeedEnd.OnNext(Unit.Default);

        if (cnt >= textAsset.SplitedText.Length)
            onAssetEnd.OnNext(Unit.Default);
    }
}
