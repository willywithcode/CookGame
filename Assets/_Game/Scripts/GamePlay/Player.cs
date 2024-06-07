using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum TypeOfMove 
{
    Walk,
    Run, 
    Jump
}
public class Player : ACacheMonoBehauviour
{
    [SerializeField, BoxGroup("Speed")] private float speedWalk;
    [SerializeField, BoxGroup("Speed")] private float speedRun;
    [SerializeField, BoxGroup("Speed")] private float speedMoveOnJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator anim;
    [SerializeField] private StateMachine<Player> stateMachine;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private float maxHeightStep;
    [SerializeField] private float gravityScale;
    [SerializeField] private float speedSlip;
    [SerializeField] private Button jumpButton;
    public bool isSprint;
    private string currentAnim;
    private float speed;
    private float turnToSmoothTime = 0.1f;
    private float speedFall = 0;
    private Vector3 dirSlip = Vector3.zero;
    // Defind states 
    public IdleState idleState = new IdleState();
    public MoveState moveState = new MoveState();
    public JumpState jumpState = new JumpState();
    public SlipState slipState = new SlipState();
    public FallState fallState = new FallState();

    private void Awake()
    {
        InputManager.Instance.OnClickBtnJump += this.CheckChangeStateJump;
    }

    private void Start()
    {
        this.ChangeState(idleState);
    }   

