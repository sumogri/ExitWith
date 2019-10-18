using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObject/TextAsset")]
public class TextAsset : ScriptableObject
{
    [SerializeField, TextArea] private string text;
    public string[] SplitedText;

    public void OnEnable()
    {
        SplitedText = text.Split('\n');
    }
}
