using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using Photon.Pun;

public class EntityAttackController : EntityController
{
    private Tilemap gameTilemap;
    private Tilemap attackHighlightMap;
    private Tile attackHighlight;
    private BaseGoblin entity;
    private EntityActionManager actionManager;
    
    

    public static EntityAttackController Create(GameObject parent, BaseGoblin entity, EntityActionManager actionManager)
    {
        EntityAttackController _attackController = parent.AddComponent<EntityAttackController>();
        _attackController.entity = entity;
        _attackController.actionManager = actionManager;
        _attackController.ACTION_TYPE = ACTION.LEFT_CLICK;
        return _attackController;
    }

    private void Start() {
        gameTilemap = TilemapRepository.getInstance().GetTilemap(TilemapRepository.GAME_MAP);
        attackHighlightMap = TilemapRepository.getInstance().GetTilemap(TilemapRepository.ATTACK_MAP);
        attackHighlight = TilemapRepository.getInstance().GetTile(TilemapRepository.ATTACK_TILE);
    }
    
    public override void HandleAction(InputAction.CallbackContext context)
    {
        clicks++;
        displayAttackableTiles();
        if (clicks <= 2) return;
        if (TurnManager.getInstance().bonusActionTaken) return;

        Vector2 mousePos = context.action.actionMap.FindAction(MOUSE_POS).ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int gridPos = gameTilemap.WorldToCell((Vector3) mousePos);

        if(!canAttack(gridPos)) return;

        PhotonView.Get(this).RPC($"AttackEntity", RpcTarget.All, (Vector3) mousePos);
    }

    [PunRPC]
    private void AttackEntity(Vector3 targetPos)
    {
        Vector3Int gridPos = gameTilemap.WorldToCell(targetPos);
        Vector3 worldPos = gameTilemap.CellToWorld(gridPos);

        GameObject targetEntity = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero).collider.gameObject;
        Entity target = targetEntity.GetComponent<Entity>();

        TurnManager.getInstance().HandleTurnAction(TurnManager.ACTION.BONUS_ACTION);
        target.OnDamage(entity, targetPos);
        actionManager.Deselect();

        GameManager.getInstance().AudioPlayer("SFXVol", "Hit");
    }

    public override void Clear()
    {
        attackHighlightMap.ClearAllTiles();
    }

    public void displayAttackableTiles()
    {
        for(int x = gameTilemap.cellBounds.min.x; x <= gameTilemap.cellBounds.max.x; x++) {
            for(int y = gameTilemap.cellBounds.min.y; y <= gameTilemap.cellBounds.max.y; y++) {
                if (!canAttack(new Vector3Int(x,y,0))) continue;
                attackHighlightMap.SetTile(new Vector3Int(x,y,0), attackHighlight);
            }
        }
    }
    
    private bool canAttack(Vector3Int targetPos)
    {  
        Vector3 worldPos = gameTilemap.CellToWorld(targetPos);
        int dist = (int) Math.Ceiling(Vector3.Distance(gameTilemap.WorldToCell(entity.GetCurrentPos()), targetPos));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y += 0.16f), Vector2.zero);
        
        bool inRange = dist <= entity.Range && dist != 0;
        bool isOccupied = hit.collider != null && hit.collider.name != "Tilemap - Highlight [SpawnPoints]";
        bool isSameTeam = isOccupied && (hit.collider.gameObject.GetComponent<PhotonView>()?.IsMine ?? false);
        bool canAttack = inRange && isOccupied && !isSameTeam;
        
        if(canAttack) entity.AddEntityToRange(hit.collider.gameObject.GetComponent<Entity>());

        return canAttack;
    }
}