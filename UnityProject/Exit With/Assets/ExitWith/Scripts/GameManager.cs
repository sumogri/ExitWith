using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextWindow textWindow;
    [SerializeField] private TextAsset textAsset;
    [SerializeField] private Room defaultRoom;
    [SerializeField] private MapWindow mapWindow;

    // Start is called before the first frame update
    void Start()
    {
        textWindow.SetText(textAsset);
        defaultRoom.Enter();
        mapWindow.OnMoveTo.Subscribe(OnMoveTo);
    }

    private void OnMoveTo(Room room)
    {
        PlayerState.Plase.Value = room.RoomId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
