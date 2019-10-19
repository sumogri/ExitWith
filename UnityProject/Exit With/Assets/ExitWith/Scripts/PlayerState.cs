using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class PlayerState
{
    public static ReactiveProperty<int> HP { get; set; } = new ReactiveProperty<int>();
    public static ReactiveProperty<int> Plase { get; set; } = new ReactiveProperty<int>();
    public static List<int> Items { get; set; } = new List<int>();
}