    private void Update()
    {
        stateMachine.ExecuteState();
    }
    public void ChangeAnim(string nextAnim )
    {
        if (nextAnim != currentAnim)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = nextAnim;
            anim.SetTrigger(currentAnim);
        }
    }

    public void ChangeState(IState<Player> newState)
    {
        stateMachine.ChangeState(newState);
        
    }
    public Vector3 GetMoveDirection()
    {
        return InputManager.Instance.GetMoveDirection();
    }

    public void SetupSpeed(TypeOfMove type)
    {
        if (type == TypeOfMove.Jump)
        {
            speed = speedMoveOnJump;
        }
        else if(type == TypeOfMove.Walk)
        {
            speed = speedWalk;
        }
        else if(type == TypeOfMove.Run)
        {
            speed = speedRun;
        }
    }
    public void Move()
    {
        if(Vector3.Distance(this.GetMoveDirection(), Vector3.zero) <= 0.1f)
            return;
        Tuple<Vector3, float> directionLocal = this.GetDirectionLocal();
        TF.position += new Vector3(directionLocal.Item1.x, 0, directionLocal.Item1.z) * (speed * Time.deltaTime);
    }

    public void Rotate()
    {
        Tuple<Vector3, float> directionLocal = this.GetDirectionLocal();
        TF.rotation = Quaternion.Euler(0, directionLocal.Item2, 0);
    }

    public Tuple<Vector3, float> GetDirectionLocal()
    {
        Vector3 direction = this.GetMoveDirection().normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnToSmoothTime, 0.1f);
        Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward.normalized;
        return new Tuple<Vector3, float>(moveDir, angle);
    }
    #region MoveState
    public void SetYAxis()
    {
        Vector3 checkPoint = TF.position + capsuleCollider.center;
        RaycastHit hit;
        if (Physics.Raycast( checkPoint + capsuleCollider.radius * this.GetDirectionLocal().Item1, Vector3.down, out hit, capsuleCollider.height/2 + maxHeightStep , groundLayer))
        {
            if(hit.point.y + capsuleCollider.height/2 - TF.position.y > maxHeightStep)
            {
                return;
            }

            if (Physics.Raycast(checkPoint, Vector3.down,
                    out hit, capsuleCollider.height / 2 + maxHeightStep, groundLayer))
            {
                Vector3 newPos = new Vector3(TF.position.x, hit.point.y + capsuleCollider.height / 2, TF.position.z);
                TF.position = newPos;
            }
        }
        this.CheckFall();

    }

    public void ChangeTypeMove(bool isRun)
    {
        isSprint = isRun;
    }

    public void CheckRun()
    {
        if (isSprint)
        {
            speed = speedRun;
            this.ChangeAnim(Constant.ANIM_RUN_STRING);
        }
        else
        {
            speed = speedWalk;
            this.ChangeAnim(Constant.ANIM_WALK_STRING);
        }
    }

    public void CheckChangeStateJump()
    {
        if (stateMachine.CompareCurrentState(idleState) || stateMachine.CompareCurrentState(moveState))
        {
            this.ChangeState(jumpState);
        }
    }
    public void CheckFall()
    {
        float stepValue = 360.0f / 4f;
        for (int i = 0; i < 4; i++)
        {
            float angle = i * stepValue;
            Vector3 checkpointDir = Quaternion.Euler(0, angle, 0) * Vector3.forward.normalized;
            Vector3 checkPoint =  TF.position + capsuleCollider.center;
            RaycastHit hit;
            if (Physics.Raycast(checkPoint + capsuleCollider.radius * checkpointDir, Vector3.down, out hit, capsuleCollider.height / 2 + maxHeightStep,
                    groundLayer))
            {
                return;
            }
        }
        this.ChangeState(fallState);
    }

    public bool CheckCollide()
    {
        List<Vector3> listLiftPoint = new List<Vector3>
        {
            Vector3.zero,
            new Vector3(-GetDirectionLocal().Item1.z, GetDirectionLocal().Item1.y, GetDirectionLocal().Item1.x),
            new Vector3(GetDirectionLocal().Item1.z, GetDirectionLocal().Item1.y, -GetDirectionLocal().Item1.x)
        };
        Vector3 checkPoint =  TF.position + capsuleCollider.center + (capsuleCollider.height/2 - maxHeightStep) * Vector3.down;
        for (int i = 0; i < 3; i++)
        {
            if(Physics.Raycast(checkPoint + listLiftPoint[i] * capsuleCollider.radius,
                   GetDirectionLocal().Item1, out RaycastHit hit, capsuleCollider.radius, groundLayer))
            {
                Vector3 newPos = hit.point - listLiftPoint[i] * capsuleCollider.radius - this.GetDirectionLocal().Item1 * capsuleCollider.radius;
                newPos.y = TF.position.y;
                TF.position = newPos;
                return true;
            }
        }
        checkPoint = TF.position + capsuleCollider.center;
        for (int i = 0; i < 3; i++)
        {
            if(Physics.Raycast(checkPoint + listLiftPoint[i] * capsuleCollider.radius,
                   GetDirectionLocal().Item1, out RaycastHit hit, capsuleCollider.radius, groundLayer))
            {
                Vector3 newPos = hit.point - listLiftPoint[i] * capsuleCollider.radius - this.GetDirectionLocal().Item1 * capsuleCollider.radius;
                newPos.y = TF.position.y;
                TF.position = newPos;
                return true;
            }
        }
        checkPoint =  TF.position + capsuleCollider.center + capsuleCollider.height/2  * Vector3.up;
        for (int i = 0; i < 3; i++)
        {
            if(Physics.Raycast(checkPoint + listLiftPoint[i] * capsuleCollider.radius,
                   GetDirectionLocal().Item1, out RaycastHit hit, capsuleCollider.radius, groundLayer))
            {
                Vector3 newPos = hit.point - listLiftPoint[i] * capsuleCollider.radius - this.GetDirectionLocal().Item1 * capsuleCollider.radius;
                newPos.y = TF.position.y;
                TF.position = newPos;
                return true;
            }
        }

        return false;
    }
    public void MoveInMoveState()
    {
        this.Rotate();
        this.CheckRun();
        if (!CheckCollide())
        {
            this.SetYAxis();
            this.Move();
            
        }
    }

    #endregion MoveState

    #region Gravity
    public void AddGravity()
    {
        TF.position += new Vector3(0, speedFall * Time.deltaTime, 0);
        speedFall += gravityScale * Time.deltaTime;
    }

    public void AddInitationVelocity()
    {
        speedFall = jumpForce;
    }

    #endregion

    #region JumpState
    public void Jump()
    {
        this.AddGravity();
        if (CheckOnGround(out RaycastHit hit))
        {
            /*if (CheckEncounterSlipSlope(out Vector3 dir))
            {
                Debug.Log("jump to slip slope");
                dirSlip = dir;
                this.ChangeState(slipState);
                return;
            }*/
            Vector3 newPos = new Vector3(TF.position.x, hit.point.y + capsuleCollider.height / 2, TF.position.z);
            TF.position = newPos;
            if(Vector3.Distance(this.GetMoveDirection(), Vector3.zero) <= 0.1f) this.ChangeState(idleState);
            else this.ChangeState(moveState);
        }
    }

    public bool CheckEncounterSlipSlope(out Vector3 dir)
    {
        dir = Vector3.zero;
        float minHeight = (TF.position + capsuleCollider.center).y - capsuleCollider.height / 2;
        float stepValue = 360.0f / 20f;
        for (int i = 0; i <= 20; i++)
        {
            float angle = i * stepValue;
            Vector3 checkpointDir = Quaternion.Euler(0, angle, 0) * Vector3.forward.normalized;
            Vector3 checkPoint =  TF.position + capsuleCollider.center;
            RaycastHit hit;
            if (Physics.Raycast(checkPoint + capsuleCollider.radius * checkpointDir, Vector3.down, out hit, (capsuleCollider.height / 2 + maxHeightStep) * 2,
                    groundLayer))
            {
                if((TF.position + capsuleCollider.center).y - hit.point.y < capsuleCollider.height / 2)
                if(hit.point.y < minHeight)
                {
                    minHeight = hit.point.y;
                    dir = checkpointDir;
                }
            }
        }
        Debug.Log(Mathf.Abs(minHeight + capsuleCollider.height / 2 - (TF.position + capsuleCollider.center).y));
        if (Mathf.Abs(minHeight + capsuleCollider.height / 2 - (TF.position + capsuleCollider.center).y) >
            maxHeightStep * 0.9f)
            return true;
        return false;
    }

    public bool CheckOnGround(out RaycastHit outHit)
    {
        RaycastHit hit;
        if (Physics.Raycast(capsuleCollider.center+TF.position, Vector3.down, out hit, capsuleCollider.height / 2,
                groundLayer))
        {
            outHit = hit;
            return true;
        }

        outHit = hit;
        return false;
    }

    #endregion
    
    #region FallState

    public void Fall()
    {
        this.AddGravity();
        if (CheckOnGround(out RaycastHit hit))
        {
            Vector3 newPos = new Vector3(TF.position.x, hit.point.y + capsuleCollider.height / 2, TF.position.z);
            TF.position = newPos;
            if(Vector3.Distance(this.GetMoveDirection(), Vector3.zero) <= 0.1f) this.ChangeState(idleState);
            else this.ChangeState(moveState);
        }
    }

    public void StartFall()
    {
        speedFall = 0;
    }
    #endregion FallState

    #region SlipState

    public void Slip()
    {
        TF.position += new Vector3(dirSlip.x, 0, dirSlip.z) * (speedSlip * Time.deltaTime);
        Vector3 checkPoint =  TF.position + capsuleCollider.center;
        RaycastHit hit;
        if (Physics.Raycast( checkPoint + dirSlip * capsuleCollider.radius, Vector3.down, out hit, capsuleCollider.height/2 + maxHeightStep, groundLayer))
        {
            Vector3 newPos = new Vector3(TF.position.x, hit.point.y + capsuleCollider.height / 2, TF.position.z);
            TF.position = newPos;
            if(Mathf.Abs(hit.point.y + capsuleCollider.height/2 - TF.position.y) <= 0.1f)
            {
                this.ChangeState(idleState);
            }
        }
    }
    

    #endregion SlipState
    
}
