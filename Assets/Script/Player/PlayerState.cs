using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player , PlayerStateMachine _stateMachine, string _animBoolname) //constructor
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolname;
    }

    public virtual void Enter()
    {
        //bug.Log("Enter" + animBoolName);
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
         xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit() 
    {
        //bug.Log("Exit" + animBoolName);
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AniamtionFinishTrigger() => triggerCalled = true;
        
}
