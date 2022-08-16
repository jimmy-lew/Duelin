using UnityEngine;

public class Armored : Tank
{
    public override int Cost() => 2;

    public override void OnDamage(BaseGoblin attackingEntity, Vector3 targetPos)
    {
        if (attackingEntity.Damage > 0)
        {
            BaseGoblin copy = attackingEntity;
            copy.Damage -= 1;
            base.OnDamage(copy, targetPos);
        }
    }
}
