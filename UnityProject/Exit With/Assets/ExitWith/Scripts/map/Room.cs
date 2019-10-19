using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Room : MonoBehaviour,IComparable
{
    public string Name { get { return name; } }
    [SerializeField] private string name;
    public int RoomId { get { return roomId; } }
    [SerializeField] private int roomId;
    public bool IsRitualable { get { return isRitualable; } }
    [SerializeField] private bool isRitualable;
    public TextAsset[] OnRoomText { get { return onRoomText; } }
    [SerializeField] private TextAsset[] onRoomText;
    public TextAsset[] OnFindText { get { return onFindText; } }
    [SerializeField] private TextAsset[] onFindText;
    public IObservable<Room> OnMoveTo { get { return onMoveTo; } }
    private Subject<Room> onMoveTo = new Subject<Room>();
    public Room[] Neighbors { get { return neighbors; } }
    [SerializeField] private Room[] neighbors;
    

    public void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Pressed);
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
