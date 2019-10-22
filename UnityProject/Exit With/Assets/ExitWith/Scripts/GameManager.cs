using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// シナリオの流れを司る
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextWindow textWindow;
    [SerializeField] private TextAsset dounyuAsset; //導入部テキスト
    [SerializeField] private Room defaultRoom;
    [SerializeField] private MapWindow mapWindow;
    [SerializeField] private PlaceView placeView;
    [SerializeField] private ActionWindow actionWindow;
    [SerializeField] private ItemWindow itemWindow;

    // Start is called before the first frame update
    void Start()
    {
        //導入流し
        defaultRoom.Enter();
        mapWindow.OnMoveTo.Subscribe(OnMoveTo);
        placeView.OnViewChanged.First().Subscribe(_ => {
            textWindow.SetText(dounyuAsset);
            textWindow.OnAssetEnd.First().Subscribe(__ => {
                actionWindow.ActionActivate();
                PlayerState.Items.Add(0); //ケータイゲット
            });
        });
    }

    private void OnMoveTo(Room room)
    {
        var text = room.OnEnterTexts[0];
        //もし入ったことがあるなら
        if (room.OwnVisitState == Room.VisitState.visit)
            text = room.OnEnterTexts[1];

        PlayerState.Plase.Value = room.RoomId; //入室,周り回ってEnterをコール    
        
        //テキスト流し予約
        placeView.OnViewChanged.First().Subscribe(_ => {
            textWindow.SetText(text);
            textWindow.OnAssetEnd.First().Subscribe(__ => {
                actionWindow.ActionActivate();
            });
        });
    }

    public void Find()
    {
        var room = mapWindow.Rooms[PlayerState.Plase.Value];
        var text = room.OnFindTexts[0];
        if (room.isFinded)
            text = room.OnFindTexts[1];

        //テキスト流し予約
        textWindow.SetText(text);

        //アイテムを入手
        if (room.GettableItem != null && !room.isFinded)
        {
            //拾う描写 => アイテムウィンドウ開く => 読後描写
            PlayerState.Items.Add(room.GettableItem.ItemID);
            textWindow.OnAssetEnd.First().Subscribe(_ =>
            {
                itemWindow.Activate(room.GettableItem);
                itemWindow.OnCloseWindow.First().Subscribe(__ =>
                {
                    if (room.GettableItem.ReactionText != null)
                    {
                        textWindow.SetText(room.GettableItem.ReactionText);
                        textWindow.OnAssetEnd.First().Subscribe(___ =>
                        {
                            actionWindow.ActionActivate();
                        });
                    }
                    else
                    {
                        actionWindow.ActionActivate();
                    }
                });
            });
        }
        else
        {
            textWindow.OnAssetEnd.First().Subscribe(_ =>
            {
                actionWindow.ActionActivate();
            });
        }

        room.Find();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
