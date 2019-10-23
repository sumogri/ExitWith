using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemAsset")]
public class ItemAsset : ScriptableObject
{
    public string DetailsText => text;
    [SerializeField, TextArea(1, 100)] private string text;
    public string ItemName => itemName;
    [SerializeField] private string itemName;
    public TextAsset ReactionText => reactionText;
    [SerializeField] private TextAsset reactionText;

    public int ItemID => itemID;
    [SerializeField] int itemID;

    public TextAsset BattleText => battleText;
    [SerializeField] private TextAsset battleText;//戦闘時に使用したときのリアクション
}
