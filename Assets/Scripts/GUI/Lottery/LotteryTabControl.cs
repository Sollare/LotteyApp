using System;
using UnityEngine;
using System.Collections;

public class LotteryTabControl : MonoBehaviour
{
    public UISprite[] Tabs;
    public LotteriesScrollView Lotteries;

    private const string markerOff = "marker_off";
    private const string markerOn = "marker_on";

    private UISprite activeTab;

    void Awake()
    {
        Lotteries.OnItemChanged += OnScrollViewItemChanged;
    }

    private void OnScrollViewItemChanged(LotteryItem item)
    {
        if(activeTab)
            SetTabSelected(activeTab, false);

        activeTab = Tabs[(int) item.LoadLotteryOfType];
        SetTabSelected(activeTab, true);
    }

    void SetTabSelected(UISprite activeSprite, bool selected)
    {
        activeTab.spriteName = selected ? markerOn : markerOff;
    }
}
