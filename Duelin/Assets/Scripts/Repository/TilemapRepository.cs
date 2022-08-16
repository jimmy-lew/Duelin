using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRepository
{
    private static TilemapRepository _instance = null;
    public static TilemapRepository getInstance() => _instance == null ? _instance = new TilemapRepository() : _instance;

    #region MapNames
    public static readonly string MINE_MAP = "Tilemap - Mines";
    public static readonly string WALL_MAP = "Tilemap - Walls";
    public static readonly string GAME_MAP = "Tilemap - GameMap";
    public static readonly string SPAWN_MAP = "Tilemap - Highlight [SpawnPoints]";
    public static readonly string ATTACK_MAP = "Tilemap - Highlight [Attack]";
    public static readonly string MOVEMENT_MAP = "Tilemap - Highlight [Movement]";
    public static readonly string ACTIVE_MAP = "Tilemap - Highlight [Active]";
    public static readonly string PASSIVE_MAP = "Tilemap - Highlight [Passive]";
    #endregion

    #region TileNames
    public static readonly string ATTACK_TILE = "attack_highlight"; 
    public static readonly string ACTIVE_TILE = "attack_highlight"; 
    public static readonly string MOVE_TILE = "movement_highlight";  
    public static readonly string SPAWN_TILE = "spawn_highlight";  
    public static readonly string PASSIVE_TILE = "passive_highlight";  
    public static readonly string WALL_TILE = "stone_wall";  
    #endregion

    TilemapRepository() {}

    public Tilemap GetTilemap(string tilemapName) => GameObject.Find(tilemapName).GetComponent<Tilemap>();
    public Tile GetTile(string tileName) => Resources.Load<Tile>($"Levels/Tiles/{tileName}");
}