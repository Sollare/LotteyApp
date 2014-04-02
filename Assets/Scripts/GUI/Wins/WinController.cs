using System;
using UnityEngine;
using System.Collections;


public class WinController : MonoBehaviour
{
    public event EventHandler<Lottery> OnLotteryLost;
    public event EventHandler<Lottery> OnLotteryWinner;

    public virtual void CallLotteryLost(Lottery e)
    {
        EventHandler<Lottery> handler = OnLotteryLost;
        if (handler != null) handler(this, e);
    }

    public virtual void CallLotteryWinner(Lottery e)
    {
        EventHandler<Lottery> handler = OnLotteryWinner;
        if (handler != null) handler(this, e);
    }

    private static WinController _instance;

    public static WinController instance
    {
        get
        {
            if (_instance != null) return _instance;
            else
                return (_instance = GameObject.Find("LotteryController").AddMissingComponent<WinController>());
        }
    }

    void Awake()
    {
        LotteryController.instance.Model.OnLotteryFinished += LotteryCompleted;
    }

    private void LotteryCompleted(object sender, Lottery lottery)
    {
        
    }
}
