using UnityEngine;

public class Preparation : Enhancement
{
    public override int Cost() => 2;
    protected override void HandleEffect(GameObject target)
    {
        BaseGoblin targetGoblin = target.GetComponent<BaseGoblin>();
        if(!isEligible(gameTilemap.WorldToCell(new Vector3(targetGoblin.transform.position.x, targetGoblin.transform.position.y - 0.16f, targetGoblin.transform.position.z)))) return;

        targetGoblin.InitialHealth += 1;
        targetGoblin.AddHealth(1);
        targetGoblin.Damage += 1;
    }
}