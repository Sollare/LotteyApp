using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class Lottery : EventArgs
{
    public LotteryData Data;

    public Lottery(LotteryData data)
    {
        Data = data;
    }
}
