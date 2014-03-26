using System;
using UnityEngine;
using System.Collections;

public class LotteryItem : MonoBehaviour
{
    public LotteryData.LotteryType LoadLotteryOfType;

    public LotteryData Data;

    public UILabel LotteryLabel;
    
    void Awake()
    {
        if (LoadLotteryOfType != LotteryData.LotteryType.Instant)
            RepeatFetching(3);
        else
        {
            Data = new LotteryData {type = LotteryData.LotteryType.Instant, id = -1, name = "Instant", totalmoney = 35};
        }
    }
    
    public void FetchData()
    {
        LotteryController.instance.FetchLottery(LoadLotteryOfType, OnLotteryDataFetched);
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

            yield return new WaitForSeconds(3f);
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
            Debug.Log(">> LOADED >> " + fetchedObject.type + " >> " + fetchedObject.expiration);

            Data = fetchedObject;

            LotteryLabel.text = Data.name;
        }
        else
        {
            LotteryLabel.text = "Loading...";
        }

    }
}
