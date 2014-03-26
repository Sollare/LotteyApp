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
        if (selectedItem == null) return;

        Debug.Log("New Target: " + selectedItem.name);

        if (selectedItem.Data.type == LotteryData.LotteryType.Instant)
            SetLotteryInfoPanelVisible(false);
        else
            SetLotteryInfoPanelVisible(true);

        MainLotteryTimer.SetExpirationDate(selectedItem.Data.expiration);
        MainLotteryTimer.StartTimer();
    }

    void SetLotteryInfoPanelVisible(bool value)
    {
        var alphaValue = value ? 1f : 0f;
        TweenAlpha.Begin(LotteryInfoWidget.gameObject, 0.25f, alphaValue);
    }
}
