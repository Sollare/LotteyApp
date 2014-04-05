using UnityEditor;
using UnityEngine;
using System.Collections;

public class HelpPanel : MonoBehaviour
{
    private UIPanel panel;
    private bool visible = false;

    public UISprite ticketSilouette;
    public TicketsGrid grid;
    private Vector3 ticketInitialPosition;

    public UISprite buyTicket;
    public UILabel ticketLabel;

    public UISprite ticketArrow;

    void Awake()
    {
        ticketInitialPosition = ticketSilouette.cachedTransform.position;
        panel = GetComponent<UIPanel>();
    }

    void OnPress(bool isPressed)
    {
        if (isPressed == false && UICamera.currentTouch.current == gameObject)
            SetVisible(false);
    }

    public void Show()
    {
        SetVisible(true);
    }

    public void SetVisible(bool show)
    {
        if (show == visible) return;

        ticketArrow.enabled = show;
        visible = show;
        
        if (show)
        {

            var firstTicket = grid.FirstTicket;

            if (firstTicket)
            {
                ticketLabel.text = "Drag the selected ticket in the lottery field";
                ticketSilouette.cachedTransform.position = firstTicket.transform.position;
            }
            else
            {
                ticketLabel.text = "You have no tickets.\nBuy the one!";
                ticketSilouette.cachedTransform.position = buyTicket.cachedTransform.position;
            }

            gameObject.SetActive(true);
            panel.alpha = 0f;

            var tweener = TweenAlpha.Begin(gameObject, 0.3f, 1f);
            tweener.onFinished.Clear();
        }
        else
        {
            

            panel.alpha = 1f;

            var tweener = TweenAlpha.Begin(gameObject, 0.3f, 0f);

            tweener.onFinished.Clear();
            tweener.AddOnFinished(new EventDelegate.Callback(delegate
            {
                gameObject.SetActive(false);
            }));
        }
    }
}
