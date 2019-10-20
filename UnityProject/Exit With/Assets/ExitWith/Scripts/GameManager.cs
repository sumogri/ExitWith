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
            textWindow.OnAssetEnd.Subscribe(__ => {
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

        room.Find();

        //テキスト流し予約
        textWindow.SetText(text);
        textWindow.OnAssetEnd.Subscribe(_ => {
            actionWindow.ActionActivate();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
