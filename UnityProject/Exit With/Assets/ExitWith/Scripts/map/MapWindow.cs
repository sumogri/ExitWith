using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class MapWindow : MonoBehaviour
{
    public Room[] Rooms { get { return rooms; } }
    [SerializeField] private Room[] rooms;
    public IObservable<Room> OnMoveTo { get { return onMoveTo; } }
    private Subject<Room> onMoveTo = new Subject<Room>();

    [SerializeField] private GameObject playerIcon;

    // Start is called before the first frame update
    void Start()
    {
        Array.Sort(rooms);

        foreach(var r in rooms)
        {
            r.OnMoveTo.Subscribe(MoveTo);
        }
        PlayerState.Plase.Subscribe(OnMoved);

        //test
        onMoveTo.Subscribe(r => Debug.Log(r.Name));
    }

    /// <summary>
    /// 目的地設定完了通知
    /// </summary>
    /// <param name="room">目的地</param>
    private void MoveTo(Room room)
    {
        onMoveTo.OnNext(room);
        //test
        OnMoved(room.RoomId);
    }

    /// <summary>
    /// 移動完了したら
    /// </summary>
    /// <param name="roomId">移動先</param>
    private void OnMoved(int roomId)
    {
        playerIcon.transform.position = rooms[roomId].gameObject.transform.position;
    }
}
