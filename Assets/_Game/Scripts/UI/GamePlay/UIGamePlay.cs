using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    private bool isFirstTimeOpen = true;
    private bool canPressJumpBtn = true;

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
    [SerializeField] private TouchField touchField;
    [SerializeField] private PickupItemScreen pickupItemScreen;
    [SerializeField] private ButtonCustom cookButton;
    public PickupItemScreen PickupItemScreen => pickupItemScreen;
    private Vector3 moveDirection;
    private Vector3 previousMoveDirection;
    private Tween toggleTween;
    public override void Setup()
    {
        base.Setup();
        FSTActionPlayer stateMachine = GameManager.Instance.Player.actionStateMachine;
        ToggleCookButton(!stateMachine.CompareCurrentState(stateMachine.noActionUpperState));
        if (isFirstTimeOpen)
        {
            isFirstTimeOpen = false;
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
        cookButton.gameObject.SetActive(false);
        cookButton.button.onClick.AddListener(() =>
        {
            cookButton.gameObject.SetActive(false);
        });
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
        cookButton.customButtonOnClick = null;
        cookButton.customButtonOnClick += action;
    }

    public void ToggleButtonInteractItem(bool state)
    {
        throwItemBtn.gameObject.SetActive(state);
        storeItemBtn.gameObject.SetActive(state);
    }
}
