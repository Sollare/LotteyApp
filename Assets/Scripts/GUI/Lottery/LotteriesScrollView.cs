using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LotteriesScrollView : MonoBehaviour
{
    private List<LotteryItem> items;
    private UICenterOnChild _scrollCenterOnChild;

    public event EventHandler<LotteryData> onNewLotteryTarget;

    protected virtual void NewLotteryTarget(LotteryData data)
    {
        EventHandler<LotteryData> handler = onNewLotteryTarget;
        if (handler != null) handler(this, data);
    }

    void Awake ()
	{
	    items = gameObject.GetComponentsInChildren<LotteryItem>().ToList();
        _scrollCenterOnChild = GetComponent<UICenterOnChild>();

        _scrollCenterOnChild.onFinished += OnDragFinished;
        _scrollCenterOnChild.OnNewTargetToCenter += OnNewTargetToCenter;
	}

    private void OnNewTargetToCenter(GameObject target)
    {
        var lotteryItem = target.GetComponent<LotteryItem>();

        if (lotteryItem != null)
        {
            NewLotteryTarget(lotteryItem.Data);
        }
    }

    private void OnDragFinished()
    {
        //Debug.Log("Dragfinished");
    }

    void Update () 
    {
	
	}
}
