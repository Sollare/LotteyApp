using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;

public class TicketsScrollView : UIScrollView {

	// Use this for initialization
	void Start () {
	
	}

    private Vector3 move;
	
	// Update is called once per frame
    public override void MoveRelative(Vector3 relative)
    {
        base.MoveRelative(relative);

        move += relative;
       // print(move);

        if (move.y > 140)
        {
            Press(false);
            move = Vector3.zero;
        }

        
    }

    public void OnClick()
    {
        print("clicked");
    }

    public void OnPress(bool isDown)
    {
        Debug.Log("Released");
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
