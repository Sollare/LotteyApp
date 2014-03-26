using System;


    [Serializable]
    public class LotteryData : EventArgs
    {
        public int id;
        public string name;
        public int totalmoney = -1;
        public double expiration;
        public LotteryType type;
        public Bet[] MyBets;

        public enum LotteryType
        {
            Daily,
            Week,
            Instant
        }
    }

    public class UserInfo
    {
        public int id;
        public string name;
        public string email;

        public Ticket[] freeTickets;
        public Bet[] bets;
        public Win[] victories;
    }
    
    public class FreeTicketViewModel
    {
        public int id;
    }

    public class Win
    {
        public int? drawingId;
        public int totalMoney;
    }
