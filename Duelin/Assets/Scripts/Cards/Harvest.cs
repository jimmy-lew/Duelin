using UnityEngine;

public class Harvest : Consumable
{
    public override int Cost() => 4;
    protected override void HandleEffect(GameObject target = null) => LocalInventory.getInstance().AddGameState(LocalInventory.GAME_STATE.HARVEST, 2);
}