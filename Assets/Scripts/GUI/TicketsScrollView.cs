using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public delegate void TicketPulled(DragDropTicket ticket);
    public delegate void TicketReturned(DragDropTicket ticket);
    public delegate void TicketActivated(DragDropTicket ticket);


    public class TicketsScrollView : UIScrollView
    {
        public TicketsGrid grid;

        public float PullCoefficient = 1.5f;

        public event TicketPulled OnTicketPulled;
        public event TicketActivated OnTicketActivated;
        public event TicketReturned OnTicketReturned;

        protected virtual void CallTicketPulled(DragDropTicket ticket)
        {
            Debug.Log("Потащили билет " + ticket.ticketInstance.id);

            TicketPulled handler = OnTicketPulled;
            if (handler != null) handler(ticket);
        }
        protected virtual void CallTicketReturned(DragDropTicket ticket)
        {
            Debug.Log("Вернули билет " + ticket.ticketInstance.id);

            TicketReturned handler = OnTicketReturned;
            if (handler != null) handler(ticket);
        }
        protected virtual void CallTicketActivated(DragDropTicket ticket)
        {
            Debug.Log("Активировали билет " + ticket.ticketInstance.id);

            TicketActivated handler = OnTicketActivated;
            if (handler != null) handler(ticket);
        }

        public void Start()
        {
            //grid.Clear();
            panel.bottomAnchor.absolute = -2 * (int)TicketPlaceholder.instance.localSize.y;

            TicketsController.instance.OnTicketsModelLoaded += ModelInitialized;   
            TicketsController.instance.OnTicketsAdded += TicketsAdded;
            TicketsController.instance.OnTicketsRemoved += TicketsRemoved;

            // Контроллер билетов должен знать о том, что билет был активирован
            this.OnTicketActivated += TicketsController.instance.OnTicketActivated;

            TicketsController.instance.Initialize();
        }

        private void TicketsRemoved(IEnumerable<Ticket> tickets)
        {
        
        }

        private void TicketsAdded(IEnumerable<Ticket> tickets)
        {

        }

        private void ModelInitialized(TicketData model)
        {
            //Debug.Log("Model initialized");
            grid.InsertTickets(model.tickets);
        }

        private Vector3 move;
	
        public override void MoveRelative(Vector3 relative)
        {
            base.MoveRelative(relative);

            move += relative;
            //print(move);

            if (move.y > TicketPlaceholder.instance.localSize.y * PullCoefficient)
            {
                Press(false);

                if (UICamera.currentTouch != null)
                    UICamera.currentTouch.pressed = null;

                move = Vector3.zero;

                PullFirstTicket();
            }
        }

        public void PullFirstTicket()
        {
            var firstTicket = grid.FirstTicket;

            firstTicket.Pull();

            firstTicket.OnTicketActivated += CallTicketActivated;
            firstTicket.OnTicketReturned += TicketReturned;

            CallTicketPulled(firstTicket);
        }

        private void TicketReturned(DragDropTicket ticket)
        {
            ticket.OnTicketActivated -= CallTicketActivated;
            ticket.OnTicketReturned -= TicketReturned;

            CallTicketReturned(ticket);
        }

        public override bool shouldMoveVertically
        {
            get { return move.y < 140; }
        }

        public override void SetDragAmount(float x, float y, bool updateScrollbars)
        {
            base.SetDragAmount(x, y, updateScrollbars);
            print(y);
        }
    }
}