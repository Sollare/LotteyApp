using System;
using UnityEngine;
using System.Collections;

public class LotteryCommonInfoView : MonoBehaviour {

    public LotteryTimer MainLotteryTimer;
    public UIWidget LotteryInfoWidget;

    public LotteriesScrollView LotteriesScrollView;

	// Use this for initialization
	void Awake () 
    {
	    LotteriesScrollView.onNewLotteryTarget += NewLotteryTarget;
	}

    private void NewLotteryTarget(object sender, LotteryData lotteryData)
    {
        if (lotteryData == null) return;

        Debug.Log("New Target: " + lotteryData.type);

        if (lotteryData.type == LotteryData.LotteryType.Instant)
            SetLotteryInfoPanelVisible(false);
        else
            SetLotteryInfoPanelVisible(true);

        MainLotteryTimer.SetExpirationDate(lotteryData.expiration);
        MainLotteryTimer.StartTimer();
    }

    void SetLotteryInfoPanelVisible(bool value)
    {
        var alphaValue = value ? 1f : 0f;
        TweenAlpha.Begin(LotteryInfoWidget.gameObject, 0.25f, alphaValue);
    }
}
