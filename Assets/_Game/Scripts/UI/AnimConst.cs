using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class AnimConst
{
    public static void DoClickButton(this RectTransform transform, System.Action action = null)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        var Btn = transform.GetComponent<Button>();
        Btn.enabled = false;
        transform.DOScale(SIZE_CLICK_BUTTON, TIME_CLICK_BUTTON).OnComplete(() =>
        {
            Btn.enabled = true;
            action?.Invoke();
        }).SetLoops(2, LoopType.Yoyo);
    }

    public static Vector3 SIZE_CLICK_BUTTON = Vector3.one * 1.1f;
    public static float TIME_CLICK_BUTTON = 0.1f;

    public static Vector3 SIZE_SACLE_ITEM = Vector3.one * 0.9f;
    public static float TIME_SACLE_ITEM = 0.1f;
}
