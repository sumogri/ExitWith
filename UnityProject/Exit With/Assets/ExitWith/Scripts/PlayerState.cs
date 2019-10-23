﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class PlayerState
{
    public static ReactiveProperty<int> HP { get; private set; } = new ReactiveProperty<int>();
    public static int Place {
        get { return placeId; }
        set {
            onPlaceChangeSubject.OnNext(value);
            placeId = value;
        }
    }
    private static int placeId = 0;
    public static IObservable<int> OnPlaceChange => onPlaceChangeSubject;
    private static BehaviorSubject<int> onPlaceChangeSubject = new BehaviorSubject<int>(19);
    public static ReactiveCollection<int> Items { get; private set; } = new ReactiveCollection<int>();
    public static ReactiveProperty<bool> IsCharming { get; private set; } = new ReactiveProperty<bool>();
}
