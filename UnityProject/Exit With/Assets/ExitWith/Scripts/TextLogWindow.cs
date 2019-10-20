using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextLogWindow : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TextMeshProUGUI ugui;

    public void SetLogText(string text)
    {
        this.text.text += text;
        Vector2 contentSize = content.sizeDelta;
        contentSize.y = this.text.preferredHeight; 
        content.sizeDelta = contentSize;
    }
}
