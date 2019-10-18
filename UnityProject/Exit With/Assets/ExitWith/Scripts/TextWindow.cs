using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UniRx.Async;
using TMPro;

public class TextWindow : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TMP_Text text;
    private TextAsset textAsset;
    private int cnt = 0;
    private bool nowFeeding = false; //現在テキスト送り中か
    private bool skipFeeding; //テキスト送りをスキップして即座に文を出す

    public void SetText(TextAsset asset)
    {
        textAsset = asset;
        text.text = textAsset.SplitedText[0];
        cnt = 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TextFeed();
    }

    private void TextFeed()
    {
        if (nowFeeding)
        {
            skipFeeding = true;
            return;
        }

        if (cnt >= textAsset.SplitedText.Length)
        {
            return;
        }

        nowFeeding = true;
        TextFeedAsync();
    }

    private async UniTask TextFeedAsync()
    {
        text.text = "";
        for (int i = 0; i < textAsset.SplitedText[cnt].Length && !skipFeeding; i++)
        {
            await UniTask.Delay(1);
            text.text += textAsset.SplitedText[cnt][i];
        }
        text.text = textAsset.SplitedText[cnt];
        skipFeeding = false;
        nowFeeding = false;
        cnt++;
    }
}
