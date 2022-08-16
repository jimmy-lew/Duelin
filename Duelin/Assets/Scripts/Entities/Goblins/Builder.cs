using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class Builder : BaseGoblin, IActiveAbility
{
    private Tilemap GAME_MAP;
    private Tilemap WALL_MAP;
    private Tile WALL;
    public override int Cost() => 2;

    new protected void Start()
    {
        base.Start();
        GAME_MAP = TilemapRepository.getInstance().GetTilemap(TilemapRepository.GAME_MAP);
        WALL_MAP = TilemapRepository.getInstance().GetTilemap(TilemapRepository.WALL_MAP);
        WALL = TilemapRepository.getInstance().GetTile(TilemapRepository.WALL_TILE);
    }

    public bool isActive(Vector3Int targetPos)
    {
        Vector3 worldPos = GAME_MAP.CellToWorld(targetPos);
        int dist = (int) Math.Ceiling(Vector3.Distance(GAME_MAP.WorldToCell(GetCurrentPos()), targetPos));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);

        bool hasTile = GAME_MAP.HasTile(targetPos);
        bool inRange = dist <= 1 && dist != 0;
        bool isOccupied = hit.collider != null;
        bool canBuild = hasTile && inRange && !isOccupied;

        return canBuild;
    }
    
    public void HandleActive(GameObject targetEntity, Vector3Int targetPos)
    {
        photonView.RPC("RunActive", RpcTarget.All, GAME_MAP.CellToWorld(targetPos));
    }

    [PunRPC]
    private void RunActive(Vector3 targetPos)
    {
        Vector3Int gridPos = GAME_MAP.WorldToCell(targetPos);
        WALL_MAP.SetTile(new Vector3Int(gridPos.x, gridPos.y, 2), WALL);
    }

    public string ActiveAbilityDescription() => "Build 1 wall each turn";
}