using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LotteriesScrollView : MonoBehaviour
{
    private List<LotteryItem> items;

	void Awake ()
	{
	    items = gameObject.GetComponentsInChildren<LotteryItem>().ToList();
	}
	
	void Update () 
    {
	
	}
}
