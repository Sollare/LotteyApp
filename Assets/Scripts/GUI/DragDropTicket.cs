using UnityEngine;
using System.Collections;

public class DragDropTicket : UIDragDropItem
{
    private TicketsGrid _grid;

	// Use this for initialization
	void Awake ()
	{
	    _grid = NGUITools.FindInParents<TicketsGrid>(transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
