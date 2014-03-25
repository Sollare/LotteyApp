using UnityEngine;
using System.Collections;

public class InstancePreloader : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
        var ticketController = TicketsController.instance;
    }
}
