using UnityEngine;
using System.Collections;

public class OverScreenPanel : MonoBehaviour
{
    public UISprite Sprite;

    private string winSprite = "you_win";
    private string lostSprite = "try_again";

    void OnPress(bool isPressed)
    {
        if(UICamera.currentTouch.current == gameObject)
            LotteryViewController.instance.OverScreen.SetVisible(false);
    }

    public void SetResult(bool win)
    {
        Sprite.spriteName = win ? winSprite : lostSprite;
    }

    public void SetVisible(bool show)
    {
        if (show)
        {
            gameObject.SetActive(true);
            TweenAlpha.Begin(gameObject, 0.3f, 1f);
        }
        else
        {
            var tweener = TweenAlpha.Begin(gameObject, 0.3f, 0f);

            tweener.onFinished.Clear();
            tweener.AddOnFinished(new EventDelegate.Callback(delegate
            {
                gameObject.SetActive(false);
            }));
        }
    }
}