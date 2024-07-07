using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIRecipe : UICanvas
{
    [SerializeField] private RecipeLine recipeLinePrefab;
    [SerializeField] private Transform container;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform groupRecipe;
    [SerializeField] private RectTransform title;
    [SerializeField] private RectTransform backBtn;
    private bool isFirstTime = false;
    public override void Setup()
    {
        base.Setup();
        this.EffectOpen();
        if (!isFirstTime)
        {
            isFirstTime = true;
            List<Recipe> listRecipe = SaveGameManager.Instance.dataItemContainer.recipes;
            for(int i = 0; i < listRecipe.Count; i++)
            {
                RecipeLine recipeLine = Instantiate(recipeLinePrefab, container);
                List<DataItem> listIngredients = new List<DataItem>();
                List<int> listQuantity = new List<int>();
                listRecipe[i].GetIngredients(out listIngredients, out listQuantity);
                DataItem resultItem = listRecipe[i].GetResult();
                recipeLine.Setup(listIngredients, listQuantity, resultItem);
            }
        }
    }
    public void TurnOffRecipe()
    {
        canvasGroup.interactable = false;
        Vector2 tempPosGroupRecipe = groupRecipe.anchoredPosition;
        Vector2 tempPosTitle = title.anchoredPosition;
        Vector2 tempPosBackBtn = backBtn.anchoredPosition;
        groupRecipe.DOAnchorPosY(groupRecipe.anchoredPosition.y - 100, 0.5f).SetEase(Ease.InOutBack);
        title.DOAnchorPosY(title.anchoredPosition.y + 100, 0.5f).SetEase(Ease.InOutBack);
        backBtn.DOAnchorPosX(backBtn.anchoredPosition.x, 0.5f).SetEase(Ease.InOutBack).onComplete += () =>
        {
            canvasGroup.interactable = true;
            this.CloseDirectly();
            groupRecipe.anchoredPosition = tempPosGroupRecipe;
            title.anchoredPosition = tempPosTitle;
            backBtn.anchoredPosition = tempPosBackBtn;
        };
    }
    public void EffectOpen()
    {
        canvasGroup.interactable = false;
        Vector2 tempPosGroupRecipe = groupRecipe.anchoredPosition;
        Vector2 tempPosTitle = title.anchoredPosition;
        Vector2 tempPosBackBtn = backBtn.anchoredPosition;
        groupRecipe.anchoredPosition = new Vector2(groupRecipe.anchoredPosition.x, groupRecipe.anchoredPosition.y - 100);
        title.anchoredPosition = new Vector2(title.anchoredPosition.x, title.anchoredPosition.y + 100);
        backBtn.anchoredPosition = new Vector2(backBtn.anchoredPosition.x - 100, backBtn.anchoredPosition.y);
        groupRecipe.DOAnchorPosY(tempPosGroupRecipe.y, 0.5f).SetEase(Ease.InOutBack);
        title.DOAnchorPosY(tempPosTitle.y, 0.5f).SetEase(Ease.InOutBack);
        backBtn.DOAnchorPosX(tempPosBackBtn.x, 0.5f).SetEase(Ease.InOutBack).onComplete += () =>
        {
            canvasGroup.interactable = true;
        };
        
    }
}
