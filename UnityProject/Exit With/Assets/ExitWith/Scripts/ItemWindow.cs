using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ItemWindow : MonoBehaviour
{    
    [SerializeField] private ItemButton[] buttons;
    [SerializeField] private Text text;
    [SerializeField] private RectTransform content;
    [SerializeField] private Text itemname;

    // Start is called before the first frame update
    void Start()
    {
        Array.Sort(buttons);
        foreach(var b in buttons)
        {
            b.OnPressedObservable.Subscribe(OnPressed);
            b.gameObject.SetActive(false);
        }
        PlayerState.Items.ObserveAdd().Subscribe(e => OnGetItem(e.Value));
    }

    private void OnGetItem(int id)
    {
        buttons[id].gameObject.SetActive(true);
    }

    private void OnPressed(ItemAsset asset)
    {
        text.text = asset.DetailsText;
        Vector2 contetSize = content.sizeDelta;
        contetSize.y = text.preferredHeight;
        content.sizeDelta = contetSize;
        itemname.text = $"-{asset.ItemName}";
    }
}
