using UnityEngine;
using Photon.Pun;

public class Marauder : BaseGoblin, IActiveAbility
{
    public string ActiveAbilityDescription() => "Once every 4 turns, give all units +1 | +1";
    public override int Cost() => 7;

    // !Not working
    public void HandleActive(GameObject targetEntity, Vector3Int targetPos)
    {
        // object[] args = new object[LocalInventory.getInstance().GetEntityListSize()];
        // if (Cooldown > 0) return;
        // Cooldown += 4;
        // int index = 0;
        // LocalInventory.getInstance().GetGoblins().ForEach(goblin => {
        //     args[index] = goblin;
        //     index++;
        //     goblin.GetComponent<BaseGoblin>().AddHealth(1);
        //     goblin.GetComponent<BaseGoblin>().Damage += 1;
        // });     
        // photonView.RPC("RunActive", RpcTarget.All, args as object);   
    }

    // [PunRPC]
    // private void RunActive(object[] goblins)
    // {   
    //     goblins.ForEach(goblin => {
    //         ((GameObject)goblin).GetComponent<BaseGoblin>().AddHealth(1);
    //     });
    // }

    public bool isActive(Vector3Int targetPos) => false;
    public bool isTargetable() => false;
}