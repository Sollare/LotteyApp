using UnityEngine;
using System.Collections;

public class TicketPlaceholder : MonoBehaviour
{
    private static UIWidget _instance;
    public static UIWidget instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("_TicketPlaceHolder").GetComponent<UIWidget>();

            return _instance;
        }
    }
}
