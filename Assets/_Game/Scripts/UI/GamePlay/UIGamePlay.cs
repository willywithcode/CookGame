using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGamePlay : UICanvas
{
    private bool isFirstTimeOpen = true;
    private bool canPressJumpBtn = true;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private ButtonCustom jumpButton;
    [SerializeField] private ButtonCustom sprintButton;
    [SerializeField] private TouchField touchField;
    private Vector3 moveDirection;
    private Vector3 previousMoveDirection;
    public override void Setup()
    {
        base.Setup();
        if (isFirstTimeOpen)
        {
            isFirstTimeOpen = false;
            sprintButton.customButtonDown += () =>
            {
                InputManager.Instance.isSprint = true;
            };
            sprintButton.customButtonUp += () =>
            {
                InputManager.Instance.isSprint = false;
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
        }
        InputManager.Instance.OnAssignTouchField?.Invoke(touchField);
#if UNITY_EDITOR
        joystick.gameObject.SetActive(false);
        jumpButton.gameObject.SetActive(false);
        sprintButton.gameObject.SetActive(false);
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
    
}
