using System.Linq;
using UnityEngine;
using System.Collections;

public class InstantLotterySlider : UISlider
{
    [SerializeField]
    public int[] values;

    [SerializeField]
    public UISprite[] sprites;

    public GameObject Container;

    public string spriteNamePop = "game's_icon_1";
    public string spriteNameTube = "game's_icon_2";

    private bool availiable = false;

    void Awake()
    {
        TicketsController.instance.OnTicketViewActivated += TicketActivated;
    }

    private void TicketActivated(DragDropTicket ticket, LotteryItem item)
    {
        if (item.LoadLotteryOfType == LotteryData.LotteryType.Instant)
        {
            values = new int[3];

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Random.Range(0, 2);

                sprites[i].spriteName = (values[i] == 0) ? spriteNamePop : spriteNameTube;
            }

            SetVisible(true);
        }
    }

    protected override void OnPressBackground(GameObject go, bool isPressed)
    {
        //Debug.Log("Press: " + isPressed);

        //if (isPressed) return;

        //base.OnPressBackground(go, isPressed);
    }

    protected override void OnDragBackground(GameObject go, Vector2 delta)
    {
        value -= UICamera.currentTouch.delta.x / (foregroundWidget.width * 2f);

        if (availiable && value <= 0.1f)
        {
            CheckForVictory();
        }

    }

    void CheckForVictory()
    {
        LotteryController.instance.View.CloseLotteryDetails();
        SetVisible(false);


        if (values.Distinct().Count() == 1)
        {
            WinController.instance.CallLotteryWinner(new Lottery(LotteriesScrollView.instance.currentItem.lotteryInstance));
        }
        else
        {
            WinController.instance.CallLotteryLost(new Lottery(LotteriesScrollView.instance.currentItem.lotteryInstance));
        }
    }

    void SetVisible(bool visible)
    {
        availiable = visible;
        value = 1f;

        if (visible)
        {
            Container.SetActive(false);

            var tweener = TweenAlpha.Begin(gameObject, 0.3f, 1f);
            tweener.onFinished.Clear();
            tweener.onFinished.Add(new EventDelegate(delegate
            {
                Container.SetActive(true);
            }));
        }
        else
        {
            Container.SetActive(false);
            var tweener = TweenAlpha.Begin(gameObject, 0.3f, 0f);
            tweener.onFinished.Clear();
        }
    }
}
