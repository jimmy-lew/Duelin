using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityActionManager : MonoBehaviour
{
    private BaseGoblin entity;
    private EntityActions _actions;
    private EntityActions.MainActions _mainAction;
    private EntityMovementController _movementController;
    private EntityAttackController _attackController;
    private EntityAbilityController _abilityController;
    private List<EntityController> _controllerList = new List<EntityController>();

    private void Awake() {
        _actions = new EntityActions();
        _mainAction = _actions.Main;
    }

    private void OnEnable() {
        _actions.Enable();
    }

    private void OnDisable() {
        _actions.Disable();
    }

    void Start()
    {
        entity = GetComponent<BaseGoblin>();
        _movementController = EntityMovementController.Create(gameObject, entity, this);
        _attackController   = EntityAttackController.Create(gameObject, entity, this);
        _abilityController  = EntityAbilityController.Create(gameObject, entity, this);

        _controllerList.AddRange(new List<EntityController>{
            _attackController,
            _movementController,
            _abilityController
        });
    }

    public bool Select()
    {
        EntityController.clicks = 0;
        entity.isMovementBlocked = false;

        entity.unit_card.GetComponent<UnitCard>().RenderCard(entity, true);

        _controllerList.ForEach(controller => {
            string ACTION = Utility.GetEnumDescription(controller.ACTION_TYPE);
            _mainAction.Get().FindAction(ACTION).performed += controller.HandleAction;
        });

        return true;
    }

    public bool Deselect()
    {
        entity.ClearEntitiesInRange();
        entity.Clear();
        entity.unit_card.GetComponent<UnitCard>().RenderCard(entity);
        
        _controllerList.ForEach(controller => {
            string ACTION = Utility.GetEnumDescription(controller.ACTION_TYPE);
            _mainAction.Get().FindAction(ACTION).performed -= controller.HandleAction;
            controller.Clear();
        });

        GameObject.FindGameObjectsWithTag("Highlight").ForEach(obj => {
            obj.GetComponent<Tilemap>().ClearAllTiles();
        });

        entity.isSelected = false;

        return false;
    }

    public void UpdateController()
    {
        
    }
}