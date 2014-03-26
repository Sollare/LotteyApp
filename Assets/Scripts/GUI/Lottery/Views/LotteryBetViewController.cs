using System;
using UnityEngine;
using System.Collections;

public class LotteryBetViewController : MonoBehaviour
{

    void Awake()
    {
        BetsController.instance.OnBetPerformed += BetPerformed;
    }

    private void BetPerformed(object sender, Bet bet)
    {
        
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
