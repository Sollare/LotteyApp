using Assets.Scripts.GUI;
using UnityEngine;
using System.Collections;

public class DragDropTicket : UIDragDropItem
{
    public UISprite BackgroundSprite;

    public Ticket ticketInstance;

    private UIWidget _widget;

    public UIWidget Widget
    {
        get
        {
            if (_widget) return _widget;
            else return (_widget = GetComponent<UIWidget>());
        }
    }

    public event TicketViewReturned OnTicketReturned;
    public event TicketViewActivated OnTicketActivated;

    public virtual void TicketReturned(DragDropTicket ticket)
    {
        enabled = false;
        collider.enabled = true;

        TicketViewReturned handler = OnTicketReturned;
        if (handler != null) handler(ticket);
    }
    
    protected virtual void TicketActivated(DragDropTicket ticket)
    {
        enabled = false;
        collider.enabled = true;

        TicketViewActivated handler = OnTicketActivated;
        if (handler != null) handler(ticket, LotteriesScrollView.instance.currentItem);

        Debug.Log("Removing ticket");
        //TicketsController.instance.Model.RemoveTicket(ticketInstance);

        //TicketsController.instance.Model.AddTicket(Ticket.NewTicket());
        //TicketsController.instance.Model.AddTicket(Ticket.NewTicket());
        //Destroy(gameObject);
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
        if (UICamera.currentTouch == null) return;

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
        if (UICamera.currentTouch == null || UICamera.currentTouch.pressed == null || surface == null)
        {
            TicketReturned(this);
            base.OnDragDropRelease(surface);
            return;
        }

        base.OnDragDropRelease(surface);

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
