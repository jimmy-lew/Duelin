using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public interface IActiveAbility
{
    public static TilemapRepository REPO = TilemapRepository.getInstance();
    public static Tilemap GAME_MAP = REPO.GetTilemap(TilemapRepository.GAME_MAP);
    public static Tilemap ACTIVE_MAP = REPO.GetTilemap(TilemapRepository.ACTIVE_MAP); 
    public static Tile ACTIVE_TILE = REPO.GetTile(TilemapRepository.ACTIVE_TILE);
    public static readonly string MOUSE_POS = "POS";
    public string ActiveAbilityDescription();
    public virtual bool isTargetable() => true;
    public bool isActive(Vector3Int targetPos);
    public void HandleActive(GameObject targetEntity, Vector3Int targetPos);
}

public static class ActiveAbilityExtensions
{
    public static void UseActive(this IActiveAbility entity, InputAction.CallbackContext context)
    {
        if(entity.isTargetable()) entity.DisplayActiveTiles();

        Vector2 mousePos = context.action.actionMap.FindAction(IActiveAbility.MOUSE_POS).ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int gridPos = IActiveAbility.GAME_MAP.WorldToCell((Vector3) mousePos);
        
        if(entity.isTargetable() && !entity.isActive(gridPos)) return;
        Vector3 worldPos = IActiveAbility.GAME_MAP.CellToWorld(gridPos);
        Collider2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero).collider;
        GameObject targetEntity = hit == null ? Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y), Vector2.zero).collider?.gameObject : hit?.gameObject;

        entity.HandleActive(targetEntity, gridPos);
        if(entity is BaseGoblin) 
        {
            ((BaseGoblin)entity).isAbilityUsed = true;
            ((BaseGoblin)entity).actionManager.Deselect();
        }
    }

    private static void DisplayActiveTiles(this IActiveAbility entity)
    {
        for(int x = IActiveAbility.GAME_MAP.cellBounds.min.x; x <= IActiveAbility.GAME_MAP.cellBounds.max.x; x++)
        {
            for(int y = IActiveAbility.GAME_MAP.cellBounds.min.y; y <= IActiveAbility.GAME_MAP.cellBounds.max.y; y++)
            {
                if (!entity.isActive(new Vector3Int(x,y,0))) continue;
                IActiveAbility.ACTIVE_MAP.SetTile(new Vector3Int(x,y,0), IActiveAbility.ACTIVE_TILE);
            }
        }
    }

    public static void Clear(this IActiveAbility entity)
    {
        IActiveAbility.ACTIVE_MAP.ClearAllTiles();
    }
}