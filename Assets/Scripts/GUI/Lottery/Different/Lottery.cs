using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class Lottery 
{
    public enum LotteryType
    {
        Daily,
        Week,
        Instant
    }

    public Drawing Data;

    public LotteryType Type = LotteryType.Instant;

    public void FetchData()
    {
       
    }

    private static string GetUrlStringForLottery(Lottery lottery)
    {
        switch (lottery.Type)
        {
            case LotteryType.Daily:

                break;

            case LotteryType.Week:
                break;

            case LotteryType.Instant:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }
}
