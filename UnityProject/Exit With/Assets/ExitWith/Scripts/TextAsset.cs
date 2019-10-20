using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public string[] SplitedText { get; private set; }
    private static Regex regex = new Regex("<[^<>]*>");

    public void OnEnable()
    {
        SplitedText = text.Split('\n');
    }

    /// <summary>
    /// テキストのi行目を読み、返す
    /// 行中にある制御文字は除去、対応するイベントを発火する
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public string ReadLine(int i)
    {
        //制御文字のパース
        //制御文字一覧
        //<SE [id]> id番目のSEを鳴らす
        //<ENEMY> 敵立ち絵を出す
        var tmp = SplitedText[i];
        var ms = regex.Matches(tmp);
        foreach(Match m in ms)
        {
            Debug.Log($"{m.Value},{m.Value.Contains("SE")},{m.Value.Contains("ENEMY")}");
            
        }

        return regex.Replace(SplitedText[i],"");
    }
}
