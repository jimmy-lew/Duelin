using UnityEngine;

#nullable enable
public abstract class Entity : MonoBehaviour
{
    public abstract void OnDamage(BaseGoblin attackingEntity, Vector3 targetPos);
    public abstract void OnDeath(BaseGoblin? attackingEntity = null, Vector3? targetPos = null);
}