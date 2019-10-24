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
    [SerializeField] private Sprite[] textImages; //テキストから制御される部屋画像
    [SerializeField] private string[] textName; //テキストから制御される部屋名
    [SerializeField] private GameObject[] damageObjs; //テキストから制御されるダメージエフェクト

    // Start is called before the first frame update
    void Start()
    {
        PlayerState.OnPlaceChange.Subscribe(async i => await OnMoved(i));
        TextAsset.OnRoom.Subscribe(async i => await ChangePlace(textImages[i],textName[i])).AddTo(gameObject);
        TextAsset.OnDamage.Subscribe(i => damageObjs[i].SetActive(true)).AddTo(gameObject);
    }

    private async UniTask OnMoved(int roomId)
    {
        await UniTask.DelayFrame(1);
        await ChangePlace(map.Rooms[roomId].OwnPlaceImage, map.Rooms[roomId].RoomName);
    }

    private async UniTask ChangePlace(Sprite newImage,string newName)
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
        placeText.text = newName;
        placeImage.sprite = newImage;

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
