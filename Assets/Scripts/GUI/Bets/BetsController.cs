using System;
using LotteyServerApp.Models;
using UnityEngine;
using System.Collections;

public class BetsController : MonoBehaviour
{
    public event EventHandler<Bet> OnBetPerformed;
    public event EventHandler<ResponseMessage> OnBetFailed;

    protected virtual void BetPerformed(Bet e)
    {
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
        Debug.Log("Ticket " + ticket.ticketInstance.id + " activated on lottery " + item.lotteryInstance.id);
        BetPerformed(new Bet() { drawingId = item.lotteryInstance.id, id = ticket.ticketInstance.id});
    }

    /// <summary>
    /// Совершить ставку
    /// </summary>
    /// <param name="ticket">Билет</param>
    /// <param name="lottery">Лотерея</param>
    public void PerformBet(Ticket ticket, Lottery lottery, WWWOperations.OnObjectFecthed<ResponseMessage> callback)
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
                string[] split = fetchedData.Message.Split('/');
                Bet bet = new Bet {drawingId = int.Parse(split[0]), id = int.Parse(split[1])};

                BetPerformed(bet);
            }
        }

        callback(fetchedData, error);
    }

    private static string GetUrlStringForBet(Ticket ticket, Lottery lottery)
    {
        return string.Format("{0}/Bets/Bet/{1}?ticketId={2}", WWWOperations.instance.ServerUrl, lottery.Data.id,
            ticket.id);
    }
}

public class Bet : EventArgs
{
    public int id;
    public int drawingId;
}