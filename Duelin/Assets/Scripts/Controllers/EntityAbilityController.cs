using UnityEngine;
using UnityEngine.InputSystem;

public class EntityAbilityController : EntityController
{
    private BaseGoblin entity;
    private EntityActionManager actionManager;

    public static EntityAbilityController Create(GameObject parent, BaseGoblin entity, EntityActionManager actionManager)
    {
        EntityAbilityController _abilityController = parent.AddComponent<EntityAbilityController>();
        _abilityController.entity = entity;
        _abilityController.actionManager = actionManager;
        _abilityController.ACTION_TYPE = ACTION.RIGHT_CLICK;
        return _abilityController;
    }

    public override void HandleAction(InputAction.CallbackContext context)
    {
        if (TurnManager.getInstance().isFirstTurn) return;
        if (entity.HasStatus(BaseGoblin.STATUS.SILENCED)) return;
        if (entity.isAbilityUsed) return;
        if (entity is IActiveAbility) ((IActiveAbility)entity).UseActive(context);
    }
}