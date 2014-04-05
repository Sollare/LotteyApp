using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class LotteryItemDetails : MonoBehaviour 
{
    public UIWidget _widget;

    public UILabel TimeOver;
    public UILabel TotalPrize;
    public UILabel Bets;

    public LotteryData lotteryData;

    protected UIWidget cachedUIWidget
    {
        get
        {
            if (_widget) return _widget;
            else return (_widget = GetComponent<UIWidget>());
        }
    }

    void Awake()
    {
        LotteryController.instance.Model.OnLotteryUpdate += LotteryUpdated;
        LotteriesScrollView.instance.OnItemChanged += ItemChanged;
        LotteriesScrollView.instance.OnSelectedItemDataUpdated += SelectedItemDataUpdated;
    }

    private void SelectedItemDataUpdated(LotteryItem value)
    {
        lotteryData = value.lotteryInstance;
    }

    private void ItemChanged(LotteryItem value)
    {
        lotteryData = value.lotteryInstance;
    }

    private void LotteryUpdated(object sender, Lottery lottery)
    {
        if (lotteryData.id == lottery.Data.id)
        {
            UpdateInfo(lottery.Data);
        }
    }

    public void UpdateInfo(LotteryData data)
    {
        if (data == null)
        {
            Debug.LogError("Cannot show null info value");
            return;
        }

        lotteryData = data;

        TimeOver.text = LotteryTimer.instance.ToString();
        var bets = 0;
        if (lotteryData.MyBets != null)
            bets = lotteryData.MyBets.Count();

        TotalPrize.text = string.Format("${0}.00", lotteryData.totalmoney);
        Bets.text = string.Format("you made {0} bet{1}", bets, bets <= 1 ? "s" : "");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Invoke("ShowC", Time.deltaTime);
    }

    private void ShowC()
    {
        LotteryTimer.instance.OnTimeStringChanged += TimerStringChanged;

        cachedUIWidget.alpha = 0f;
        var tweener = TweenAlpha.Begin(gameObject, 0.3f, 1f);
        tweener.onFinished.Clear();
    }

    public void Hide()
    {
        LotteryTimer.instance.OnTimeStringChanged -= TimerStringChanged;

        var tweener = TweenAlpha.Begin(gameObject, 0.3f, 0f);
        tweener.onFinished.Clear();
        tweener.AddOnFinished(delegate { gameObject.SetActive(false); Destroy(tweener);});
    }

    private void TimerStringChanged(string timeString)
    {
        TimeOver.text = LotteryTimer.instance.ToString();
    }
}
