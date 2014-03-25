using System;
using Assets.Scripts.GUI;
using UnityEngine;
using System.Collections;

public class LotteryDropContainer : MonoBehaviour
{
    public float HeightCoefficient = 1.3f;
    public float Duration = 1.3f;

    // Нужен чтобы принимать события отрыва билета и т.п.
    public TicketsScrollView scrollView;


    private bool expanded = false;
    private DragDropTicket _waitingForTicket;

    void Awake()
    {
        scrollView.OnTicketPulled += TicketPulled;
        scrollView.OnTicketReturned += TicketReturned;
        scrollView.OnTicketActivated += TicketActivated;
    }

    // Включаем коллайдер, ждем приема
    private void TicketPulled(DragDropTicket ticket)
    {
        StartCoroutine("WaitForCursorOver");

        _waitingForTicket = ticket;
        collider.enabled = true;

        if(UICamera.current != null)
            UICamera.currentTouch.current = this.gameObject;

    }

    private void TicketReturned(DragDropTicket ticket)
    {
        AbortPreparation();
    }

    private void TicketActivated(DragDropTicket ticket)
    {
        AbortPreparation();
    }

    void AbortPreparation()
    {
        StopCoroutine("WaitForCursorOver");

        _waitingForTicket = null;
        collider.enabled = false;
        UICamera.currentTouch.current = null;

        Squeeze();
    }

    //void OnDrop(GameObject go)
    //{
    //    Debug.Log("Билет был активирован: " + go.name);
    //}

    void Expand()
    {
        var tweenScale = TweenScale.Begin(gameObject, Duration, new Vector3(1, HeightCoefficient, 1));
        expanded = true;
    }

    void Squeeze()
    {
        var tweenScale = TweenScale.Begin(gameObject, Duration, new Vector3(1, 1, 1));
        expanded = false;
    }
    
    IEnumerator WaitForCursorOver()
    {
        while (true)
        {
            var hoveredObject = UICamera.hoveredObject;

            if (hoveredObject == null)
            {
                if (expanded)
                    Squeeze();
            }
            else
            {
                if (hoveredObject == this.gameObject && !expanded)
                    Expand();

                if (hoveredObject != this.gameObject && expanded)
                    Squeeze();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
