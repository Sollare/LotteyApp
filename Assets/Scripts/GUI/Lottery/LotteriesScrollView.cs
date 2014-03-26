using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LotteriesScrollView : MonoBehaviour
{
    public List<LotteryItem> items;
    private UICenterOnChild _scrollCenterOnChild;

    public event DataExtensions.EventDelegate<LotteryItem> OnItemChanged;
    public event DataExtensions.EventDelegate<LotteryItem> OnItemDataUpdated;
    public event DataExtensions.EventDelegate<LotteryItem> OnSelectedItemDataUpdated;

    protected virtual void CallSelectedItemDataUpdated(LotteryItem value)
    {
        //Debug.LogWarning("Selected item is updated: " + (currentItem == value));
        if (currentItem != value) return;

        DataExtensions.EventDelegate<LotteryItem> handler = OnSelectedItemDataUpdated;
        if (handler != null) handler(value);
    }

    protected virtual void CallItemDataUpdated(LotteryItem value)
    {
        DataExtensions.EventDelegate<LotteryItem> handler = OnItemDataUpdated;
        if (handler != null) handler(value);
    }

    protected virtual void CallItemChanged(LotteryItem data)
    {
        Debug.Log("Item changed");

        DataExtensions.EventDelegate<LotteryItem> handler = OnItemChanged;
        if (handler != null) handler(data);
    }

    [SerializeField]
    private LotteryItem _currentItem;
    public LotteryItem currentItem
    {
        get { return _currentItem; }
        set
        {
            if (_currentItem != null)
                _currentItem.OnDataUpdated -= onSelectedItemDataUpdated;
            

            _currentItem = value;

            if (_currentItem != null)
            {
                _currentItem.OnDataUpdated += onSelectedItemDataUpdated;
                CallItemChanged(_currentItem);
            }

        }
    }

    private void onSelectedItemDataUpdated(LotteryItem value)
    {
        if(value != null)
            CallSelectedItemDataUpdated(value);
    }

    void Awake ()
	{
	    //items = gameObject.GetComponentsInChildren<LotteryItem>().ToList();
        _scrollCenterOnChild = GetComponent<UICenterOnChild>();

        _scrollCenterOnChild.onFinished += OnDragFinished;
        _scrollCenterOnChild.OnNewTargetToCenter += OnNewTargetToCenter;
	}

    private void OnNewTargetToCenter(GameObject target)
    {
        Debug.Log("New target " + target.name);

        var lotteryItem = target.GetComponent<LotteryItem>();

        if (lotteryItem != null)
        {
            currentItem = lotteryItem;
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
