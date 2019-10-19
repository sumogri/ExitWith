using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextWindow textWindow;
    [SerializeField] private TextAsset textAsset;
    [SerializeField] private Room defaultRoom;

    // Start is called before the first frame update
    void Start()
    {
        textWindow.SetText(textAsset);
        defaultRoom.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
