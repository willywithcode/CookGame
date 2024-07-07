using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIInventory : UICanvas
{
    [SerializeField] private int numRow;
    [SerializeField] private Transform[] content;
    [SerializeField] private InventoryItem inventoryItemPrefab;
    [SerializeField] private MouseFollower mouseFollower;
    [SerializeField] private DetailItemUI detailItemUI;
    [SerializeField] private InventorySelectedItem inventorySelectedItem;
    [SerializeField] private TouchField touchField;
    [SerializeField] private TextMeshProUGUI countItemHoldText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform view;
    [SerializeField] private float[] point;
    [SerializeField] private ButtonCustom backPageBtn;
    [SerializeField] private ButtonCustom nextPageBtn;
    [SerializeField] private ChangePageChecker backPageChecker;
    [SerializeField] private ChangePageChecker nextPageChecker;
    [SerializeField] private ButtonCustom tabAllBtn;
    [SerializeField] private ButtonCustom tabFoodBtn;
    [SerializeField] private ButtonCustom tabIngredientBtn;
    [SerializeField] private RectTransform focusBar;
    [SerializeField] private InventoryPage inventoryFoodPage;
    [SerializeField] private InventoryPage inventoryIngredientPage;
    [SerializeField] private ButtonCustom cancelAutoHoldingBtn;
    [SerializeField] private GameObject autoHoldingPanel;
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    private InventoryItem currentSelectedItem;
    private bool isFirstTime = true;
    private bool isHaveItemSelected = false;
    private Tween tween;
    private Tween tweenChangePage;
    private Tween tweenFocusBar;
    private Tween tweenBtnCancelAutoHolding;
    private int currentSlide = 0;
    private Vector3 tempPos;
    private bool isAutoHolding = false;
    
    public InventoryItem CurrentSelectedItem => currentSelectedItem;
    public override void Setup()
    {
        base.Setup();
        this.ChangeTextNumHoldItem(GameManager.Instance.Player.ListItemHold.Count);
        GameManager.Instance.RenUIPlayer.SpawnPlayer();
        this.SetFirstPage();
        this.SetFirstTab();
        this.ToggleAutoHolding(false);
        this.detailItemUI.SplitBtn.gameObject.SetActive(true);
        if (isFirstTime)
        {
            this.SetUpButtonTab();
            this.detailItemUI.SetInvisibleButton();
            this.detailItemUI.AddEventOnclickSplitBtn(SplitItem);
            this.detailItemUI.AddEventOnclickThrowBtn(ThrowItem);
            this.detailItemUI.AddEventOnclickThrowAllBtn(ThrowAllItem);
            this.detailItemUI.AddEventOnclickHoldBtn(HoldItem);
            this.cancelAutoHoldingBtn.customButtonOnClick += () =>
            {
                this.ToggleAutoHolding(false);
                tweenBtnCancelAutoHolding?.Kill();
            };
            tempPos = focusBar.localPosition;
            for(int k = 0; k < content.Length; k++)
            {
                for(int i = 0; i < numRow; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, content[k]);
                        inventoryItem.Setup();
                        inventoryItems.Add(inventoryItem);
                        this.SetupItemAction(inventoryItem);
                    }
                }
            }

            for (int i = 0; i < content.Length; i++)
            {
                
                content[i].SetParent(view);
            }
            inventorySelectedItem.Setup();
            this.TurnOffAllBorder();
            this.SetupItemAction(inventorySelectedItem);
            this.LoadData();
            InputManager.Instance.OnAssignTouchFieldPlayerUI?.Invoke(touchField);
        }
        else
        {
            tween.Kill();
            tween = this.canvasGroup.DOFade(1, 0.5f).From(0);
            this.canvasGroup.interactable = true;
        }
    }

    public override void CloseDirectly()
    {
        if (isFirstTime)
        {
            base.CloseDirectly();
            isFirstTime = false;
            return;
        }
        tween.Kill();
        this.canvasGroup.interactable = false;
        tween = this.canvasGroup.DOFade(0, 0.5f).From(1).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            if(IsDestroyOnClose) Destroy(this.gameObject);
        });
    }

    private void SetupItemAction(InventoryItem item)
    {
        item.onItemClicked += HandleItemClicked;
        item.onItemDropped += HandleItemDropped;
        item.onItemBeginDrag += HandleItemBeginDrag;
        item.onItemEndDrag += HandleItemEndDrag;
        item.onItemDrag += HandleItemDrag;
    }
    private void HandleItemClicked(InventoryItem inventoryItem)
    {
        if (!inventoryItem.HaveItem)
        {
            this.detailItemUI.ResetDetail();
            this.TurnOffAllBorder();
            this.currentSelectedItem = null;
            if(detailItemUI.IsButtonVisible) detailItemUI.DoMoveButtonsDown();
            return;
        }
        this.TurnOnNewBorder(inventoryItem);
        this.currentSelectedItem = inventoryItem;
        this.detailItemUI.SetDetail(inventoryItem.DataItem);
        detailItemUI.SetTextTitleFunctionButton(currentSelectedItem.DataItem.type);
        if (!detailItemUI.IsButtonVisible)
        {
            detailItemUI.DoMoveButtonsUp();
        }
        if(isAutoHolding) this.HoldItem();
    }
    public void HandleItemDropped(InventoryItem inventoryItem)
    {
        if(mouseFollower.CurrentInventoryItem == null || !mouseFollower.CurrentInventoryItem.HaveItem) return;
        this.SwapItem(mouseFollower.CurrentInventoryItem, inventoryItem);
        this.detailItemUI.DoMoveButtonsDown();
        this.Save();
    }
    private void HandleItemBeginDrag(InventoryItem inventoryItem)
    {
        if(!inventoryItem.HaveItem) return;
        if(isHaveItemSelected) return;
        isHaveItemSelected = true;
        this.TurnOnNewBorder(inventoryItem);
        this.currentSelectedItem = inventoryItem;
        this.detailItemUI.SetDetail(inventoryItem.DataItem);
        mouseFollower.gameObject.SetActive(true);
        mouseFollower.SetFollower(inventoryItem.DataItem, inventoryItem.QuantityItem, inventoryItem);
    }
    private void HandleItemEndDrag(InventoryItem inventoryItem)
    {
        mouseFollower.gameObject.SetActive(false);
        if (isHaveItemSelected && mouseFollower.CurrentInventoryItem == inventoryItem)
        {
            isHaveItemSelected = false;
        }
    }
    private void HandleItemDrag(InventoryItem inventoryItem)
    {
        if (!inventoryItem.HaveItem && isHaveItemSelected) return;
        mouseFollower.FollowMouse();
    }
    public void SwapItem(InventoryItem currentItem, InventoryItem targetItem)
    {
        if(currentItem == targetItem) return;  
        this.currentSelectedItem = null;
        this.detailItemUI.ResetDetail();
        this.TurnOffAllBorder();
        if (!targetItem.HaveItem)
        {
            targetItem.SetupItem(currentItem.DataItem, currentItem.QuantityItem);
            currentItem.RemoveItem();
            if(currentItem == inventorySelectedItem)
            {
                GameManager.Instance.RenUIPlayer.ResetItem();
            }
            return;
        }
        if (currentItem.DataItem != targetItem.DataItem 
            || !currentItem.DataItem.isStackable 
            || !targetItem.DataItem.isStackable)
        {
            DataItem dataItem = currentItem.DataItem;
            int quantity = currentItem.QuantityItem;
            currentItem.SetupItem(targetItem.DataItem, targetItem.QuantityItem);
            targetItem.SetupItem(dataItem, quantity);
            return;
        }
        int totalQuantity = currentItem.QuantityItem + targetItem.QuantityItem;
        if (totalQuantity > currentItem.DataItem.maxStack)
        {
            int reminder = totalQuantity - currentItem.DataItem.maxStack;
            targetItem.SetupItem(currentItem.DataItem, currentItem.DataItem.maxStack);
            currentItem.SetupItem(targetItem.DataItem, reminder);
            return;
        }
        targetItem.SetupItem(currentItem.DataItem, totalQuantity);
        currentItem.RemoveItem();
    }

    public void AddItemToInventory(int index, string dataName, int quantity)
    {
        if (index < 0 || index >= inventoryItems.Count) return;
        inventoryItems[index].SetupItem(SaveGameManager.GetDataItem(dataName), quantity);
        this.Save();
    }
    public bool AddItemToInventory(string dataNameItem, int quantity)
    {
        if(CheckHaveSameItemInventory( SaveGameManager.GetDataItem(dataNameItem), out int indexItem))
        {
            if(inventoryItems[indexItem].QuantityItem + quantity > inventoryItems[indexItem].DataItem.maxStack)
            {
                int reminder = inventoryItems[indexItem].QuantityItem + quantity - inventoryItems[indexItem].DataItem.maxStack;
                this.AddItemToInventory(indexItem, dataNameItem, inventoryItems[indexItem].DataItem.maxStack);
                if (CheckHaveEmptyInventory(out int index))
                {
                    AddItemToInventory(index, dataNameItem,  reminder);
                }
                else
                {
                    UIManager.Instance.GetUI<UIGamePlay>().PopUpText("Inventory is full");
                }
            }
            else
            {
                this.AddItemToInventory(indexItem,dataNameItem, inventoryItems[indexItem].QuantityItem + quantity);
            }

            return true;
        }
        else if (CheckHaveEmptyInventory(out int index))
        {
            this.AddItemToInventory(index,dataNameItem, inventoryItems[index].QuantityItem + quantity);
            return true;
        }
        UIManager.Instance.GetUI<UIGamePlay>().PopUpText("Inventory is full");
        return false;

    }
    public void ThrowItem() 
    {
        if (this.currentSelectedItem == null) return;
        currentSelectedItem.ThrowItem();
        if (currentSelectedItem.DataItem == null) currentSelectedItem = null;
        this.Save();
    }

    public void ThrowAllItem()
    {
        if (currentSelectedItem == null) return;
        currentSelectedItem.ThrowAllItem();
        currentSelectedItem = null;
        this.Save();
    }

    public void SplitItem()
    {
        if(currentSelectedItem == null) return;
        if (CheckHaveEmptyInventory(out int index))
        {
            int splitQuantity = currentSelectedItem.SplitItem();
            inventoryItems[index].SetupItem(currentSelectedItem.DataItem, splitQuantity);
        }
        this.Save();
    }

    public void HoldItem()
    {
        if(currentSelectedItem == null) return;
        if(currentSelectedItem.QuantityItem < 1) return;
        if(GameManager.Instance.Player.ListItemHold.Count >= 7) return;
        if (!isAutoHolding)
        {
            this.ToggleAutoHolding(true);
            tweenBtnCancelAutoHolding?.Kill();
            tweenBtnCancelAutoHolding = cancelAutoHoldingBtn.transform.DOScale(1.1f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo);
        }
        this.PostEvent(EventID.OnHoldingItem, this);
        if(currentSelectedItem.QuantityItem < 1)
        {
            currentSelectedItem = null;
            this.TurnOffAllBorder();
        }
        currentSelectedItem.Hold();
        this.Save();
    }

    public bool CheckHaveEmptyInventory(out int index)
    {
        index = -1;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (!inventoryItems[i].HaveItem)
            {
                index = i;
                return true;
            }
        }

        return false;
    }
    public bool CheckHaveSameItemInventory(DataItem type,out int index)
    {
        index = -1;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].HaveItem && inventoryItems[i].DataItem == type 
                                           && inventoryItems[i].DataItem.isStackable && inventoryItems[i].QuantityItem < type.maxStack)
            { 
                index = i;
                return true;
            }
        }
        return false;
    }

    public void TurnOffAllBorder()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].ToggleBorder(false);
        }
        inventorySelectedItem.ToggleBorder(false);
    }

    public void TurnOnNewBorder(InventoryItem inventoryItem)
    {
        this.TurnOffAllBorder();
        inventoryItem.ToggleBorder(true);
    }
    public void ChangeTextNumHoldItem(int num)
    {
        countItemHoldText.text = num.ToString() + "/7";
    }

    public void Save()
    {
        SaveGameManager.Instance.InventoryItems = new List<ItemData>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].HaveItem)
            {
                SaveGameManager.Instance.InventoryItems.Add(new ItemData(inventoryItems[i].DataItem.name,
                    inventoryItems[i].DataItem.isStackable ? inventoryItems[i].QuantityItem: 1) );
            }
            else
            {
                SaveGameManager.Instance.InventoryItems.Add(new ItemData("", 0));
            }
        }
        SaveGameManager.Instance.SaveData();
    }
    
    public void LoadData()
    {
        if (SaveGameManager.Instance.InventoryItems.Count <= 0)
        {
            var list = new List<ItemData>()
            {
                new ItemData("Seedtomato",99),
                new ItemData("Seedcabbage",99),
                new ItemData("Meat",1),
                new ItemData("Bread",99),
                new ItemData("Tomato",99),
                new ItemData("Cabbage",99),
                new ItemData("Sandwicha",99),
                new ItemData("Sandwichb",99),
                new ItemData("Sandwichc",99),
                new  ItemData("Burntmeat",99),
            };
            for(int i = 0;i < list.Count; i++)
            {
                this.AddItemToInventory(i, list[i].name, list[i].quantity);
            }
            return;
        }
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (SaveGameManager.Instance.InventoryItems[i].quantity > 0)
            {
                inventoryItems[i].SetupItem(SaveGameManager.GetDataItem(SaveGameManager.Instance.InventoryItems[i].name),
                    SaveGameManager.Instance.InventoryItems[i].quantity);
            }
        }
    }

    public void OpenUIGamePlay()
    {
        this.CloseDirectly();
        GameManager.ChangeState(GameState.Gameplay);
        UIManager.Instance.OpenUI<UIGamePlay>();
    }

    public void TurnToNextPage()
    {
        currentSlide++;
        if (currentSlide >= point.Length - 1)
        {
            currentSlide = point.Length - 1;
            nextPageBtn.gameObject.SetActive(false);
            nextPageChecker.gameObject.SetActive(false);
        }
        backPageBtn.gameObject.SetActive(true);
        backPageChecker.gameObject.SetActive(true);
        tweenChangePage?.Kill();
        tweenChangePage = view.DOAnchorPosX(point[currentSlide], 0.5f).SetEase(Ease.OutBack);
    }
    public void TurnToBackPage()
    {
        currentSlide--;
        if (currentSlide <= 0)
        {
            currentSlide = 0;
            backPageBtn.gameObject.SetActive(false);
            backPageChecker.gameObject.SetActive(false);
        }
        nextPageChecker.gameObject.SetActive(true);
        nextPageBtn.gameObject.SetActive(true);
        tweenChangePage?.Kill();
        tweenChangePage = view.DOAnchorPosX(point[currentSlide], 0.5f).SetEase(Ease.OutBack);
    }

    public void FocusTab(ButtonCustom button)
    {
        tweenFocusBar?.Kill();
        focusBar.SetParent(button.TF);
        tweenFocusBar = focusBar.DOLocalMove(tempPos, 0.5f).SetEase(Ease.OutBack);
    }

    public void SetUpButtonTab()
    {
        tabAllBtn.Btn.onClick.AddListener(() =>
        {
            detailItemUI.SplitBtn.gameObject.SetActive(true);
            FocusTab(tabAllBtn);
            this.ToggleViewAll(true);
            inventoryIngredientPage.ToggleContent(false);
            inventoryFoodPage.ToggleContent(false);
            this.SetFirstPage();
        });
        tabFoodBtn.Btn.onClick.AddListener(() =>
        {
            if(inventoryFoodPage.IsOpening) return;
            detailItemUI.SplitBtn.gameObject.SetActive(false);
            FocusTab(tabFoodBtn);
            ToggleViewAll(false);
            inventoryIngredientPage.ToggleContent(false);
            inventoryFoodPage.ToggleContent(true);
            this.Filter(ItemType.Food);
            this.SetFirstPage();
        });
        tabIngredientBtn.Btn.onClick.AddListener(() =>
        {
            if(inventoryIngredientPage.IsOpening) return;
            detailItemUI.SplitBtn.gameObject.SetActive(false);
            FocusTab(tabIngredientBtn);
            ToggleViewAll(false);
            inventoryIngredientPage.ToggleContent(true);
            inventoryFoodPage.ToggleContent(false);
            this.Filter(ItemType.Ingredient);
            this.SetFirstPage();
        });
    }
    public bool IsDraging()
    {
        return mouseFollower.gameObject.activeSelf;
    }

    public void Filter(ItemType type)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (type == ItemType.Ingredient)
            {
                if (inventoryItems[i].HaveItem && inventoryItems[i].DataItem.type == ItemType.Ingredient)
                {
                    inventoryIngredientPage.AddItem(inventoryItems[i], HandleItemClicked);
                }
            }
            else
            {
                if (inventoryItems[i].HaveItem && inventoryItems[i].DataItem.type == ItemType.Food)
                {
                    inventoryFoodPage.AddItem(inventoryItems[i], HandleItemClicked);
                }
            }
        }

        if (type == ItemType.Ingredient)
        {
            inventoryIngredientPage.FinishAddItem();
        }
        else
        {
            inventoryFoodPage.FinishAddItem();
        }
    }
    private void ToggleViewAll(bool state)
    {
        for (int i = 0; i < content.Length; i++)
        {
            content[i].gameObject.SetActive(state);
        }
    }
    public void SetFirstPage()
    {
        currentSlide = 0;
        view.anchoredPosition = new Vector2(point[0], 0);
        backPageBtn.gameObject.SetActive(false);
        backPageChecker.gameObject.SetActive(false);
        nextPageBtn.gameObject.SetActive(true);
        nextPageChecker.gameObject.SetActive(true);
    }
    public void SetFirstTab()
    {
        FocusTab(tabAllBtn);
        ToggleViewAll(true);
        inventoryIngredientPage.ToggleContent(false);
        inventoryFoodPage.ToggleContent(false);
    }

    public void ToggleAutoHolding(bool state)
    {
        isAutoHolding = state;
        cancelAutoHoldingBtn.gameObject.SetActive(state);
        autoHoldingPanel.SetActive(state);
    }
    public InventoryItem GetInventoryItem(int index)
    {
        return inventoryItems[index];
    }
 }
