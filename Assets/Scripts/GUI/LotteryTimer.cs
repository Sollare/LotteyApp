using System;
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

    public DateTime expirationDate { get; protected set; }

    private TimeSpan? _timeSpan;

    private TimeSpan _secondSpan = new TimeSpan(0,0,1);

    public const string EmptyTimerString = "--:--:--";
    private string resultString = EmptyTimerString;

    public void SetExpirationDate(DateTime expirationDate)
    {
        //Debug.Log(expirationDate);
        this.expirationDate = expirationDate;
        _timeSpan = expirationDate.Subtract(DateTime.Now);
    }

    void Awake()
    {
        _instance = this;
        label.text = EmptyTimerString;
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
        while (_timeSpan.HasValue && _timeSpan.Value.TotalSeconds > 0)
        {
            resultString = GetFormatString();
            CallTimeStringChanged(resultString);

            if (updateLabel && label)
                label.text = resultString;

            yield return new WaitForSeconds(1f);
            
            _timeSpan = _timeSpan.Value.Subtract(_secondSpan);
        }
    }

    private string GetFormatString()
    {
        if (_timeSpan == null) return EmptyTimerString;

        //int totalHours = _timeSpan.Value.TotalHours * 24 + _timeSpan.Value.Hours;

        return string.Format("{0}:{1}:{2}",
                     ((int)_timeSpan.Value.TotalHours).ToString("00"),
                     _timeSpan.Value.Minutes.ToString("00"),
                     _timeSpan.Value.Seconds.ToString("00"));
    }

    public override string ToString()
    {
        return resultString;
    }
}
