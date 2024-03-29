﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Room : MonoBehaviour, IComparable
{
    public string RoomName { get { return name; } }
    [SerializeField] private string name;
    public int RoomId { get { return roomId; } }
    [SerializeField] private int roomId;
    public bool IsRitualable { get { return isRitualable; } }
    [SerializeField] private bool isRitualable;
    public TextAsset[] OnEnterTexts { get { return onRoomText; } set { onRoomText = value; } }
    [SerializeField] private TextAsset[] onRoomText;
    public TextAsset[] OnFindTexts { get { return onFindText; } set { onFindText = value; } }
    [SerializeField] private TextAsset[] onFindText;
    public Sprite OwnPlaceImage { get { return placeImage; } }
    [SerializeField] private Sprite placeImage;
    public IObservable<Room> OnMoveTo { get { return onMoveTo; } }
    private Subject<Room> onMoveTo = new Subject<Room>();
    public Room[] Neighbors { get { return neighbors; } }
    [SerializeField] private Room[] neighbors;
    [SerializeField] private bool isOpenSpace = false; //開けた場所であるか、trueなら隣接すると名前が見える
    public bool IsOpenSpace => isOpenSpace;
    //自分のUI
    private Text roomNameText;
    private Image[] roomConnections;
    private Button ownButton;
    private Color cannotEnterColor = Color.black; //隣接箇所にEnterしてない=入室できない場合
    private Color nonVisitedColor = new Color(0.594f, 0.594f, 0.594f); //入室はできるけど、入ってない場合
    private Color visitedColor = Color.white; //入ったことのある場合
    public VisitState OwnVisitState => visitState;
    private VisitState visitState = VisitState.cannot;　//訪れたかどうか
    public enum VisitState { cannot, none, visit }
    public bool isFinded { get; set; } = false;
    public IObservable<Room> OnFind => onFindSubject;
    private Subject<Room> onFindSubject = new Subject<Room>();
    public ItemAsset GettableItem => gettableItem; //この部屋で手に入るアイテム
    [SerializeField] private ItemAsset gettableItem = null;
    public RoomLock Lock { get; private set; } = null;

    public void Awake()
    {
        ownButton = gameObject.GetComponent<Button>();
        ownButton.onClick.AddListener(Pressed);
        roomNameText = gameObject.GetComponentInChildren<Text>();
        roomConnections = gameObject.GetComponentsInChildren<Image>();
        Lock = gameObject.GetComponent<RoomLock>();
    }

    public void OnEnable()
    {
        //訪問状態によるUI初期化
        switch (visitState)
        {
            case VisitState.cannot:
                InitUIToCannotVisit();
                break;
            case VisitState.none:
                InitUIToNonVisit();
                break;
            case VisitState.visit:
                InitUIToVisit();
                break;
        }
    }

    /// <summary>
    /// 入室不可(未探索による)
    /// </summary>
    private void InitUIToCannotVisit()
    {
        roomNameText.text = "???";
        ownButton.interactable = false;
        foreach (var i in roomConnections)
        {
            i.color = cannotEnterColor;
        }
    }

    /// <summary>
    /// 入室可能(まだ入ってない)
    /// </summary>
    private void InitUIToNonVisit()
    {
        if (isOpenSpace)
            roomNameText.text = RoomName;
        else
            roomNameText.text = "???";
        ownButton.interactable = true;
        ownButton.image.color = nonVisitedColor;
        foreach (var i in roomConnections)
        {
            i.color = nonVisitedColor;
        }
    }

    /// <summary>
    /// 入室可能(入ったことがある)
    /// </summary>
    private void InitUIToVisit()
    {
        ownButton.interactable = true;
        roomNameText.text = RoomName;
        ownButton.image.color = visitedColor;
        foreach (var i in roomConnections)
        {
            i.color = visitedColor;
        }
    }

    /// <summary>
    /// 入室
    /// </summary>
    public void Enter()
    {
        foreach(var n in neighbors)
        {
            n.Enterable();
        }
        visitState = VisitState.visit;
    }

    /// <summary>
    /// 移動目標にできるようにする
    /// </summary>
    public void Enterable()
    {
        if (visitState != VisitState.cannot)
            return;

        visitState = VisitState.none;
    }

    /// <summary>
    /// 探索
    /// </summary>
    public void Find()
    {
        onFindSubject.OnNext(this);
        isFinded = true;
    }

    private void Pressed()
    {
        onMoveTo.OnNext(this);
    }

    public int CompareTo(object obj)
    {
        Room room = obj as Room;
        if (room == null)
            return 1;

        return roomId.CompareTo(room.RoomId);
    }
}