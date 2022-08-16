using System;
using UnityEngine;
using Photon.Pun;

public class Barrel : Tank, IActiveAbility
{
    public override int Cost() => 2;

    public override void OnDeath(BaseGoblin attackingEntity, Vector3? targetPos = null)
    {
        base.OnDeath(attackingEntity, targetPos);
        Clear();
    }

    public void HandleActive(GameObject targetEntity, Vector3Int targetPos)
    {
        photonView.RPC("RunActive", RpcTarget.All, GAME_MAP.CellToWorld(targetPos));
    }

    [PunRPC]
    private void RunActive(Vector3 targetPos)
    {
        Collider2D hit = Physics2D.Raycast(new Vector2(targetPos.x, targetPos.y += 0.16f), Vector2.zero).collider;
        GameObject targetEntity = hit.gameObject;
        targetEntity.GetComponent<BaseGoblin>().AddStatus(BaseGoblin.STATUS.SLOWED, 2);
        targetEntity.GetComponent<BaseGoblin>().MovementRange -= 1;
    }

    public bool isActive(Vector3Int targetPos)
    {  
        Vector3 worldPos = GAME_MAP.CellToWorld(targetPos);
        int dist = (int) Math.Ceiling(Vector3.Distance(GAME_MAP.WorldToCell(GetCurrentPos()), targetPos));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);
        
        bool inRange = dist <= 2 && dist != 0;
        bool isOccupied = hit.collider != null;
        bool isSameTeam = isOccupied && (hit.collider.gameObject.GetComponent<PhotonView>()?.IsMine ?? false);
        bool canAttack = inRange && isOccupied && !isSameTeam;

        return canAttack;
    }

    public string ActiveAbilityDescription() => "-1 movement to 1 unit";
}