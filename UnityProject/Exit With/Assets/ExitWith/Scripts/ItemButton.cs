using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ItemButton : MonoBehaviour, IComparable
{
    public int ItemID => item.ItemID;
    [SerializeField] private ItemAsset item;
    private Button button;
    private Text text;
    public IObservable<ItemAsset> OnPressedObservable => onPressedSubject;
    private Subject<ItemAsset> onPressedSubject = new Subject<ItemAsset>();

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        text = gameObject.GetComponentInChildren<Text>();

        button.onClick.AddListener(OnPressed);
        text.text = item.ItemName;
    }

    private void OnPressed()
    {
        onPressedSubject.OnNext(item);
    }

    public int CompareTo(object obj)
    {
        var i = obj as ItemButton;
        if (i == null)
            return 1;

        return item.ItemID.CompareTo(i.ItemID);
    }
}
