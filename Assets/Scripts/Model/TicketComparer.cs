using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TicketComparer : IEqualityComparer<Ticket>
{
    public bool Equals(Ticket x, Ticket y)
    {
        return x.id == y.id;
    }

    public int GetHashCode(Ticket ticket)
    {
        return ticket.GetHashCode();
    }
}
