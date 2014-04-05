using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GUI;
using UnityEngine;
using System.Collections;

public delegate void TicketsModelLoaded(TicketData model);
public delegate void TicketsAddedInModel(IEnumerable<Ticket> tickets);
public delegate void TicketsRemovedFromModel(IEnumerable<Ticket> tickets);

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

    public TicketData Model;

    public event TicketsModelLoaded OnTicketsModelLoaded;
    public event TicketsAddedInModel OnTicketsAdded;
    public event TicketsRemovedFromModel OnTicketsRemoved;

    /// <summary>
    /// Активация тикета на акцию. Билет еще НЕ использован
    /// </summary>
    public TicketViewActivated OnTicketViewActivated;

    public virtual void CallTicketViewActivated(DragDropTicket ticket, LotteryItem item)
    {
        TicketViewActivated handler = OnTicketViewActivated;
        if (handler != null) handler(ticket, item);
    }

    public virtual void CallTicketsModelLoaded(TicketData ticketData)
    {
        TicketsModelLoaded handler = OnTicketsModelLoaded;
        if (handler != null) handler(ticketData);
    }

    public virtual void CallTicketsAdded(IEnumerable<Ticket> tickets)
    {
        TicketsAddedInModel handler = OnTicketsAdded;
        if (handler != null) handler(tickets);
    }

    public virtual void CallTicketsRemoved(IEnumerable<Ticket> tickets)
    {
        TicketsRemovedFromModel handler = OnTicketsRemoved;
        if (handler != null) handler(tickets);
    }

    public void Initialize()
    {
        if (!Application.isPlaying) return;

        SessionController.instance.SessionStarted += OnSessionStarted;
        SessionController.instance.SessionEnded += OnSessionEnded;

        BetsController.instance.OnBetPerformed += OnBetPerformed;

        Model = new TicketData(this, 5);
        CallTicketsModelLoaded(Model);
    }

    private void OnBetPerformed(object sender, Bet betInfo)
    {
        var ticket = Model.GetTicket(betInfo.ticketId);

        if (ticket != null)
        {
            //TicketsController.instance.Model.RemoveTicket(ticket);
            Debug.Log("Removing ticket " + ticket.id);
            Model.RemoveTicket(ticket);
        }
    }

    private void OnSessionEnded(User user)
    {
        Model = null;
        CallTicketsModelLoaded(null);
    }

    private void OnSessionStarted(User user)
    {
        Model = new TicketData(this, user.freeTickets);
        CallTicketsModelLoaded(Model);
    }
}

[Serializable]
public class TicketData
{
    private TicketsController _controller;
    private List<Ticket> _tickets;

    public IEnumerable<Ticket> Tickets
    {
        get
        {
            return _tickets;
        }
    }

    public TicketData(TicketsController controller, IEnumerable<Ticket> tickets)
    {
        _controller = controller;

        _tickets = tickets.ToList();
    }

    public TicketData(TicketsController controller, int ticketsCount)
    {
        _controller = controller;

        _tickets = new List<Ticket>();

        for (int i = 0; i < ticketsCount; i++)
            _tickets.Add(Ticket.NewTicket());

        _tickets = _tickets.OrderBy(t => t.id).ToList();

        _controller.CallTicketsModelLoaded(this);
    }
    
    public bool RemoveTicket(Ticket ticket)
    {
        var t = _tickets.FirstOrDefault(tk => tk.id == ticket.id);

        if (t == null) return false;
        else
        {
            _tickets.RemoveAll(tk => tk.id == ticket.id);
            _controller.CallTicketsRemoved(new List<Ticket>() { ticket });
            return true;
        }
    }

    public bool AddTicket(Ticket ticket)
    {
        var t = _tickets.FirstOrDefault(tk => tk.id == ticket.id);

        if (t == null)
        {
            _tickets.Add(ticket);
            _controller.CallTicketsAdded(new List<Ticket>() { ticket });
            return true;
        }
        else
            return false;
    }

    public Ticket GetTicket(int id)
    {
        return _tickets.FirstOrDefault(tk => tk.id == id);
    }
}