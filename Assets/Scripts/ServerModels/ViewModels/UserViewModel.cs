using System;


    public class Drawing
    {
        public int id;
        public string name;
        public int totalmoney;
        public DateTime expiration;
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

    public class Bet
    {
        public int id;
        public int drawingId;
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
