using System;
using UnityEngine;
using System.Collections;

public class LotteryViewController : MonoBehaviour {

    public LotteryItemDetails DetailItem;
    public UIWidget HeaderTimerWidghet;
    public UIWidget HeaderLotteryNameWidghet;
    public LotteryDropContainer TicketsDropContainer;
    public UILabel ChooseLotteryLabel;

    public UIPanel CoverPanel;
    public UIWidget TabControl;

    public OverScreenPanel OverScreen;

    public ConfirmPanel ConfirmPanel;

    private static LotteryViewController _instance;

    public static LotteryViewController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("LotteryController").GetComponent<LotteryViewController>();

            return _instance;
        }
    }

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

        WinController.instance.OnLotteryWinner += OnWin;
        WinController.instance.OnLotteryLost += OnLost;
    }

    private void OnLost(object sender, Lottery lottery)
    {
        LotteryViewController.instance.OverScreen.SetResult(false);
        LotteryViewController.instance.OverScreen.SetVisible(true);
    }

    private void OnWin(object sender, Lottery lottery)
    {
        LotteryViewController.instance.OverScreen.SetResult(true);
        LotteryViewController.instance.OverScreen.SetVisible(true);
    }

    private void BetPerformed(object sender, Bet bet)
    {
        if(CurrentLotteryItem.LoadLotteryOfType != LotteryData.LotteryType.Instant)
            ShowLotteryDetails();
        else
        {
            ShowLotteryNameInHeader(true);
            EnableLotteriesScrollView(false);
        }
    }

    public void ShowLotteryNameInHeader(bool value)
    {
        HeaderLotteryNameWidghet.cachedGameObject.SetActive(true);

        TweenAlpha.Begin(HeaderTimerWidghet.cachedGameObject, 0.1f, value ? 0f : 1f);
        TweenAlpha.Begin(HeaderLotteryNameWidghet.cachedGameObject, 0.3f, value ? 1f : 0f);
    }

    public void ShowLotteryDetails()
    {
        DetailItem.UpdateInfo(LotteryController.instance.Model.GetLottery(CurrentLotteryItem.lotteryInstance.id).Data);
        DetailItem.Show();

        ShowLotteryNameInHeader(true);
        EnableLotteriesScrollView(false);
        EnableTabControl(false);
    }

    public void CloseLotteryDetails()
    {
        DetailItem.Hide();

        TicketsDropContainer.AbortPreparation();
        ShowLotteryNameInHeader(false);

        EnableLotteriesScrollView(true);

        EnableTabControl(true);
    }

    public void EnableLotteriesScrollView(bool show)
    {
        TweenAlpha.Begin(ChooseLotteryLabel.cachedGameObject, 0.1f, show ? 1f : 0f);
        TweenAlpha.Begin(LotteriesScrollView.instance.gameObject, 0.1f, show ? 1f : 0f);
    }

    public void EnableCoverPanel(bool show)
    {
        CoverPanel.alpha = show ? 0f : 1f;

        if(show)
            CoverPanel.gameObject.SetActive(true);

        var tweener = TweenAlpha.Begin(CoverPanel.gameObject, 0.2f, show ? 1f : 0f);
        tweener.onFinished.Clear();

        if (!show) // По окончанию сокрытия
        tweener.onFinished.Add(new EventDelegate(delegate
        {
            if(!show) // По окончанию сокрытия
                CoverPanel.gameObject.SetActive(false);
        }));
    }

    public void EnableTabControl(bool show)
    {
        TweenAlpha.Begin(TabControl.gameObject, 0.2f, show ? 1f : 0f);
    }
}
