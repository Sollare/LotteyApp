using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class LotteryTimer : MonoBehaviour
{
    public delegate void TimeStringChanged(string timeString);

    public event TimeStringChanged OnTimeStringChanged;

    protected virtual void CallTimeStringChanged(string timestring)
    {
        TimeStringChanged handler = OnTimeStringChanged;
        if (handler != null) handler(timestring);
    }

    private static LotteryTimer _instance;
    public static LotteryTimer instance
    {
        get
        {
            return _instance;
        }
    }

    public UILabel label;
    public bool updateLabel;
    
    private LotteryData currentLottery;
    private int currentLotteryId;
    
    public const string EmptyTimerString = "00:00:00";
    private string resultString = EmptyTimerString;

    public void SetExpirationTime(int lotteryId)
    {
        currentLotteryId = lotteryId;

        Lottery lottery = LotteryController.instance.Model.Lotteries.FirstOrDefault(lotteryOne => lotteryOne.Data.id == lotteryId);

        if(lottery != null)
            currentLottery = lottery.Data;
    }

    private 

    void Awake()
    {
        _instance = this;
        label.text = "";
    }

    public void StartTimer()
    {
        StopCoroutine("EnumeratorStartTimer");
        StartCoroutine("EnumeratorStartTimer");
    }

    public void StopTimer()
    {
        StopCoroutine("EnumeratorStartTimer");
    }

    IEnumerator EnumeratorStartTimer()
    {
        while (currentLottery.expiration > 0)
        {
            resultString = GetFormatString();

            if (updateLabel && label)
                label.text = resultString;

            CallTimeStringChanged(resultString);

            yield return new WaitForSeconds(1f);


            foreach (var lottery in LotteryController.instance.Model.Lotteries)
            {
                lottery.Data.expiration--;
            }
        }
    }

    private string GetFormatString()
    {
        if (currentLottery.expiration < 0) return EmptyTimerString;
        //HOURS

        double totalHoursOnline = Convert.ToDouble(currentLottery.expiration) / (double)3600;
        double totalMinutesOnline = 0;
        double totalSecondsOnline = 0;

        if (Math.Abs(currentLottery.expiration % 3600) > 0.01f)
        {
            totalMinutesOnline = (totalHoursOnline - Math.Floor(totalHoursOnline)) * 60;
            totalHoursOnline = Math.Floor(Convert.ToDouble(totalHoursOnline));
        }
        else
        {
            totalHoursOnline = currentLottery.expiration / 3600;
        }

        if (Math.Abs(totalMinutesOnline%60) > 0.01f)
        {
            totalSecondsOnline = (totalMinutesOnline - Math.Floor(totalMinutesOnline))*60;
            totalMinutesOnline = Math.Floor(Convert.ToDouble(totalMinutesOnline));
        }

        var str = string.Format("{0}:{1}:{2}",
            totalHoursOnline.ToString("00"),
            totalMinutesOnline.ToString("00"),
            totalSecondsOnline.ToString("00"));


        return str;
    }

    double secondsToHours(double secs) 
    {
        double h = secs % 3600; 
        secs = secs-(3600*h); 
        return h; 
    }

    double secondsToMins(double secs) 
    {
        double m = secs % 60; 
        secs = secs-(60*m); 
        return m; 
    }

    public override string ToString()
    {
        return resultString;
    }
}
