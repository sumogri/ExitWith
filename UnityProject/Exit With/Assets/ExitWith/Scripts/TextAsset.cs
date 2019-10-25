using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UniRx;

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
    private static Regex paramRegex = new Regex("[0-9]+");
    //パースした制御文字に対応するsubject群
    public static IObservable<string> OnControle => onControlReadSubject;
    private static Subject<string> onControlReadSubject = new Subject<string>();
    public static IObservable<bool> OnZonbi => onZonbiEnterSubject; //t=on ,f=off
    private static Subject<bool> onZonbiEnterSubject = new Subject<bool>();
    public static IObservable<int> OnRoom => onRoomSubject;
    private static Subject<int> onRoomSubject = new Subject<int>();
    public static IObservable<int> OnDamage => onDamageSubject;
    private static Subject<int> onDamageSubject = new Subject<int>();
    public static IObservable<int> OnBGM => onbgmSubject;
    private static Subject<int> onbgmSubject = new Subject<int>();
    public static IObservable<int> OnSE => onseSubject;
    private static Subject<int> onseSubject = new Subject<int>();
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
        //<ZONBION> 敵立ち絵を出す
        var tmp = SplitedText[i];
        var ms = regex.Matches(tmp);
        foreach(Match m in ms)
        {
            Debug.Log($"{m.Value}");
            if (m.Value.Contains("ZONBION"))
            {
                onZonbiEnterSubject.OnNext(true);
            }
            else if (m.Value.Contains("ZONBIOFF"))
            {
                onZonbiEnterSubject.OnNext(false);
            }
            else if (m.Value.Contains("ROOM"))
            {
                var id = int.Parse(paramRegex.Match(m.Value).Value);
                Debug.Log(id);
                onRoomSubject.OnNext(id);
            }
            else if (m.Value.Contains("DAMAGE"))
            {
                var id = int.Parse(paramRegex.Match(m.Value).Value);
                Debug.Log(id);
                onDamageSubject.OnNext(id);
            }
            else if (m.Value.Contains("BGM"))
            {
                var id = int.Parse(paramRegex.Match(m.Value).Value);
                Debug.Log(id);
                onbgmSubject.OnNext(id);

            }
            else if (m.Value.Contains("SE"))
            {
                var id = int.Parse(paramRegex.Match(m.Value).Value);
                Debug.Log(id);
                onseSubject.OnNext(id);
            }
            onControlReadSubject.OnNext(m.Value);
        }

        return regex.Replace(SplitedText[i],"");
    }
}
