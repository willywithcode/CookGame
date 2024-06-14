using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    public UnityAction<TouchField> OnAssignTouchField;
    public bool isSprint = false;
    public bool canPressJumpBtn = true;
    public bool isJump = false;
    public Vector3 GetMoveDirection()
    {
#if UNITY_EDITOR
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
#endif
        return UIManager.Instance.GetUI<UIGamePlay>().GetMoveDirection();
    } 
    public bool IsJump()
    {
#if UNITY_EDITOR
        return Input.GetKey(KeyCode.Space);
#endif
        if (canPressJumpBtn && isJump)
        {
            canPressJumpBtn = false;
            return true;
        }
        return false;
    }
    public bool IsSprint()
    {
#if UNITY_EDITOR
        return Input.GetKey(KeyCode.R);
#endif
        return isSprint;
    }
}
