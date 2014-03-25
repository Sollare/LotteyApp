﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public delegate void TicketViewPulled(DragDropTicket ticket);
    public delegate void TicketViewReturned(DragDropTicket ticket);
    public delegate void TicketViewActivated(DragDropTicket ticket);

    public delegate void TicketViewEvent(DragDropTicket ticket);

    public delegate void TicketEvent(Ticket ticket);
    public delegate void TicketsEvent(IEnumerable<Ticket> tickets);

    public class TicketsScrollView : UIScrollView
    {
        public TicketsGrid grid;

        public float PullCoefficient = 1.5f;

        public event TicketViewPulled OnTicketPulled;
        public event TicketViewActivated OnTicketActivated;
        public event TicketViewReturned OnTicketReturned;

        public event TicketsEvent OnTicketsAdded;
        public event TicketsEvent OnTicketsRemoved;
        
        protected virtual void CallTicketPulled(DragDropTicket ticket)
        {
            Debug.Log(GetType().Name + ": Потащили билет " + ticket.ticketInstance.id);

            TicketViewPulled handler = OnTicketPulled;
            if (handler != null) handler(ticket);
        }
        protected virtual void CallTicketReturned(DragDropTicket ticket)
        {
            Debug.Log(GetType().Name + ": Вернули билет " + ticket.ticketInstance.id);

            TicketViewReturned handler = OnTicketReturned;
            if (handler != null) handler(ticket);
        }
        protected virtual void CallTicketActivated(DragDropTicket ticket)
        {
            Debug.Log(GetType().Name + ": Активировали билет " + ticket.ticketInstance.id);

            TicketViewActivated handler = OnTicketActivated;
            if (handler != null) handler(ticket);
        }

        public void Start()
        {
            //grid.Clear();
            RecalculateBounds();

            TicketsController.instance.OnTicketsModelLoaded += ModelInitialized;   
            TicketsController.instance.OnTicketsAdded += TicketsAdded;
            TicketsController.instance.OnTicketsRemoved += TicketsRemoved;

            // Контроллер билетов должен знать о том, что билет был активирован
            this.OnTicketActivated += TicketsController.instance.OnTicketViewActivated;

            // TODO: КОСТЫЛЬ!!!
            TicketsController.instance.Initialize();
        }

        private void TicketsRemoved(IEnumerable<Ticket> tickets)
        {
            RecalculateBounds();
        }

        private void TicketsAdded(IEnumerable<Ticket> tickets)
        {
            RecalculateBounds();
        }

        private void ModelInitialized(TicketData model)
        {
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
        }

        private void RecalculateBounds()
        {
            panel.bottomAnchor.absolute = - (TicketsController.instance.Model.Tickets.Count()) * (int)TicketPlaceholder.instance.localSize.y;
        }
    }
}