using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerSlideWallState : PlayerState
{
    public PlayerSlideWallState(Player _player, PlayerStateMachine _stateMachine, string _animBoolname) : base(_player, _stateMachine, _animBoolname)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

       

        if (xInput != 0 && player.facingDir != xInput)
        {
           
               stateMachine.ChangeState(player.idleState);
           
        }
        if (yInput < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
        rb.velocity = new Vector2(0, rb.velocity.y * .7f);

        }
           
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
