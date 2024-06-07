using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePlay : UICanvas
{
    private bool isFirstTimeOpen = true;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private ButtonCustom jumpButton;
    [SerializeField] private ButtonCustom sprintButton;
    [SerializeField] private TouchField touchField;
    public override void Setup()
    {
        base.Setup();
        if (isFirstTimeOpen)
        {
            isFirstTimeOpen = false;
            jumpButton.button.onClick.AddListener(() =>
            {
                InputManager.Instance.OnClickBtnJump?.Invoke();
            });
        }
        InputManager.Instance.OnAssignTouchField?.Invoke(touchField);
    }

    public Vector3 GetMoveDirection()
    {
        return new Vector3(joystick.Horizontal, 0, joystick.Vertical);
    }
    
}
