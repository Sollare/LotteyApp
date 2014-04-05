using UnityEngine;
using System.Collections;

public class BuyButton : MonoBehaviour {

    public void BuyTickets()
    {
        ConfirmPanel.ShowConfirmDialog("Buy 5 tickets for 0.99$?", delegate
        {
            for(int i = 0; i < 5; i++)
                TicketsController.instance.Model.AddTicket(Ticket.NewTicket());
        });
        //TicketsController.instance.Model.AddTicket(Ticket.NewTicket());
    }
}
