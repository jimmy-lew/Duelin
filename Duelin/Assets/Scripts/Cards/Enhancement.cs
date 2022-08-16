using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enhancement : Targetable
{
    protected override bool isEligible(Vector3Int targetPos)
    {
        Vector3 worldPos = gameTilemap.CellToWorld(targetPos);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);
        bool isOccupied = hit.collider != null && hit.collider.name != "spawn(Clone)";
        bool isSameTeam = isOccupied && (hit.collider.gameObject.GetComponent<PhotonView>()?.IsMine ?? false);

        Debug.Log(isSameTeam);

        return isOccupied && isSameTeam;
    }
}
