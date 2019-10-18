using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class TextWindow : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TMP_Text text;
    private TextAsset textAsset;
    private int cnt = 0;

    public void SetText(TextAsset asset)
    {
        textAsset = asset;
        text.text = textAsset.SplitedText[0];
        cnt = 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cnt >= textAsset.SplitedText.Length)
            return;

        text.text = textAsset.SplitedText[cnt];
        cnt++;
    }
}
