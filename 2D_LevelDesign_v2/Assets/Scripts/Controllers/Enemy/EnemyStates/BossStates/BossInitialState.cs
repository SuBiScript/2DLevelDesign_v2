using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInitialState : BossState
{
    public BossInitialState(BossController bc)
    {
        AudioManager.instance.Stop("LevelMusic");
        AudioManager.instance.Stop("MenuMusic");
        AudioManager.instance.Play("BossMusic");
    }

    public override void Update(BossController bc)
    {
        if (bc.bossModel.following & !GameManager.gameMaster.camera.bossReady)
            bc.ChangeState(new BossFollowingState(bc));
    }

    public override void FixedUpdate(BossController bc)
    {

    }

    public override void CheckTransition(BossController bc)
    {
        base.BossTransitionState(bc);
    }
}
