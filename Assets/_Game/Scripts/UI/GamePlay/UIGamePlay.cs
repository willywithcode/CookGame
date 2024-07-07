using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    private bool isFirstTimeOpen = true;
    private bool canPressJumpBtn = true;
    [SerializeField] private SelectedItemContainer selectedItemContainer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private RectTransform[] positions;
    [SerializeField] private Image toogleImg;
    [SerializeField] private RectTransform handle;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private ButtonCustom jumpButton;
    [SerializeField] private ButtonCustom sprintButton;
    [SerializeField] private ButtonCustom inventoryButton;
    [SerializeField] private ButtonCustom attackButton;
    [SerializeField] private ButtonCustom throwItemBtn;
    [SerializeField] private ButtonCustom storeItemBtn;
    [SerializeField] private ButtonCustom interactFarmButton;
    [SerializeField] private ButtonCustom teleBtn;
    [SerializeField] private ButtonCustom viewStoreBtn;
    [SerializeField] private ButtonCustom recipeBtn;
    [SerializeField] private ButtonCustom orderBtn;
    [SerializeField] private ButtonCustom cuttingBtn;
    [SerializeField] private ButtonCustom trashBtn;
    [SerializeField] private ButtonCustom fishingBtn;
    [SerializeField] private TouchField touchField;
    [SerializeField] private PickupItemScreen pickupItemScreen;
    [SerializeField] private ButtonCustom cookButton;
    [SerializeField] private TextMeshProUGUI currentCoinTxt;
    [SerializeField] private Image farmInteractIcon;
    [SerializeField] private Sprite waterIcon;
    [SerializeField] private Sprite harvestIcon;
    [SerializeField] private Sprite plantIcon;
    [SerializeField] private TextMeshProUGUI textPopUp;
    public PickupItemScreen PickupItemScreen => pickupItemScreen;
    private Vector3 moveDirection;
    private Vector3 previousMoveDirection;
    private Tween toggleTween;
    private Tween tweenIncressCoin;
    private Tween tweenEffect;
    private bool isAssignCookBtnAction = false;
    public override void Setup()
    {
        base.Setup();
        FSTActionPlayer stateMachine = GameManager.Instance.Player.actionStateMachine;
        selectedItemContainer.SetUp();
        currentCoinTxt.text = SaveGameManager.Instance.CurrentCoin.ToString();
        if (isFirstTimeOpen)
        {
            isFirstTimeOpen = false;
            SoundManager.Instance.PlayBgMusic(SoundManager.DataSound.bgm);
            selectedItemContainer.HandleOnClickedItem();
            interactFarmButton.gameObject.SetActive(false);
            teleBtn.gameObject.SetActive(false);
            viewStoreBtn.gameObject.SetActive(false);
            orderBtn.gameObject.SetActive(false);
            cuttingBtn.gameObject.SetActive(false);
            trashBtn.gameObject.SetActive(false);
            fishingBtn.gameObject.SetActive(false);
            sprintButton.customButtonDown += () =>
            {
                toggleTween?.Kill();
                if (InputManager.Instance.isSprint)
                {

                    toggleTween = handle.DOLocalMove(positions[0].localPosition, 0.3f).OnComplete(() =>
                    {
                        InputManager.Instance.isSprint = false;
                        toogleImg.sprite = sprites[0];
                    });
                }
                else
                {
                    toggleTween = handle.DOLocalMove(positions[1].localPosition, 0.3f).OnComplete(() =>
                    {
                        InputManager.Instance.isSprint = true;
                        toogleImg.sprite = sprites[1];
                    });
                }
            };
            
            jumpButton.customButtonDown += () =>
            {
                InputManager.Instance.isJump = true;
            };
            jumpButton.customButtonUp += () =>
            {
                InputManager.Instance.isJump = false;
                InputManager.Instance.canPressJumpBtn = true;
            };
            attackButton.customButtonDown += () =>
            {
                InputManager.Instance.isAttack = true;
            };
            attackButton.customButtonUp += () =>
            {
                InputManager.Instance.isAttack = false;
                InputManager.Instance.canPressAttackBtn = true;
            };
            throwItemBtn.customButtonDown += () =>
            {
                InputManager.Instance.isThrowItem = true;
            };
            throwItemBtn.customButtonUp += () =>
            {
                InputManager.Instance.isThrowItem = false;
                InputManager.Instance.canPressThrowItemBtn = true;
            };
            storeItemBtn.customButtonDown += () =>
            {
                InputManager.Instance.isStoreItem = true;
            };
            storeItemBtn.customButtonUp += () =>
            {
                InputManager.Instance.isStoreItem = false;
                InputManager.Instance.canPressStoreItemBtn = true;
            };
            
        }
        InputManager.Instance.OnAssignTouchField?.Invoke(touchField);
#if UNITY_EDITOR
        joystick.gameObject.SetActive(false);
        jumpButton.gameObject.SetActive(false);
        sprintButton.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
#endif
    }

    private void Update()
    {
        if(Vector3.Distance(moveDirection, Vector3.zero) >= 0.01f) previousMoveDirection = moveDirection;
#if UNITY_EDITOR
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
#endif
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
    public Vector3 GetPreviousMoveDirection()
    {
        return previousMoveDirection;
    }
    public void OpenInventory()
    {
        UIManager.Instance.OpenUI<UIInventory>();
    }
    public void ToggleCookButton(bool state)
    {
        cookButton.gameObject.SetActive(state);
    }
    public void AddCookButtonEvent(UnityAction action)
    {
        cookButton.customButtonOnClick += action;
    }
    public void AddFarmInteractEvent(UnityAction action)
    {
        interactFarmButton.customButtonOnClick += action;
    }
    public void AddCuttingEvent(UnityAction action)
    {
        cuttingBtn.customButtonOnClick += action;
    }
    public void AddTrashBinEvent(UnityAction action)
    {
        trashBtn.customButtonOnClick += action;
    }
    public void AddFishingEvent(UnityAction action)
    {
        fishingBtn.customButtonOnClick += action;
    }
    public void AddTeleportEvent(UnityAction action)
    {
        teleBtn.customButtonOnClick += action;
    }

    public void ToggleButtonInteractItem(bool state)
    {
        throwItemBtn.gameObject.SetActive(state);
        storeItemBtn.gameObject.SetActive(state);
    }
    public void ToggleButtonInteractFarm(bool state)
    {
        interactFarmButton.gameObject.SetActive(state);
    }
    public void ToggleButtonFishingSite(bool state)
    {
        fishingBtn.gameObject.SetActive(state);
    }
    public SelectedItem GetCurrentSelectedItem()
    {
        return selectedItemContainer.CurrentSelectedItem;
    }

    public void ToggleAllInteractButton(bool state)
    {
        ToggleButtonInteractFarm(state);
        ToggleButtonInteractItem(state);
        ToggleCookButton(state);
    }
    public void ToggleButtonTeleport(bool state)
    {
        teleBtn.gameObject.SetActive(state);
    }
    public void ToggleButtonViewStore(bool state)
    {
        viewStoreBtn.gameObject.SetActive(state);
    }
    public void ToggleButtonTrashBin(bool state)
    {
        trashBtn.gameObject.SetActive(state);
    }

    public void ToggleButtonOrder(bool state)
    {
        orderBtn.gameObject.SetActive(state);
    }
    public void ToggleButtonKnifeTable(bool state)
    {
        cuttingBtn.gameObject.SetActive(state);
    }
    public void OpenSetting()
    {
        UIManager.Instance.OpenUI<UISetting>();
    }
    public void OpenRecipe()
    {
        UIManager.Instance.OpenUI<UIRecipe>();
    }
    public void ViewStore()
    {
        UIManager.Instance.OpenUI<UIShop>();
    }
    public void OpenOrder()
    {
        UIManager.Instance.OpenUI<UIOrder>();
    }
    public void TweenIncressCoin(int coin)
    {
        SaveGameManager.Instance.CurrentCoin += coin;
        currentCoinTxt.TweenIncressCoin(
            SaveGameManager.Instance.CurrentCoin - coin, 
            SaveGameManager.Instance.CurrentCoin, 1f);
    }
    public void ChangeIconFarmInteract(FarmInteractType type)
    {
        switch (type)
        {
            case FarmInteractType.Water:
                farmInteractIcon.sprite = waterIcon;
                break;
            case FarmInteractType.Harvest:
                farmInteractIcon.sprite = harvestIcon;
                break;
            case FarmInteractType.Plant:
                farmInteractIcon.sprite = plantIcon;
                break;
        }
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
public enum FarmInteractType
{
    Water,
    Harvest,
    Plant
}
