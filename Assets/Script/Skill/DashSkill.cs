using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    public override void UseSkill()
    {

        base.UseSkill();
        Debug.Log("Dash used, create clone behind");
    }
}