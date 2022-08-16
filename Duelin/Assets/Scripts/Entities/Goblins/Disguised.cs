using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Photon.Pun;


public class Disguised : BaseGoblin, IActiveAbility
{
    private Tilemap GAME_MAP;
    public override int Cost() => 4;
    new protected void Start() {
        base.Start();
        GAME_MAP = TilemapRepository.getInstance().GetTilemap(TilemapRepository.GAME_MAP);
    }

    public bool isActive(Vector3Int targetPos)
    {
        Vector3 worldPos = GAME_MAP.CellToWorld(targetPos);
        int dist = (int) Math.Ceiling(Vector3.Distance(GAME_MAP.WorldToCell(GetCurrentPos()), targetPos));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);
        
        bool isOccupied = hit.collider != null;
        bool inRange = dist <= 9 && dist != 0;
        bool isUnit = isOccupied && hit.collider.gameObject.GetComponent<BaseGoblin>() != null;
        bool isSameTeam = isOccupied && (hit.collider.gameObject.GetComponent<PhotonView>()?.IsMine ?? false);
        bool canCopy = isOccupied && !isSameTeam && isUnit && inRange;

        return canCopy;
    }

    public void HandleActive(GameObject targetEntity, Vector3Int targetPos)
    {

    }

    public string ActiveAbilityDescription() => "Adopt the abilities of another unit for this turn";
}
