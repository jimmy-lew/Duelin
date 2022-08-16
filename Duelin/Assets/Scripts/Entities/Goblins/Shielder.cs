using System;
using UnityEngine;
using Photon.Pun;

public class Shielder : Tank, IActiveAbility
{
    public override int Cost() => 3;

    public void HandleActive(GameObject targetEntity, Vector3Int targetPos)
    {
        photonView.RPC("RunActive", RpcTarget.All, GAME_MAP.CellToWorld(targetPos));
    }

    [PunRPC]
    private void RunActive(Vector3 targetPos)
    {
        Collider2D hit = Physics2D.Raycast(new Vector2(targetPos.x, targetPos.y += 0.16f), Vector2.zero).collider;
        GameObject targetEntity = hit.gameObject;
        targetEntity.GetComponent<BaseGoblin>().AddStatus(BaseGoblin.STATUS.PARALYZED, 2);
    }

    public bool isActive(Vector3Int targetPos)
    {
        Vector3 worldPos = GAME_MAP.CellToWorld(targetPos);
        int dist = (int) Math.Ceiling(Vector3.Distance(GAME_MAP.WorldToCell(GetCurrentPos()), targetPos));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);
        
        bool inRange = dist <= 1 && dist != 0;
        bool isOccupied = hit.collider != null;
        bool isSameTeam = isOccupied && (hit.collider.gameObject.GetComponent<PhotonView>()?.IsMine ?? false);
        bool canAttack = inRange && isOccupied && !isSameTeam;

        return canAttack;
    }

    public string ActiveAbilityDescription() => "Paralyze a unit for 2 turns";
}