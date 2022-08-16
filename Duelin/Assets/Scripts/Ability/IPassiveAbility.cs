using UnityEngine.Tilemaps;

public interface IPassiveAbility
{
    public static Tilemap GAME_MAP = TilemapRepository.getInstance().GetTilemap(TilemapRepository.GAME_MAP);
    public static readonly string MOUSE_POS = "POS";
    public string PassiveAbilityDescription();
    public void HandlePassive();
}

public static class PassiveAbilityExtensions
{
    public static void UsePassive(this IPassiveAbility entity)
    {
        entity.HandlePassive();
    }
}