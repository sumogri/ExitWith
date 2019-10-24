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
    [SerializeField] private BattleWindow battleWindow;

    // Start is called before the first frame update
    void Start()
    {
        //導入流し
        PlayerState.Place = defaultRoom.RoomId;
        mapWindow.OnMoveTo.Subscribe(Move);
        battleWindow.OnPressButtonObservable.Subscribe(i => {
            if (i == null)
                ExitBattle();
            else
                UseItem(i);
        });
        //最初の入場はUI外から来るので、ここでさばく
        placeView.OnViewChanged.First().Subscribe(_ => {
            textWindow.SetText(dounyuAsset);
            textWindow.OnAssetEnd.First().Subscribe(__ => {
                actionWindow.ActionActivate();
                PlayerState.Items.Add(0); //ケータイゲット
            });
        });
    }

    private void Move(Room room)
    {
        var text = room.OnEnterTexts[0];
        //もし入ったことがあるなら
        if (room.OwnVisitState == Room.VisitState.visit)
            text = room.OnEnterTexts[1];

        //カギがかかっていれば
        if (room.Lock != null && room.Lock.IsLocked())
        {
            text = room.Lock.OnLockedText;
            room = room.Lock.FrontRoom;
        }

        PlayerState.Place = room.RoomId; //入室,周り回ってEnterをコール    
        
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
        var room = mapWindow.Rooms[PlayerState.Place];
        var text = room.OnFindTexts[0];
        if (room.isFinded)
            text = room.OnFindTexts[1];

        if (battleWindow.IsWon && room == battleWindow.BattleRoom) //勝利後バトルルームだけ処理違い
        {
            if (IsItemGetable(room))
                text = room.OnFindTexts[1];
            else
                text = room.OnFindTexts[0];
        }

        textWindow.SetText(text);

        //アイテムを入手
        if (IsItemGetable(room)) //ここをいい感じに移譲すると入手条件いじれる
        {
            //拾う描写 => アイテムウィンドウ開く => 読後描写
            textWindow.OnAssetEnd.First().Subscribe(_ =>
            {
                GetItem(room.GettableItem);
            });
        }
        else if (!battleWindow.IsWon && room == battleWindow.BattleRoom)
        {
            textWindow.OnAssetEnd.First().Subscribe(_ =>
            {
                battleWindow.Activate();
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

    private void GetItem(ItemAsset item)
    {
        //拾う描写 => アイテムウィンドウ開く => 読後描写
        PlayerState.Items.Add(item.ItemID);
        itemWindow.Activate(item);
        itemWindow.OnCloseWindow.First().Subscribe(__ =>
        {
            if (item.ReactionText != null)
            {
                textWindow.SetText(item.ReactionText);
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
    }
    

    private void UseItem(ItemAsset item)
    {
        textWindow.SetText(item.BattleText);
        
        textWindow.OnAssetEnd.First().Subscribe(_ =>
        {
            if (battleWindow.IsWon)
            {
                GetItem(battleWindow.WinDropItem);
            }
            else
            {
                battleWindow.Activate();
            }
            
        });
    }
    private void ExitBattle()
    {
        //バトルから逃げ出す
        mapWindow.Activate(); //マップ開いて
        mapWindow.OnCloseWindow.First().Subscribe(x => {
            if (x == null)
                battleWindow.Activate(); //マップが閉じた時、どこにも行かないなら戦闘続行
        });
    }

    private bool IsItemGetable(Room room)
    {
        //血濡れの客室は別条件
        if (room.RoomId == 22)
            return PlayerState.Items.Contains(8) && !PlayerState.Items.Contains(10); //レポート所持&血未所持

        return room.GettableItem != null && !room.isFinded;
    }
}
