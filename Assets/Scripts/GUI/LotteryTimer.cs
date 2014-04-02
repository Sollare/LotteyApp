using System;
using System.Collections.Generic;
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

    private double totalSecondsLeft = -1;
    
    public const string EmptyTimerString = "--:--:--";
    private string resultString = EmptyTimerString;

    public void SetExpirationTime(double expirationDate)
    {
        //Debug.Log("new time" + expirationDate);
        totalSecondsLeft = Math.Round(expirationDate);
    }

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
        while (totalSecondsLeft > 0)
        {
            resultString = GetFormatString();

            if (updateLabel && label)
                label.text = resultString;

            CallTimeStringChanged(resultString);

            yield return new WaitForSeconds(1f);

            totalSecondsLeft--;
            //_timeSpan = _timeSpan.Value.Subtract(_secondSpan);
        }
    }

    private string GetFormatString()
    {
        if (totalSecondsLeft < 0) return EmptyTimerString;
        //HOURS

        //double hours = secondsToHours(totalSecondsLeft);
        //double minutes = secondsToMins(totalSecondsLeft);
        //double seconds = totalSecondsLeft%60;

        double totalHoursOnline = Convert.ToDouble(totalSecondsLeft)/(double) 3600;
        double totalMinutesOnline = 0;
        double totalSecondsOnline = 0;

        if (Math.Abs(totalSecondsLeft%3600) > 0.01f)
        {
            totalMinutesOnline = (totalHoursOnline - Math.Floor(totalHoursOnline)) * 60;
            totalHoursOnline = Math.Floor(Convert.ToDouble(totalHoursOnline));
        }
        else
        {
            totalHoursOnline = totalSecondsLeft/3600;
        }
        if (Math.Abs(totalMinutesOnline%60) > 0.01f)
        {
            totalSecondsOnline = (totalMinutesOnline - Math.Floor(totalMinutesOnline))*60;
                //(totalMinutesOnline – Math.Floor(totalMinutesOnline)) *60;
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
