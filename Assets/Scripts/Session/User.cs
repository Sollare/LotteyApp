using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

[Serializable]
public class User
{
    public int id;
    public string name;
    public string email;

    public Ticket[] freeTickets;
    public Bet[] bets;
    public Win[] victories;

    public override string ToString()
    {
        StringBuilder sb= new StringBuilder();

        sb.AppendLine("Id:" + id);
        sb.AppendLine("Name:" + name);
        sb.AppendLine("Email:" + email);

        sb.AppendLine("-----");
        sb.AppendLine("Tickets: " );

        foreach (var ticket in freeTickets)
        {
            sb.AppendLine("Ticket: " + ticket.id);
        }

        sb.AppendLine("Bets: " + bets.Count());
        sb.AppendLine("Wins: " + victories.Count());

        return sb.ToString();
    }
}
