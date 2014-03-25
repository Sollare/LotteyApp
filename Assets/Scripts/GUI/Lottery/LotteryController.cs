using UnityEngine;
using System.Collections;

public class LotteryController : MonoBehaviour 
{

    private static LotteryController _instance;

    public static LotteryController instance
    {
        get
        {
            if (_instance != null) return _instance;
            else 
                return (_instance = GameObject.Find("MainScreen").GetComponent<LotteryController>());
        }
    }



	// Use this for initialization
    void Awake()
    {
        _instance = this;
    }
}
