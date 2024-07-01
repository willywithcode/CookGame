using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    public UnityAction<TouchField> OnAssignTouchField;
    public UnityAction<TouchField> OnAssignTouchFieldPlayerUI;
    public bool isSprint = false;
    public bool canPressJumpBtn = true;
    public bool isJump = false;
    public bool isAttack = false;
    public bool canPressAttackBtn = true;
    public bool isThrowItem = false;
    public bool canPressThrowItemBtn = true;
    public bool isStoreItem = false;
    public bool canPressStoreItemBtn = true;
    
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
        if (canPressAttackBtn && isAttack)
        {
            canPressAttackBtn = false;
            return true;
        }
        return false;
    }

    public bool IsInteract()
    {
        #if UNITY_EDITOR
        return Input.GetKey(KeyCode.F);
        #endif
        if(canPressThrowItemBtn && isThrowItem)
        {
            canPressThrowItemBtn = false;
            return true;
        }
        return false;
    }
    public bool IsStore()
    {
        #if UNITY_EDITOR
        return Input.GetKey(KeyCode.G);
        #endif
        if (canPressStoreItemBtn && isStoreItem)
        {
            canPressStoreItemBtn = false;
            return true;
        }
        return false;
    }

}
