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
    public bool isAttack = false;
    public Vector3 GetMoveDirection()
    {
        return UIManager.Instance.GetUI<UIGamePlay>().GetMoveDirection();
    } 
    public Vector3 GetPreviousMoveDirection()
    {
        return UIManager.Instance.GetUI<UIGamePlay>().GetPreviousMoveDirection();
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
        return Input.GetKey(KeyCode.E);
#endif
        return isSprint;
    }

    public bool IsAttack()
    {
#if UNITY_EDITOR
        return Input.GetKey(KeyCode.R);
#endif
        return isAttack;
    }
    
}
