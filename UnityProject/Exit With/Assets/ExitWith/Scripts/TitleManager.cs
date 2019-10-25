using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject onNormalUI;
    [SerializeField] private GameObject onTrueUI;
    [SerializeField] private TMP_Text textNormal;
    [SerializeField] private TMP_Text textTrue;
    private readonly string title = "Exit with ";
    private readonly string normalPuls = "...";
    private readonly string truePuls = "---";
    private int animationStep = 0;
    private float nowTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerState.IsTrueEnd)
        {
            onTrueUI.SetActive(true);
            onNormalUI.SetActive(false);
        }
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime >= 0.5)
        {
            textNormal.text = title;
            textTrue.text = title;
            for(int i = 0; i < animationStep; i++)
            {
                textNormal.text += normalPuls[i];
                textTrue.text += truePuls[i];
            }

            if (animationStep >= 3)
                animationStep = 0;
            else
                animationStep++;

            nowTime = 0;
        }
    }

    public void SceneChange()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }
}
