using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class SpawnManager : MonoBehaviour, IClickable
{
    public static SpawnManager getInstance() => GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
    Tilemap spawnHighlightMap;
    Tile spawnHighlight;
    SelectionActions _input;
    SelectionActions.InputActions _inputAction;
    Camera _camera;
    List<Vector3> playerSpawnPoints = new List<Vector3>();
    List<List<Vector3>> SpawnPoints = new List<List<Vector3>>{
        new List<Vector3>{
            new Vector3 (0.16f, -0.4f, 0),  // Bottom Left Right
            new Vector3 (0, -0.32f, 0),     // Bottom Left Middle
            new Vector3 (-0.16f, -0.4f, 0)  // Bottom Left Left
        },
        new List<Vector3>{
            new Vector3 (0.16f, 0.72f, 0),  // Top Right Right
            new Vector3 (0, 0.64f, 0),      // Top Right Middle
            new Vector3 (-0.16f, 0.72f, 0)  // Top Right Left
        },
        new List<Vector3>{
            new Vector3 (-1.12f, 0.24f, 0), // Top Left Left
            new Vector3 (-0.96f, 0.16f, 0), // Top Left Middle
            new Vector3 (-1.12f, 0.08f, 0)  // Top Left Right
        },
        new List<Vector3>{
            new Vector3 (1.12f, 0.08f, 0),  // Bottom Right Left
            new Vector3 (0.96f, 0.16f, 0),  // Bottom Right Middle
            new Vector3 (1.12f, 0.24f, 0)   // Bottom Right Right
        },
    };

    void Awake()
    {
        _input = new SelectionActions();
        _inputAction = _input.Input;
        _camera = Camera.main;
    }
    void OnEnable() 
    {
        _input.Enable();
    }
    void OnDisable() 
    {
        _input.Disable();
    }
    void Start()
    { 
        playerSpawnPoints = SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber-1];
        spawnHighlightMap = GetComponent<Tilemap>();
        spawnHighlight = TilemapRepository.getInstance().GetTile(TilemapRepository.SPAWN_TILE);
    }

    public bool HasSpawnPoints() => playerSpawnPoints.Any(spawnPoint => canSpawn(spawnPoint));
    public bool IsSpawnOccupied() => playerSpawnPoints.Any(spawnPoint => !canSpawn(spawnPoint));
    public void DisplaySpawnableTiles() 
    {
        Clear();
        playerSpawnPoints.ForEach(spawnPoint => {
            if (!canSpawn(spawnPoint)) return;
            Vector3Int gridPos = spawnHighlightMap.WorldToCell(spawnPoint);
            spawnHighlightMap.SetTile(new Vector3Int(gridPos.x -= 1, gridPos.y -= 1, gridPos.z), spawnHighlight);
        });
    }

    public void Clear() => spawnHighlightMap.ClearAllTiles();

    bool canSpawn(Vector3 targetPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(targetPos.x, targetPos.y), Vector2.zero);
        bool isOccupied = hit.collider != null && hit.collider.name != "Tilemap - Highlight [SpawnPoints]" && hit.collider.name != "spawn(Clone)";
        return !isOccupied;
    }

    private void SpawnUnit(GameObject unit, Vector3 position) => PhotonNetwork.Instantiate($"Prefabs/Units/{unit.name}", position, Quaternion.identity);
    public void OnClick(GameObject prevSelection = null)
    {
        bool isItemCard = prevSelection != null && prevSelection.GetComponent<ItemCard>() != null;
        bool isUnitItemCard = isItemCard && prevSelection.GetComponent<ItemCard>().type == ItemCard.ItemType.UNIT;
        if (!isItemCard || !isUnitItemCard) return;

        Vector2 mousePos = _inputAction.Pos.ReadValue<Vector2>();
        mousePos = _camera.ScreenToWorldPoint(mousePos);
        Vector3Int gridPos = spawnHighlightMap.WorldToCell((Vector3) mousePos);
        Vector3 spawnPos = spawnHighlightMap.CellToWorld(gridPos);

        if (!canSpawn(spawnPos)) return;

        ItemCard itemCard = prevSelection.GetComponent<ItemCard>();

        itemCard.Deselect();
        GameObject item = ShopManager.getInstance().Purchase(itemCard.item);

        if (item == null) return;

        SpawnUnit(itemCard.item, new Vector3(spawnPos.x, spawnPos.y += 0.16f, spawnPos.z));
        GameManager.getInstance().AudioPlayer("SFXVol", "Spawn");
        Destroy(itemCard);
        ShopManager.getInstance().RenderItemsForSale();
    }
}