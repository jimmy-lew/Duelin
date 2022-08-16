using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : AttackSpell
{
    public override int Cost() => 2;
    protected override void HandleEffect(GameObject target = null)
    {
        BaseGoblin targetGoblin = target.GetComponent<BaseGoblin>();
        if(!isEligible(gameTilemap.WorldToCell(new Vector3(targetGoblin.transform.position.x, targetGoblin.transform.position.y - 0.16f, targetGoblin.transform.position.z)))) return;

        targetGoblin.AddStatus(BaseGoblin.STATUS.SILENCED, 2);
    }
}
