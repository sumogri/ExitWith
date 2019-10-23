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
    [SerializeField] private RectTransform sclloreContent;
    [SerializeField] private Text itemname;
    [SerializeField] private GameObject contentRoot;
    public IObservable<Unit> OnCloseWindow => onCloseWindowSubject;
    private Subject<Unit> onCloseWindowSubject = new Subject<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        Array.Sort(buttons);
        foreach(var b in buttons)
        {
            b.OnPressedObservable.Subscribe(OnPressed);
            b.gameObject.SetActive(false);
        }
        PlayerState.Items.ObserveAdd().Subscribe(e => OnGetItem(e.Value)).AddTo(gameObject);
    }

    public void Activate()
    {
        contentRoot.SetActive(true);
    }
    public void Activate(ItemAsset asset)
    {
        Activate();
        SetAsset(asset);
    }

    public void DisActivate()
    {
        contentRoot.SetActive(false);
        onCloseWindowSubject.OnNext(Unit.Default);
    }

    private void OnGetItem(int id)
    {
        buttons[id].gameObject.SetActive(true);
    }

    private void OnPressed(ItemAsset asset)
    {
        SetAsset(asset);
    }

    private void SetAsset(ItemAsset asset)
    {
        text.text = asset.DetailsText;
        Vector2 contetSize = sclloreContent.sizeDelta;
        contetSize.y = text.preferredHeight;
        sclloreContent.sizeDelta = contetSize;
        itemname.text = $"-{asset.ItemName}";
    }
}
