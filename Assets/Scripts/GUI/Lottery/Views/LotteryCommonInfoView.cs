using System;
using UnityEngine;
using System.Collections;

public class LotteryCommonInfoView : MonoBehaviour {

    public LotteryTimer MainLotteryTimer;
    public UIWidget LotteryInfoWidget;

    private static LotteryTimer _mainTimer;
    public static LotteryTimer MainTimer
    {
        get { return _mainTimer; }
    }

    public LotteriesScrollView LotteriesScrollView;

	// Use this for initialization
	void Awake ()
	{
	    _mainTimer = MainLotteryTimer;

	    LotteriesScrollView.OnItemChanged += ItemChanged;
        LotteriesScrollView.OnSelectedItemDataUpdated += OnSelectedItemDataUpdated;
	}

    private void OnSelectedItemDataUpdated(LotteryItem selectedItem)
    {
        UpdateHeader(selectedItem);
    }

    private void ItemChanged(LotteryItem newItem)
    {
        UpdateHeader(newItem);
    }

    void UpdateHeader(LotteryItem selectedItem)
    {
        if (selectedItem == null || selectedItem.lotteryInstance == null) return;

        //Debug.Log("New Target: " + selectedItem.name);

        if (selectedItem.lotteryInstance.type == LotteryData.LotteryType.Instant)
            SetLotteryInfoPanelVisible(false);
        else
            SetLotteryInfoPanelVisible(true);

        MainLotteryTimer.SetExpirationTime(selectedItem.lotteryInstance.expiration);
        MainLotteryTimer.StartTimer();
    }

    void SetLotteryInfoPanelVisible(bool value)
    {
        var alphaValue = value ? 1f : 0f;
        TweenAlpha.Begin(LotteryInfoWidget.gameObject, 0.25f, alphaValue);
    }
}
