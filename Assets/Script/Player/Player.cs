using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : Entity
{


    [Header("Attack Settings")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;

    public bool isBusy { get; private set; }

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public float swordReturnForce = 12f;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Settings")]
    
   
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public float dashDir { get; private set; }
    private float defaultDashSpeed;

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
  


  

 


    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }

    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }

    public PlayerAirState airState { get; private set; }

    public PlayerSlideWallState wallSlideState { get; private set; }

    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }



    public PlayerPrimaryAttackState PlayerPrimaryAttack { get; private set; }

    public PlayerCounterAttackState PlayerCounterAttack { get; private set; }

    public PlayerAimSwordState PlayerAimSword { get; private set; }

    public PlayerCatchSwordState PlayerCatchSword { get; private set; }

    public PlayerBlackHoleState PlayerBlackHole { get; private set; }

    public PlayerDeadState PlayerDeadState { get; private set; }



    #endregion


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerSlideWallState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        PlayerPrimaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        PlayerCounterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        PlayerAimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        PlayerCatchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        PlayerBlackHole = new PlayerBlackHoleState(this, stateMachine, "Jump");
        PlayerDeadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }




    protected override void Update() // use Player Monobehavior Update to run Update() from PlayerState, 
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
         
        if(Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }

    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage );
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);

    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;

    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;


    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(PlayerCatchSword);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds) //Coroutine to prevent player from spamming attack
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AniamtionFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }

        

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())

        {
           
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(PlayerDeadState);
    }



}