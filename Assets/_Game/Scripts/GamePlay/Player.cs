using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using ECM2;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : ACacheMonoBehauviour
{
    public Character character;
    public CharacterDataSO characterData;
    public FSTPlayer stateMachine;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private AnimancerComponent anim;
    private float turnToSmoothTime = 0.1f;
    private void Start()
    {
        stateMachine.ChangeState(stateMachine.idleState);
    }   
    public AnimancerState ChangeAnim(ClipTransition nextAnim)
    {
        return anim.Play(nextAnim);
    }
    public AnimancerState ChangeAnim(ClipTransition nextAnim, float transitionDuration)
    {
        return anim.Play(nextAnim, transitionDuration);
    }
    public void ChangeSpeed(float speed)
    {
        character.maxWalkSpeed = speed;
    }
    public Vector3 GetMoveDirection()
    {
        return InputManager.Instance.GetMoveDirection();
    }

    public void Rotate()
    {
        Tuple<Vector3, float> directionLocal = this.GetDirectionLocal();
        playerTransform.rotation = Quaternion.Euler(0, directionLocal.Item2, 0);
    }

    public Tuple<Vector3, float> GetDirectionLocal()
    {
        Vector3 direction = this.GetMoveDirection().normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnToSmoothTime, 0.1f);
        Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward.normalized;
        return new Tuple<Vector3, float>(moveDir, angle);
    }
    
    
}
