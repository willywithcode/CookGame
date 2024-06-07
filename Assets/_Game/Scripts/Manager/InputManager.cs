using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    public UnityAction OnClickBtnJump;
    public UnityAction<TouchField> OnAssignTouchField;
    public Vector3 GetMoveDirection()
    {
        return UIManager.Instance.GetUI<UIGamePlay>().GetMoveDirection();
    }
}
