using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextLogWindow : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private TMP_Text text;
    const float ADD_CONTENT_SIZE = 66;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetLogText(string text)
    {
        Vector2 contentSize = content.sizeDelta;
        contentSize.y += ADD_CONTENT_SIZE;
        content.sizeDelta = contentSize;
        this.text.text += text;
    }
}
