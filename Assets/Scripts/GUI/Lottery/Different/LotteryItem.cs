using System;
using UnityEngine;
using System.Collections;

public class LotteryItem : MonoBehaviour
{
    public event DataExtensions.EventDelegate<LotteryItem> OnDataUpdated;

    protected virtual void CallDataUpdated(LotteryItem value)
    {
        DataExtensions.EventDelegate<LotteryItem> handler = OnDataUpdated;
        if (handler != null) handler(value);
    }

    public LotteryData.LotteryType LoadLotteryOfType;

    [SerializeField]
    private LotteryData _lotteryInstance;

    public LotteryData lotteryInstance
    {
        get { return _lotteryInstance; }
        set
        {
            _lotteryInstance = value;

            if (value != null)
                ShowLoadingScreen(false);
        }
    }

    public UILabel LotteryLabel;
    public UILabel LabelTotal;
    public UILabel LabelTotalValue;

    public UISprite LoadingRing;

    private int fetchRepeatRate = 3;
    
    void Awake()
    {
        ShowLoadingScreen(true);

        if (LoadLotteryOfType != LotteryData.LotteryType.Instant)
            StartCoroutine("EnumeratorRepeatFetch", 3);
        else
        {
            lotteryInstance = new LotteryData {type = LotteryData.LotteryType.Instant, id = -1, name = "Instant", totalmoney = 35};
        }

        SessionController.instance.SessionStarted += SessionStarted;
        SessionController.instance.SessionEnded += SessionEnded;
        BetsController.instance.OnBetPerformed += BetPerformed;
    }

    void ShowLoadingScreen(bool show)
    {
        LotteryLabel.cachedGameObject.SetActive(!show);
        LabelTotal.cachedGameObject.SetActive(!show);
        LabelTotalValue.cachedGameObject.SetActive(!show);

        LoadingRing.cachedGameObject.SetActive(show);
    }

    private void BetPerformed(object sender, Bet bet)
    {
        
    }

    private void SessionStarted(User user)
    {
        if (LoadLotteryOfType == LotteryData.LotteryType.Instant) return;

        fetchRepeatRate = 10;
        ShowLoadingScreen(true);
        StartCoroutine("AuhorizedLotteryDataFetch", fetchRepeatRate);
    }

    private void SessionEnded(User user)
    {
        fetchRepeatRate = 3;
        StopCoroutine("AuhorizedLotteryDataFetch");
    }

    public void FetchData()
    {
        LotteryController.instance.FetchLottery(LoadLotteryOfType, OnLotteryDataFetched);
    }

    public void ConstantFetchData()
    {
        LotteryController.instance.FetchLottery(LoadLotteryOfType, OnceAgainDataFetched);
    }


    void RepeatFetching(int count)
    {
        StartCoroutine("EnumeratorRepeatFetch", count);
    }

    private IEnumerator EnumeratorRepeatFetch(int count)
    {
        int counter = 0;

        while (counter < count)
        {
            counter++;
            FetchData();

            yield return new WaitForSeconds(fetchRepeatRate);
        }
    }

    private IEnumerator AuhorizedLotteryDataFetch(int rate)
    {
        while (SessionController.isAuthorized)
        {
            ConstantFetchData();

            yield return new WaitForSeconds(rate);
        }
    }

    private void OnLotteryDataFetched(LotteryData fetchedObject, string error)
    {
        if (error == null)
            StopCoroutine("EnumeratorRepeatFetch");

        //  public int id;
        //public string name;
        //public int totalmoney;
        //public DateTime expiration;
        //public LotteryType type;

        if (fetchedObject != null && error == null)
        {
            //Debug.Log(">> LOADED >> " + fetchedObject.type + " >> " + fetchedObject.expiration);

            lotteryInstance = fetchedObject;
            UpdateView(lotteryInstance);

            CallDataUpdated(this);
        }
        else
        {
            //Debug.Log("Ошибка: " + error + " Data: "+ fetchedObject);
            //LotteryLabel.text = "";
        }

    }

    private void OnceAgainDataFetched(LotteryData fetchedObject, string error)
    {

        if (fetchedObject != null && error == null)
        {
            StopCoroutine("EnumeratorRepeatFetch");
            StopCoroutine("AuhorizedLotteryDataFetch");

            ShowLoadingScreen(false);

            if (lotteryInstance == null)
            {
                lotteryInstance = fetchedObject;
                //Debug.Log("Complete update");
            }
            else
            {
                lotteryInstance.totalmoney = fetchedObject.totalmoney;
                lotteryInstance.MyBets = fetchedObject.MyBets;
            }

            //Debug.Log("Bets: " + fetchedObject.MyBets.Length);

            UpdateView(lotteryInstance);

            CallDataUpdated(this);
        }
    }

    void UpdateView(LotteryData data)
    {
        if (data.totalmoney >= 0)
        {
            LabelTotalValue.text = string.Format("${0:D3}.00", data.totalmoney);

            LabelTotal.cachedGameObject.SetActive(true);
            LabelTotalValue.cachedGameObject.SetActive(true);
        }
        else
        {
            LabelTotal.cachedGameObject.SetActive(true);
            LabelTotalValue.cachedGameObject.SetActive(true);
        }

        LotteryLabel.text = lotteryInstance.name;
    }
}
