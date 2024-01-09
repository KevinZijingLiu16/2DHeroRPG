using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;

    private float defaultGravityScale;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolname) : base(_player, _stateMachine, _animBoolname)
    {
    }

    public override void AniamtionFinishTrigger()
    {
        base.AniamtionFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravityScale = player.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravityScale;
        player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
            
        }
        if (stateTimer < 0)
        {
           rb.velocity = new Vector2(0, -0.1f);
            if (!skillUsed)
            {
                if (player.skill.blackHole.CanUseSkill())
                {
                    skillUsed = true;
                }

               
            }
        }

        if (player.skill.blackHole.SkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }
      
    }
}
