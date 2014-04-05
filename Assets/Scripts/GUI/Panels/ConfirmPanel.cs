using UnityEngine;
using System.Collections;

public class ConfirmPanel : MonoBehaviour
{
    private UIPanel panel;

    private EventDelegate.Callback callback = null;
    private EventDelegate.Callback cancelCallback = null;

    public UILabel textLabel;

    private static ConfirmPanel _instance;

    public static ConfirmPanel instance
    {
        get
        {
            if (_instance == null)
                _instance = LotteryViewController.instance.ConfirmPanel;

            return _instance;
        }
    }

    void Awake()
    {
        panel = GetComponent<UIPanel>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        panel.alpha = 0f;

        var tweener = TweenAlpha.Begin(gameObject, 0.3f, 1f);
        tweener.onFinished.Clear();
    }

    public void Close()
    {
        if (cancelCallback != null)
            cancelCallback();

        panel.alpha = 1f;
        instance.callback = null;

        var tweener = TweenAlpha.Begin(gameObject, 0.3f, 0f);

        tweener.onFinished.Clear();
        tweener.onFinished.Add(new EventDelegate(delegate
        {
            gameObject.SetActive(false);
        }));
    }

    public void Confirm()
    {
        if (instance.callback != null)
            instance.callback();

        Close();
    }

    public static void ShowConfirmDialog(string text, EventDelegate.Callback confirmCallback)
    {
        instance.textLabel.text = text;

        instance.callback = confirmCallback;
        instance.cancelCallback = null;

        instance.Show();
    }

    public static void ShowConfirmDialog(string text, EventDelegate.Callback confirmCallback, EventDelegate.Callback cancelCallback)
    {
        instance.textLabel.text = text;

        instance.callback = confirmCallback;
        instance.cancelCallback = cancelCallback;

        instance.Show();
    }
}
