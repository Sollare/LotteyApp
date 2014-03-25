using UnityEngine;
using System.Collections;

public class BuyButton : MonoBehaviour {

    public void BuyTickets()
    {
        TicketsController.instance.Model.AddTicket(Ticket.NewTicket());
    }
}
