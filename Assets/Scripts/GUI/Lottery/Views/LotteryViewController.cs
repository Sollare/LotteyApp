using System;
using UnityEngine;
using System.Collections;

public class LotteryViewController : MonoBehaviour {

    public LotteryItemDetails DetailItem;
    public UIWidget HeaderTimerWidghet;
    public UIWidget HeaderLotteryNameWidghet;

    public static LotteryItem CurrentLotteryItem
    {
        get
        {
            return LotteriesScrollView.instance.currentItem;
        }
    }

    void Awake()
    {
        BetsController.instance.OnBetPerformed += BetPerformed;
    }

    private void BetPerformed(object sender, Bet bet)
    {
        DetailItem.UpdateInfo(CurrentLotteryItem.lotteryInstance);
        DetailItem.Show();
    }
}
