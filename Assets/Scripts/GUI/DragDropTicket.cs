using Assets.Scripts.GUI;
using UnityEngine;
using System.Collections;

public class DragDropTicket : UIDragDropItem
{
    public Ticket ticketInstance;

    public event TicketReturned OnTicketReturned;
    public event TicketActivated OnTicketActivated;

    protected virtual void TicketReturned(DragDropTicket ticket)
    {
        enabled = false;
        collider.enabled = true;

        TicketReturned handler = OnTicketReturned;
        if (handler != null) handler(ticket);
    }
    
    protected virtual void TicketActivated(DragDropTicket ticket)
    {
        enabled = false;
        collider.enabled = true;

        Destroy(gameObject);

        TicketActivated handler = OnTicketActivated;
        if (handler != null) handler(ticket);
    }

    private TicketsScrollView _scrollView;
    private TicketsGrid _grid;

	// Use this for initialization
	protected override void Start ()
	{
        base.Start();

        _grid = NGUITools.FindInParents<TicketsGrid>(transform);
        _scrollView = NGUITools.FindInParents<TicketsScrollView>(transform);

	    enabled = false;
	}

    public void Pull()
    {
        if (UICamera.currentTouch != null)
        UICamera.currentTouch.pressed = gameObject;

        enabled = true;
        collider.enabled = false;

        OnDragDropStart();
    }

    protected override void OnDragEnd()
    {
        //Debug.Log("Dropped from " + transform.parent.name);
        base.OnDragEnd();

        enabled = false;
        collider.enabled = true;


        //Invoke("Anchor", Time.deltaTime);
    }

    protected override void OnDragDropMove(Vector3 delta)
    {
        base.OnDragDropMove(delta);
        //Debug.Log(UICamera.currentTouch.current);
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);

        if (UICamera.currentTouch == null || UICamera.currentTouch.pressed == null)
            TicketReturned(this);

        if (surface)
        {
            if (surface.tag == "TicketDropSurface")
            {
                TicketActivated(this);
            }
        }
        else
        {
            TicketReturned(this);
        }
    }
}
