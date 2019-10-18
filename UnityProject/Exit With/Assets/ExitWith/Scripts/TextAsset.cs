using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 一連単位のテキストアセット
/// 
/// プレイヤーの行動と行動の間で1アセット,感覚としては節に近い?
/// e.g. 探索 => 描写(アセット) => 移動 => 描写(アセット) => ....
/// ここで2つの描写は、別々のアセットに入る
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/TextAsset")]
public class TextAsset : ScriptableObject
{
    [SerializeField, TextArea(1,100)] private string text;
    public string[] SplitedText;

    public void OnEnable()
    {
        SplitedText = text.Split('\n');
    }
}
