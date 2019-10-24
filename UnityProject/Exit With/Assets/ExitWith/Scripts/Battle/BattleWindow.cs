using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class BattleWindow : MonoBehaviour
{
    [SerializeField] private GameObject contentRoot;
    [SerializeField] private GameObject itemButtonRoot;
    private ItemButton[] itemButtons;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text lifeView;
    [SerializeField] private Image damageEff;
    [SerializeField] private Image zonbi;
    private const int winableItemId = 4; //使うと勝つアイテムID
    public bool IsWon { get; private set; } = false;
    public Room BattleRoom => battleRoom;
    [SerializeField] private Room battleRoom;
    [SerializeField] private TextAsset[] onWonRoomEnterText;
    [SerializeField] private TextAsset[] onWonRoomFindText;
    [SerializeField] private MapWindow map;
    public IObservable<ItemAsset> OnPressButtonObservable => onPressButtonSubject;
    private Subject<ItemAsset> onPressButtonSubject = new Subject<ItemAsset>();
    public ItemAsset WinDropItem => winDrop;
    [SerializeField] private ItemAsset winDrop;

    // Start is called before the first frame update
    void Start()
    {
        itemButtons = itemButtonRoot.GetComponentsInChildren<ItemButton>();
        foreach(var i in itemButtons)
        {
            i.OnPressedObservable.Subscribe(OnPressItemButton);
        }
        exitButton.onClick.AddListener(() => {
            contentRoot.SetActive(false);
            onPressButtonSubject.OnNext(null);
        });
        InitRoomTexts();
        PlayerState.HP.Subscribe(hp => {
            lifeView.text = "";
            for (int i = 0; i < hp; i++)
            {
                lifeView.text += "♥";
            }
        }).AddTo(gameObject);
        TextAsset.OnZonbi.Subscribe(b => zonbi.gameObject.SetActive(b)).AddTo(gameObject);
        PlayerState.OnPlaceChange.Where(i => i != battleRoom.RoomId)
            .Subscribe(_ => zonbi.gameObject.SetActive(false)).AddTo(gameObject);
    }

    private void OnPressItemButton(ItemAsset asset)
    {
        contentRoot.SetActive(false);

        if (winableItemId == asset.ItemID)
        {
            IsWon = true;
            InitRoomTexts();
        }
        else
        {
            //ダメージ
            PlayerState.HP.Value--;
        }
        onPressButtonSubject.OnNext(asset);
    }

    public void Activate()
    {
        contentRoot.SetActive(true);
        foreach(var i in itemButtons)
        {
            i.gameObject.SetActive(PlayerState.Items.Contains(i.ItemID));
        }
    }

    private void InitRoomTexts()
    {
        if (IsWon)
        {
            battleRoom.OnEnterTexts = onWonRoomEnterText;
            battleRoom.OnFindTexts = onWonRoomFindText;
            battleRoom.isFinded = false;
        }
    }
}
