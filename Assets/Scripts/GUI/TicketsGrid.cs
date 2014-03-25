﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class TicketsGrid : UIGrid
{
    public GameObject TicketPrefab;

    private int _maxTicketViewsAmount = 3;
    
    public int maxTicketViewsAmount
    {
        get { return _maxTicketViewsAmount; }
        set
        {
            _maxTicketViewsAmount = Mathf.Clamp(value, 2, 10);

            RemoveTicketsRange(_maxTicketViewsAmount, _items.Count - _maxTicketViewsAmount);
        }
    }

    public DragDropTicket FirstTicket
    {
        get
        {
            return _items.FirstOrDefault();
        }
    }

    void Awake()
    {
        cellHeight = TicketPlaceholder.instance.localSize.y;
    }

    private List<DragDropTicket> _items = new List<DragDropTicket>();

    public void InsertTickets(IEnumerable<Ticket> tickets, int at = 0)
    {
        if (!Application.isPlaying) return;

        var containingTicketsId = _items.Select(t => t.ticketInstance.id);

        var newTickets = tickets.Where(t => !containingTicketsId.Contains(t.id));

        int counter = 0;
        foreach (var newTicket in newTickets)
        {
            AddGridElement(newTicket);

            counter++;

            if (counter >= maxTicketViewsAmount)
                break;
        }

        SortByTicketId();
    }

    public void AddGridElement(Ticket ticket)
    {
        if (!Application.isPlaying) return;

        var alreadyContains = _items.FirstOrDefault(t => t.ticketInstance.id == ticket.id);

        if (alreadyContains != null) return;

        var childTicket = gameObject.AddChild(TicketPrefab);
        childTicket.SetActive(true);

        var dragNdrop = childTicket.GetComponent<DragDropTicket>();
        dragNdrop.ticketInstance = ticket;

        _items.Add(dragNdrop);
    }

    public void RemoveGridElement(Ticket ticket)
    {
        if (!Application.isPlaying) return;

        var containingTicket = _items.FirstOrDefault(t => t.ticketInstance.id == ticket.id);

        if (containingTicket != null)
        {
            _items.Remove(containingTicket);
            Destroy(containingTicket.gameObject);
        }
    }

    public void RemoveGridElementAt(int index)
    {
        if (!Application.isPlaying) return;

        DragDropTicket item = null;
        try
        {
            item = _items[index];
        }
        catch { }

        if(item != null)
        {
            _items.Remove(item);
            Destroy(item.gameObject);
        }
    }

    public void RemoveTicketsRange(int startIndex, int count)
    {
        if (startIndex < _items.Count - 1)
        {
            for (int i = startIndex; i < _items.Count && i < startIndex + count; i++)
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
        foreach (var ticket in _items)
        {
                ticket.collider.enabled = false;

            ticket.name = string.Format("_Ticket{0}", ticket.ticketInstance.id);
        }

        _items = _items.OrderBy(t => t.ticketInstance.id).ToList();

        if (_items.Count > 0)
            _items.First().collider.enabled = true;

        Reposition();
    }
}