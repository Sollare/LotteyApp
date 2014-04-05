using System;
using LotteyServerApp.Models;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;

public class BetsController : MonoBehaviour
{
    public event EventHandler<Bet> OnBetPerformed;
    public event EventHandler<ResponseMessage> OnBetFailed;

    public virtual void BetPerformed(Bet e)
    {
        //Debug.Log("BetResponse: " + e.id + " - " + e.ticketId);
        
        EventHandler<Bet> handler = OnBetPerformed;
        if (handler != null) handler(this, e);
    }

    protected virtual void BetFailed(ResponseMessage e)
    {
        EventHandler<ResponseMessage> handler = OnBetFailed;
        if (handler != null) handler(this, e);
    }

    private static BetsController _instance;

    public static BetsController instance
    {
        get
        {
            if (_instance != null) return _instance;
            else
                return (_instance = GameObject.Find("LotteryController").GetComponent<BetsController>());
        }
    }

    void Awake()
    {
        TicketsController.instance.OnTicketViewActivated += OnTicketViewActivated;
    }

    private void OnTicketViewActivated(DragDropTicket ticket, LotteryItem item)
    {
        //Debug.Log("Ticket " + ticket.ticketInstance.id + " activated on lottery " + item.lotteryInstance.id);

        ConfirmPanel.ShowConfirmDialog("Do you want to make a bet on " + item.lotteryInstance.name + "?",
            () =>
            {
                var mBet = new Bet() {drawingId = item.lotteryInstance.id, id = ticket.ticketInstance.id};
                var mTicket = ticket.ticketInstance;
                //BetPerformed(new Bet(mBet.id, mTicket.id, mBet.drawingId));

                PerformBet(mTicket, item.lotteryInstance, BetResponse);
            },
            () => ticket.TicketReturned(ticket));
    }

    private void BetResponse(ResponseMessage fetchedObject, string error)
    {
    }

    /// <summary>
    /// Совершить ставку
    /// </summary>
    /// <param name="ticket">Билет</param>
    /// <param name="lottery">Лотерея</param>
    public void PerformBet(Ticket ticket, LotteryData lottery, WWWOperations.OnObjectFecthed<ResponseMessage> callback)
    {
        WWWOperations.instance.FetchJsonObject<ResponseMessage>(GetUrlStringForBet(ticket, lottery), callback, BetChainedCallback);
    }

    private void BetChainedCallback(ResponseMessage fetchedData, string error, WWWOperations.OnObjectFecthed<ResponseMessage> callback)
    {
        if (error != null)
        {
            BetFailed(new ResponseMessage(0, error));
        }
        else
        {
            if (fetchedData.Code == 0)
            {
                BetFailed(fetchedData);
            }
            else
            {
                var serialziedBet = fetchedData.Message;
                Bet bet = JsonConvert.DeserializeObject<Bet>(serialziedBet);

                BetPerformed(bet);
            }
        }

        callback(fetchedData, error);
    }

    private static string GetUrlStringForBet(Ticket ticket, LotteryData lottery)
    {
        return string.Format("{0}/Bets/Bet/{1}?ticketId={2}", WWWOperations.instance.ServerUrl, lottery.id,
            ticket.id);
    }
}

//public class BetInfo : EventArgs
//{
//    public Bet Bet;
//    public Ticket Ticket;

//    public BetInfo(Bet bet, Ticket ticket)
//    {
//        Bet = bet;
//        Ticket = ticket;
//    }
//}

public class Bet : EventArgs
{
    public int id;
    public int ticketId;
    public int drawingId;

    public Bet() {}

    public Bet(int Id, int TicketId, int DrawingId)
    {
        id = Id;
        ticketId = TicketId;
        drawingId = DrawingId;
    }
}