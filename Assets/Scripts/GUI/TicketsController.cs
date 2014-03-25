using System;
using System.Collections.Generic;
using Assets.Scripts.GUI;
using UnityEngine;
using System.Collections;

public delegate void TicketsModelLoaded(TicketData model);
public delegate void TicketsAdded(IEnumerable<Ticket> tickets);
public delegate void TicketsRemoved(IEnumerable<Ticket> tickets);

public class TicketsController 
{
    private static TicketsController _instance;

    public static TicketsController instance
    {
        get
        {
            if (_instance != null) return _instance;
            else
            {
                _instance = new TicketsController();
                _instance.Initialize();
                return _instance;
            }
        }
    }

    public TicketData model;

    public event TicketsModelLoaded OnTicketsModelLoaded;
    public event TicketsAdded OnTicketsAdded;
    public event TicketsRemoved OnTicketsRemoved;

    public TicketActivated OnTicketActivated;

    protected virtual void CallTicketActivated(DragDropTicket ticket)
    {
        TicketActivated handler = OnTicketActivated;
        if (handler != null) handler(ticket);
    }

    protected virtual void CallTicketsModelLoaded(TicketData ticketData)
    {
        TicketsModelLoaded handler = OnTicketsModelLoaded;
        if (handler != null) handler(ticketData);
    }

    protected virtual void CallTicketsAdded(IEnumerable<Ticket> tickets)
    {
        TicketsAdded handler = OnTicketsAdded;
        if (handler != null) handler(tickets);
    }

    protected virtual void CallTicketsRemoved(IEnumerable<Ticket> tickets)
    {
        TicketsRemoved handler = OnTicketsRemoved;
        if (handler != null) handler(tickets);
    }

    public void Initialize()
    {
        if (!Application.isPlaying) return;

        model = new TicketData(2);
        CallTicketsModelLoaded(model);
    }
}

[Serializable]
public class TicketData
{
    public List<Ticket> tickets;

    public TicketData(int ticketsCount)
    {
        tickets = new List<Ticket>();

        for (int i = 0; i < ticketsCount; i++)
            tickets.Add(new Ticket() { id = UnityEngine.Random.Range(1, 1000) });
    }
}