using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackHoleSkillController currentBlackHole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(hotKeyPrefab, player.transform.position, Quaternion.identity);

       currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();

       currentBlackHole.SetupBlackHole(maxSize,growSpeed,shrinkSpeed,amountOfAttack,cloneCooldown,blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

    }

    public bool SkillCompleted()
    {
        if(!currentBlackHole)
        {
            return false;
        }

        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        return false;

    }


    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
