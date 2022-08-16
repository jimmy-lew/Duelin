using UnityEngine;
using Photon.Pun;

public class Spawn : Entity
{
    public override void OnDamage(BaseGoblin attackingEntity, Vector3 targetPos)
    {
        OnDeath();
    }

    public override void OnDeath(BaseGoblin attackingEntity = null, Vector3? targetPos = null)
    {
        if (!PhotonView.Get(this).IsMine) return;
        GameManager.getInstance().HandleWin();
    }
}