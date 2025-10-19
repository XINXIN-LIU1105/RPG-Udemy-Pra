using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (Time.time >= lastTimeAttacked + comboCounter)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);
        //player.anim.speed = 1.2f;

        #region Choose attack direction

        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        #endregion

        //����ʱ��΢�ƶ�
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;

    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);
        //player.anim.speed = 1;

        comboCounter = (++comboCounter) % 3;
        lastTimeAttacked = Time.time;

    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.zeroVelocity();

        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
