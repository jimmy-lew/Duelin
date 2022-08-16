using UnityEngine;
using Photon.Pun;

public class AttackSpell : Targetable
{
    protected override bool isEligible(Vector3Int targetPos)
    {  
        Vector3 worldPos = gameTilemap.CellToWorld(targetPos);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);
        
        bool isOccupied = hit.collider != null;
        bool isSameTeam = isOccupied && (hit.collider.gameObject.GetComponent<PhotonView>()?.IsMine ?? false);
        bool canAttack = isOccupied && !isSameTeam;

        return canAttack;
    }
}
