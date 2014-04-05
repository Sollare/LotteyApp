using System;
using System.Collections.Generic;
using System.Linq;
using LotteyServerApp.Models;
using UnityEngine;
using System.Collections;

public class LotteryModel
{
    private List<Lottery> _lotteries = new List<Lottery>(3);

    public event EventHandler<Lottery> OnLotteryFinished;

    public virtual void CallLotteryCompleted(Lottery e)
    {
        EventHandler<Lottery> handler = OnLotteryFinished;
        if (handler != null) handler(this, e);
    }

    public event EventHandler<Lottery> OnLotteryUpdate;

    protected virtual void CallLotteryUpdated(Lottery e)
    {
        //Debug.Log("Лотерея " + e.Data.id + " была обновлена");

        EventHandler<Lottery> handler = OnLotteryUpdate;
        if (handler != null) handler(this, e);
    }

    public event EventHandler<Lottery> OnLotteryLoaded;

    protected virtual void CallLotteryLoaded(Lottery e)
    {
        EventHandler<Lottery> handler = OnLotteryLoaded;
        if (handler != null) handler(this, e);
    }
    
    public event EventHandler<Lottery> OnBetPerformed;

    protected virtual void BetBerformed(Lottery e)
    {
        EventHandler<Lottery> handler = OnBetPerformed;
        if (handler != null) handler(this, e);
    }

    public int LotteriesCount
    {
        get { return _lotteries.Count(); }
    }

    public IEnumerable<Lottery> Lotteries
    {
        get
        {
            return _lotteries;
        }
    }

    public Lottery this[int index]
    {
        get
        {
            if (index < 0 || index >= LotteriesCount)
                return null;

            return _lotteries[index];
        }
    }

    public LotteryModel()
    {
        _lotteries = new List<Lottery>();
    }

    public void FetchLottery(LotteryData.LotteryType lotteryType, WWWOperations.OnObjectFecthed<LotteryData> callback)
    {
        // Связанный коллбэк для того, чтобы сначала обработать здесь, а затем уже принять
        WWWOperations.instance.FetchJsonObject<LotteryData>(GetUrlStringForLottery(lotteryType), callback, FetLotteryChainedCallback);
    }

    private void FetLotteryChainedCallback(LotteryData fetchedData, string error, WWWOperations.OnObjectFecthed<LotteryData> callback)
    {
        var addResult = AddLottery(fetchedData);
        if (addResult != null)
        {
            CallLotteryLoaded(addResult);
        }
        else
        {
            var updatedLottery = UpdateExistingLottery(fetchedData);
            
            if(updatedLottery != null)
                CallLotteryUpdated(updatedLottery);
        }

        callback(fetchedData, error);
    }

    private Lottery AddLottery(LotteryData lotteryData)
    {
        if (lotteryData == null) return null;

        if (_lotteries.Any(l => l.Data.id == lotteryData.id || l.Data.type == lotteryData.type))
        {
            //Debug.Log(string.Format("Лотерея с указанным ID: {0} или типом: {1} уже существует", lotteryData.id, lotteryData.type));
            return null;
        }
        else
        {
            var lottery = new Lottery(lotteryData);
            _lotteries.Add(lottery);
            return lottery;
        }
    }

    private bool RemoveLottery(int lotteryId)
    {
        if (_lotteries.Any(l => l.Data.id == lotteryId))
        {
            _lotteries.RemoveAll(l => l.Data.id == lotteryId);
            return true;
        }
        else
        {
            return false;
        }
    }

    private Lottery UpdateExistingLottery(LotteryData lotteryData)
    {
        if (lotteryData == null) return null;

        var existingLottery = _lotteries.FirstOrDefault(l => l.Data.id == lotteryData.id && l.Data.type == lotteryData.type);

        if (existingLottery != null)
        {
            existingLottery.Data = lotteryData;
            return existingLottery;
        }
        else
        {
            Debug.Log(string.Format("Не найдена лотерея с указанным ID: {0} и типом: {1}", lotteryData.id, lotteryData.type));
            return null;
        }
    }

    private static string GetUrlStringForLottery(LotteryData.LotteryType lotteryType)
    {//http://sstucloud.no-ip.info/LotteyServerApp/UserAPI/ActualDrawingForUser?type=0&userId=2

        if (SessionController.instance.currentUser != null)
            return string.Format("{0}/UserAPI/ActualDrawingForUser?type={1}&userId={2}",
                WWWOperations.instance.ServerUrl,
                (int) lotteryType, SessionController.instance.currentUser.id);
        else
            return string.Format("{0}/UserAPI/ActualDrawing?type={1}", WWWOperations.instance.ServerUrl, (int)lotteryType);
        
    }
}