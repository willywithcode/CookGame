using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIShop : UICanvas
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform backBtn;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform title;
    [SerializeField] private TextMeshProUGUI textPopUp;
    private Tween tweenEffect;
    public override void Setup()
    {
        base.Setup();
        this.EffectOpen();
    }

    public void TurnOffShop()
    {
        canvasGroup.interactable = false;
        Vector2 tempPosGroupRecipe = container.anchoredPosition;
        Vector2 tempPosTitle = title.anchoredPosition;
        Vector2 tempPosBackBtn = backBtn.anchoredPosition;
        container.DOAnchorPosY(container.anchoredPosition.y - 100, 0.5f).SetEase(Ease.InOutBack);
        title.DOAnchorPosY(title.anchoredPosition.y + 100, 0.5f).SetEase(Ease.InOutBack);
        backBtn.DOAnchorPosX(backBtn.anchoredPosition.x, 0.5f).SetEase(Ease.InOutBack).onComplete += () =>
        {
            canvasGroup.interactable = true;
            this.CloseDirectly();
            container.anchoredPosition = tempPosGroupRecipe;
            title.anchoredPosition = tempPosTitle;
            backBtn.anchoredPosition = tempPosBackBtn;
        };
    }
    public void EffectOpen()
    {
        canvasGroup.interactable = false;
        Vector2 tempPosGroupRecipe = container.anchoredPosition;
        Vector2 tempPosTitle = title.anchoredPosition;
        Vector2 tempPosBackBtn = backBtn.anchoredPosition;
        container.anchoredPosition = new Vector2(container.anchoredPosition.x, container.anchoredPosition.y - 100);
        title.anchoredPosition = new Vector2(title.anchoredPosition.x, title.anchoredPosition.y + 100);
        backBtn.anchoredPosition = new Vector2(backBtn.anchoredPosition.x - 100, backBtn.anchoredPosition.y);
        container.DOAnchorPosY(tempPosGroupRecipe.y, 0.5f).SetEase(Ease.InOutBack);
        title.DOAnchorPosY(tempPosTitle.y, 0.5f).SetEase(Ease.InOutBack);
        backBtn.DOAnchorPosX(tempPosBackBtn.x, 0.5f).SetEase(Ease.InOutBack).onComplete += () =>
        {
            canvasGroup.interactable = true;
        };
        
    }
    public void PopUpText(string popUpContent)
    {
        tweenEffect?.Kill();
        textPopUp.text = popUpContent;
        textPopUp.gameObject.SetActive(true);
        Tween tweenFade = textPopUp.DOFade(0, 3f);
        tweenFade.onComplete += () =>
        {
            textPopUp.gameObject.SetActive(false);
            textPopUp.color = new Color(textPopUp.color.r, textPopUp.color.g, textPopUp.color.b, 1);
        };
        tweenFade.onKill += () =>
        {
            textPopUp.gameObject.SetActive(false);
            textPopUp.color = new Color(textPopUp.color.r, textPopUp.color.g, textPopUp.color.b, 1);
        };
        float tempY = textPopUp.transform.localPosition.y;
        Tween tweenMove = textPopUp.transform.DOLocalMoveY(tempY + 100, 3f);
        tweenMove.onComplete += () =>
        {
            textPopUp.transform.localPosition = new Vector3(textPopUp.transform.localPosition.x, tempY, textPopUp.transform.localPosition.z);
        };
        tweenMove.onKill += () =>
        {
            textPopUp.transform.localPosition = new Vector3(textPopUp.transform.localPosition.x, tempY, textPopUp.transform.localPosition.z);
        };
        tweenEffect = DOTween.Sequence().Append(tweenFade).Join(tweenMove);
    }
}
