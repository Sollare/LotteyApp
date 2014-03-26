﻿using UnityEngine;
using System.Collections;

public class LotteryController : MonoBehaviour
{
    private LotteryModel _model;

    public LotteryModel Model
    {
        get
        {
            if(_model == null)
                _model = new LotteryModel();

            return _model;
        }
    }

    private static LotteryController _instance;

    public static LotteryController instance
    {
        get
        {
            if (_instance != null) return _instance;
            else
                return (_instance = GameObject.Find("LotteryController").GetComponent<LotteryController>());
        }
    }

	// Use this for initialization
    void Awake()
    {
        
        _instance = this;
    }

    public void FetchLottery(LotteryData.LotteryType lotteryType, WWWOperations.OnObjectFecthed<LotteryData> callback)
    {
        Model.FetchLottery(lotteryType, callback);
    }
}