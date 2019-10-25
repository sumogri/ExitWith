using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource seAudio;
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioClip[] se;
    [SerializeField] private float[] seVolumes;
    [SerializeField] private AudioClip[] bgm;
    [SerializeField] private float[] bgmVolumes;
    private int prePlayingBGM = int.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset.OnBGM.Subscribe(SetAndPlayBGM).AddTo(gameObject);
        TextAsset.OnSE.Subscribe(SetAndPlaySE).AddTo(gameObject);
    }

    private void SetAndPlayBGM(int i)
    {
        if (i >= bgm.Length) //番外なら止める
        {
            bgmAudio.Stop();
            prePlayingBGM = int.MaxValue;
            return;
        }
        else if(i == prePlayingBGM) //同じなら継続,なにもしない
        {
            return;
        }

        bgmAudio.clip = bgm[i];
        bgmAudio.volume = bgmVolumes[i];
        bgmAudio.Play();
        prePlayingBGM = i;
    }

    public void SetAndPlaySE(int i)
    {
        if (i >= se.Length) //番外なら止める
        {
            seAudio.Stop();
            return;
        }

        seAudio.clip = se[i];
        seAudio.volume = seVolumes[i];
        seAudio.Play();
    }
}
