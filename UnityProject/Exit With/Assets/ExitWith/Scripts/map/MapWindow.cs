using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MapWindow : MonoBehaviour
{
    public Room[] Rooms { get { return rooms; } }
    [SerializeField] private Room[] rooms;
    public IObservable<Room> OnMoveTo { get { return onMoveTo; } }
    private Subject<Room> onMoveTo = new Subject<Room>();
    [SerializeField] private GameObject playerIcon;

    [SerializeField] private Text floorText; //階表示
    [SerializeField] private Button floor1Button;
    [SerializeField] private Button floor2Button;
    [SerializeField] private Button floorB1Button;
    [SerializeField] private GameObject[] floor1Objes;
    [SerializeField] private GameObject[] floor2Objes;
    [SerializeField] private GameObject[] floorB1Objes;
    private GameObject[][] floorObjes = new GameObject[3][];
    private readonly string[] FLOOR_TEXT = { "-B1","-1F","-2F" };
    private int nowActiveFloor = 2;
    [SerializeField] private GameObject contentRoot;

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
        onMoveTo.Subscribe(r => Debug.Log(r.RoomName));
        //

        floorObjes[0] = floorB1Objes;
        floorObjes[1] = floor1Objes;
        floorObjes[2] = floor2Objes;
        floor1Button.onClick.AddListener(() => FloorChange(1));
        floor2Button.onClick.AddListener(() => FloorChange(2));
        floorB1Button.onClick.AddListener(() => FloorChange(0));
    }

    private void FloorChange(int to)
    {
        foreach(var o in floorObjes[nowActiveFloor])
        {
            o.SetActive(false);
        }
        foreach (var o in floorObjes[to])
        {
            o.SetActive(true);
        }
        floorText.text = FLOOR_TEXT[to];
        nowActiveFloor = to;
    }

    /// <summary>
    /// 目的地設定完了通知
    /// </summary>
    /// <param name="room">目的地</param>
    private void MoveTo(Room room)
    {
        //もし同じ場所に移動しようとしてるなら、無視
        if (PlayerState.Plase.Value == room.RoomId)
            return;

        onMoveTo.OnNext(room);
        contentRoot.SetActive(false);

        //test
        //OnMoved(room.RoomId);
    }

    /// <summary>
    /// 移動完了したら
    /// </summary>
    /// <param name="roomId">移動先</param>
    private void OnMoved(int roomId)
    {
        playerIcon.transform.position = rooms[roomId].gameObject.transform.position;
        playerIcon.transform.SetParent(rooms[roomId].gameObject.transform);
        rooms[roomId].Enter();

        //階段なら
        if (roomId == 12)
        {
            floor2Button.gameObject.SetActive(true);
            floor1Button.gameObject.SetActive(true);
        }
    }
}
