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
        ShowLotteryDetails();
    }

    public void ShowLotteryDetails()
    {
        DetailItem.UpdateInfo(CurrentLotteryItem.lotteryInstance);
        DetailItem.Show();

        HeaderLotteryNameWidghet.cachedGameObject.SetActive(true);
        TweenAlpha.Begin(HeaderTimerWidghet.cachedGameObject, 0.1f, 0f);
        TweenAlpha.Begin(HeaderLotteryNameWidghet.cachedGameObject, 0.3f, 1f);

        TweenAlpha.Begin(ChooseLotteryLabel.cachedGameObject, 0.1f, 0f);
        TweenAlpha.Begin(LotteriesScrollView.instance.gameObject, 0.1f, 0f);

        EnableTabControl(false);
    }

    public void CloseLotteryDetails()
    {
        DetailItem.Hide();

        TicketsDropContainer.AbortPreparation();
        TweenAlpha.Begin(HeaderTimerWidghet.cachedGameObject, 0.1f, 1f);
        TweenAlpha.Begin(HeaderLotteryNameWidghet.cachedGameObject, 0.3f, 0f);

        TweenAlpha.Begin(ChooseLotteryLabel.cachedGameObject, 0.1f, 1f);
        TweenAlpha.Begin(LotteriesScrollView.instance.gameObject, 0.1f, 1f);

        EnableTabControl(true);
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
