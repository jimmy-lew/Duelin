using UnityEngine;

public class Plague : Consumable
{
    public override int Cost() => 4;
    protected override void HandleEffect(GameObject target = null) => GameManager.getInstance().UpdateGameState(LocalInventory.GAME_STATE.PLAGUED, 2);
}
