using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLock : MonoBehaviour
{
    public TextAsset OnLockedText {
        get {
            if(nowTextNum < onLockedTexts.Length)
                nowTextNum++;

            return onLockedTexts[nowTextNum-1];
        }
    }
    [SerializeField] private TextAsset[] onLockedTexts;
    private int nowTextNum = 0;
    public Room FrontRoom => frontRoom;
    [SerializeField] private Room frontRoom;
    private enum Locktype { key,charm }
    [SerializeField] private Locktype locktype;
    private bool isLocked = false;

    public bool IsLocked()
    {
        bool locked = false;

        switch (locktype)
        {
            case Locktype.key:
                locked = !PlayerState.Items.Contains(9);
                break;
            case Locktype.charm:
                locked = !PlayerState.IsCharming.Value;
                break;
        }

        return locked;
    }
}
