using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tank : BaseGoblin, IPassiveAbility
{
    protected Tilemap GAME_MAP;
    private Tilemap PASSIVE_MAP;   
    private Tile PASSIVE_TILE;
    
    new protected void Start() {
        base.Start();
        GAME_MAP = TilemapRepository.getInstance().GetTilemap(TilemapRepository.GAME_MAP);
        PASSIVE_MAP = TilemapRepository.getInstance().GetTilemap(TilemapRepository.PASSIVE_MAP);
        PASSIVE_TILE = TilemapRepository.getInstance().GetTile(TilemapRepository.PASSIVE_TILE);
    }
    
    public override void OnDeath(BaseGoblin attackingEntity, Vector3? targetPos = null)
    {
        base.OnDeath(attackingEntity, targetPos);
        Clear();
    }

    private void displayTauntedTiles()
    {
        for(int x = GAME_MAP.cellBounds.min.x; x <= GAME_MAP.cellBounds.max.x; x++) {
            for(int y = GAME_MAP.cellBounds.min.y; y <= GAME_MAP.cellBounds.max.y; y++) {
                if (!isTaunted(new Vector3Int(x,y,0))) continue;
                PASSIVE_MAP.SetTile(new Vector3Int(x,y,0), PASSIVE_TILE);
            }
        }
    }   

    public override void Clear()
    {
        PASSIVE_MAP.ClearAllTiles();
    }

    private bool isTaunted(Vector3Int targetPos)
    {  
        Vector3 worldPos = GAME_MAP.CellToWorld(targetPos);
        int dist = (int) Math.Ceiling(Vector3.Distance(GAME_MAP.WorldToCell(GetCurrentPos()), targetPos));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);
        
        bool inRange = dist <= 1 && dist != 0;
        bool hasTile = GAME_MAP.HasTile(targetPos);

        return inRange && hasTile;
    }

    public void HandlePassive()
    {
        Clear();
        displayTauntedTiles();
    }

    public string PassiveAbilityDescription() => "Units in range can only attack this unit";
}