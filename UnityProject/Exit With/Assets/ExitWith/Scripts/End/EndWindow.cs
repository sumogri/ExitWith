﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using naichilab;
public class EndWindow : MonoBehaviour
{
    [SerializeField] private GameObject contentShar;
    [SerializeField] private GameObject contentBad1;
    [SerializeField] private GameObject contentBad2;
    [SerializeField] private GameObject contentNormal;
    [SerializeField] private GameObject contentGood;
    [SerializeField] private GameObject contentTrue;
    [SerializeField] private Button twieetButton;
    [SerializeField] private Button toTitle;
    [SerializeField] private SoundManager sound;
    public enum EndKind { bad1,bad2,normal,good,truee,none}
    private EndKind nowKind = EndKind.none;

    private void Start()
    {
        twieetButton.onClick.AddListener(OnTwieet);
        toTitle.onClick.AddListener(ToTitle);
    }

    private void ToTitle()
    {
        sound.SetAndPlaySE(14); //ボタン音

        PlayerState.InitState();
        SceneManager.LoadScene("Title");
    }

    private void OnTwieet()
    {
        sound.SetAndPlaySE(14); //ボタン音

        var endname = "BadEnd1";
        switch (nowKind)
        {
            case EndKind.bad2:
                endname = "BadEnd2";
                break;
            case EndKind.good:
                endname = "GoodEnd";
                break;
            case EndKind.normal:
                endname = "NormalEnd";
                break;
            case EndKind.truee:
                endname = "TrueEnd";
                break;
        }
        var text = $"{endname}に到達!";
        UnityRoomTweet.Tweet("exitwith", text, "unityroom", "unity1week", "Exit_With");
    }

    public void ActivateEndWindow(EndKind kind)
    {
        nowKind = kind;
        var content = contentBad1;
        switch (kind)
        {
            case EndKind.bad2:
                content = contentBad2;
                break;
            case EndKind.good:
                content = contentGood;
                break;
            case EndKind.normal:
                content = contentNormal;
                break;
            case EndKind.truee:
                content = contentTrue;
                break;
        }
        content.SetActive(true);
        contentShar.SetActive(true);
    }
}
