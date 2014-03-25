using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GUI;
using UnityEngine;
using System.Collections;



public class TicketsGrid : UIGrid
{
    public delegate void TicketsGridUpdated(TicketsGrid grid);

    public event TicketsGridUpdated OnGridUpdated;

    protected virtual void GridUpdated(TicketsGrid grid)
    {
        TicketsGridUpdated handler = OnGridUpdated;
        if (handler != null) handler(grid);
    }

    public GameObject TicketPrefab;
    public TicketsScrollView _scrollView;

    private int _maxTicketViewsAmount = 3;
    
    public int maxTicketViewsAmount
    {
        get { return _maxTicketViewsAmount; }
        set
        {
            _maxTicketViewsAmount = Mathf.Clamp(value, 2, 10);

            RemoveTicketsRange(_maxTicketViewsAmount, Items.Count - _maxTicketViewsAmount);
        }
    }

    public DragDropTicket FirstTicket
    {
        get
        {
            return Items.FirstOrDefault();
        }
    }

    public List<DragDropTicket> Items = new List<DragDropTicket>();


    void Awake()
    {
        cellHeight = TicketPlaceholder.instance.localSize.y;

        _scrollView.OnTicketActivated += OnTicketActivated;

        TicketsController.instance.OnTicketsModelLoaded += ModelInitialized;
        TicketsController.instance.OnTicketsAdded += TicketsAdded;
        TicketsController.instance.OnTicketsRemoved += TicketsRemoved;
    }

    private void TicketsRemoved(IEnumerable<Ticket> tickets)
    {
        foreach (var ticket in tickets)
        {
            RemoveGridElement(ticket);
        }

        SortByTicketId();

        GridUpdated(this);
    }

    private void TicketsAdded(IEnumerable<Ticket> tickets)
    {
        InsertTickets(tickets);

        GridUpdated(this);
    }

    private void ModelInitialized(TicketData model)
    {
        InsertTickets(model.Tickets);

        GridUpdated(this);
    }


    private void OnTicketActivated(DragDropTicket ticket)
    {
        RemoveGridElement(ticket);

        GridUpdated(this);
    }

    public void InsertTickets(IEnumerable<Ticket> tickets, int at = 0)
    {
        if (!Application.isPlaying) return;

        var containingTicketsId = Items.Select(t => t.ticketInstance.id);

        var newTickets = tickets.Where(t => !containingTicketsId.Contains(t.id)).OrderBy(t => t.id).Take(maxTicketViewsAmount - Items.Count);

        //int counter = 0;
        foreach (var newTicket in newTickets)
        {
            if(newTicket == null) continue;
            
            AddGridElement(newTicket);

            //counter++;

            //if (counter >= maxTicketViewsAmount)
            //    break;
        }

        SortByTicketId();
    }

    public void AddGridElement(Ticket ticket)
    {
        if (!Application.isPlaying) return;

        var alreadyContains = Items.FirstOrDefault(t => t.ticketInstance.id == ticket.id);

        if (alreadyContains != null) return;

        var childTicket = gameObject.AddChild(TicketPrefab);
        childTicket.SetActive(true);

        var dragNdrop = childTicket.GetComponent<DragDropTicket>();
        dragNdrop.ticketInstance = ticket;

        dragNdrop.Widget.alpha = 0f;
        TweenAlpha.Begin(dragNdrop.gameObject, 0.2f, 1f);

        Items.Add(dragNdrop);
    }

    public void RemoveGridElement(DragDropTicket ticket)
    {
        if (!Application.isPlaying) return;

        if (Items.Remove(ticket))
        {
            ticket.gameObject.SetActive(false);
        }
    }

    public void RemoveGridElement(Ticket ticket)
    {
        if (!Application.isPlaying) return;

        var containingTicket = Items.FirstOrDefault(t => t.ticketInstance.id == ticket.id);

        if (containingTicket != null)
        {
            Items.Remove(containingTicket);
            Destroy(containingTicket.gameObject);
        }
    }

    public void RemoveGridElementAt(int index)
    {
        if (!Application.isPlaying) return;

        DragDropTicket item = null;
        try
        {
            item = Items[index];
        }
        catch { }

        if(item != null)
        {
            Items.Remove(item);
            Destroy(item.gameObject);
        }
    }

    public void RemoveTicketsRange(int startIndex, int count)
    {
        if (startIndex < Items.Count - 1)
        {
            for (int i = startIndex; i < Items.Count && i < startIndex + count; i++)
            {
                RemoveGridElementAt(i);
            }
        }
    }

    //public void Clear()
    //{
    //    if (Application.isEditor) return;

    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        var child = transform.GetChild(i);

    //        if (child.name == "Z(Buy)") continue;

    //        DestroyImmediate(child.gameObject);
    //    }   

    //    _tickets.Clear();
    //}

    //void Sort()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        var child = transform.GetChild(i);

    //        if (child.name == "Z(Buy)") continue;

    //        child.name = string.Format("_Ticket{0}", i);
    //    }

    //    Reposition();
    //}
    void SortByTicketId()
    {
        foreach (var ticket in Items)
        {
                ticket.collider.enabled = false;

            ticket.name = string.Format("_Ticket({0})", ticket.ticketInstance.id.ToString("000"));
        }

        Items = Items.OrderBy(t => t.ticketInstance.id).ToList();

        if (Items.Count > 0)
            Items.First().collider.enabled = true;

        Reposition();
    }
}