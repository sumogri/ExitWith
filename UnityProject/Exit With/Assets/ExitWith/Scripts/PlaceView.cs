using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Async;

public class PlaceView : MonoBehaviour
{
    [SerializeField] private MapWindow map;
    [SerializeField] private Text placeText;
    [SerializeField] private Image placeImage;
    public IObservable<Unit> OnViewChanged { get { return onViewChanged; } } //画面切り替え終了通知
    private Subject<Unit> onViewChanged = new Subject<Unit>();
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerState.Plase.Subscribe(async i => await OnMoved(i));
    }

    private async UniTask OnMoved(int roomId)
    {
        placeText.text = "";
        Color color = Color.white;
        color.a = 1;
        float time = 0;
        float th = 0.5f;
        while (time < th)
        {
            time += Time.deltaTime;
            color.a = 1 - 1 * time / th;
            placeImage.color = color;
            await UniTask.DelayFrame(1);
        }
        placeText.text = map.Rooms[roomId].RoomName;
        placeImage.sprite = map.Rooms[roomId].OwnPlaceImage;

        color.a = 0;
        time = 0;
        while (time < th)
        {
            time += Time.deltaTime;
            color.a = 1 * time / th;
            placeImage.color = color;
            await UniTask.DelayFrame(1);
        }
        onViewChanged.OnNext(Unit.Default);
    }
}
