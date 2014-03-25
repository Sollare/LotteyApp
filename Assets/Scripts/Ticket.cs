using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class Ticket
{
    public int id;

    public static Ticket NewTicket()
    {
        return new Ticket() {id = UnityEngine.Random.Range(1, 1000)};
    }
}
