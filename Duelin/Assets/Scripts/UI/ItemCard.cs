using TMPro;
using UnityEngine;

public class ItemCard : UIElement, IClickable, IHoverable
{
    private bool isSelected = false;
    public GameObject item;
    public ItemType type;

    public void OnClick(GameObject prevSelection = null)
    {
        bool isSelectionCard = prevSelection != null && prevSelection.GetComponent<Card>() != null;
        if (isSelectionCard) prevSelection.GetComponent<Card>().Deselect();
        if (TurnManager.getInstance().itemPurchased) return;
        bool canDeselect = prevSelection != null && prevSelection.GetComponent<ItemCard>() != null && prevSelection.GetComponent<ItemCard>() != this;
        bool isSelectionGoblin = prevSelection != null && prevSelection.GetComponent<BaseGoblin>() != null;
        bool isSelectionSpawnTile = prevSelection != null && prevSelection.GetComponent<SpawnManager>() != null;

        // TODO: Migrate deselection & selection to interface

        if (isSelectionGoblin) prevSelection.GetComponent<BaseGoblin>().actionManager.Deselect();
        if (isSelectionSpawnTile) prevSelection.GetComponent<SpawnManager>().Clear();
        if (canDeselect) prevSelection.GetComponent<ItemCard>().Deselect();
        isSelected = isSelected ? Deselect() : Select();
    }
    
    public void OnHover(GameObject prevHover = null)
    {
        Debug.Log("Hovering");
    }

    private bool Select()
    {
        Utility.RenderSprite(transform, $"unit_card_collapsed_selected", "item_card");
        if (type == ItemType.UNIT) SpawnManager.getInstance().DisplaySpawnableTiles();
        else if (type == ItemType.CARD) GameObject.FindWithTag("BuyButton").GetComponent<SpriteRenderer>().enabled = true;

        DisplayItemInfo();

        return true;
    }

    public bool Deselect()
    {
        Utility.RenderSprite(transform, $"unit_card_collapsed", "item_card");
        SpawnManager.getInstance().Clear();
        if (type == ItemType.CARD) GameObject.FindWithTag("BuyButton").GetComponent<SpriteRenderer>().enabled = false;

        HideItemInfo();

        isSelected = false;
        return false;
    }

    private void DisplayItemInfo()
    {
        if(type == ItemType.UNIT)
        {
            GameObject unitStatsDisplay = GameObject.FindGameObjectWithTag("UnitStats");
            unitStatsDisplay.GetComponent<SpriteRenderer>().enabled = true;

            unitStatsDisplay.transform.GetAllChildren().ForEach(child => child.gameObject.GetComponent<SpriteRenderer>().enabled = true);
            BaseGoblin unit = item.transform.Find("entity").GetComponent<BaseGoblin>();
            Utility.RenderSprite(unitStatsDisplay.transform, $"{unit.Health}",        "health");            
            Utility.RenderSprite(unitStatsDisplay.transform, $"{unit.Damage}",        "damage");    
            Utility.RenderSprite(unitStatsDisplay.transform, $"{unit.Range}",         "range" );    
            Utility.RenderSprite(unitStatsDisplay.transform, $"{unit.MovementRange}", "speed" );   

            Vector3 targetPos = new Vector3(1.615f, -1.375f, 0f);

            if (unit is IActiveAbility)
            {
                GameObject activeAbility = Instantiate(Resources.Load<GameObject>("Prefabs/UI/unit_ability"), targetPos, Quaternion.identity);
                activeAbility.transform.Find("ability_desc").GetComponent<TextMeshPro>().text = ((IActiveAbility)unit).ActiveAbilityDescription();
                targetPos.y -= 0.17f;
            }     
            if (unit is IPassiveAbility)
            {
                GameObject passiveAbility = Instantiate(Resources.Load<GameObject>("Prefabs/UI/unit_ability"), targetPos, Quaternion.identity);
                passiveAbility.transform.Find("ability_desc").GetComponent<TextMeshPro>().text = ((IPassiveAbility)unit).PassiveAbilityDescription();
            }   
        }
        if(type == ItemType.CARD)
        {
            Debug.Log("WIP");
        }
    }

    private void HideItemInfo()
    {
        if(type == ItemType.UNIT)
        {
            GameObject unit_stats_display = GameObject.FindGameObjectWithTag("UnitStats");
            unit_stats_display.GetComponent<SpriteRenderer>().enabled = false;
            unit_stats_display.transform.GetAllChildren().ForEach(child => child.gameObject.GetComponent<SpriteRenderer>().enabled = false);
            BaseGoblin unit = item.transform.Find("entity").GetComponent<BaseGoblin>();
            if (unit is IActiveAbility || unit is IPassiveAbility)
            {
                GameObject.FindGameObjectsWithTag("UnitAbility").ForEach(ability => Destroy(ability));
            }     
        }
        if(type == ItemType.CARD)
        {
            Debug.Log("WIP");
        }
    }

    public void RenderCard(GameObject item, ItemType type)
    {
        Component itemComponent = null;
        this.item = item;
        this.type = type;
        itemComponent = type == ItemType.UNIT ? item.transform.Find("entity").GetComponent<BaseGoblin>() : item.transform.Find("card_icon").GetComponent<SpriteRenderer>();
        string spriteName = itemComponent.gameObject.GetComponent<SpriteRenderer>().sprite.name;
        Utility.RenderSprite(transform, type == ItemType.UNIT ? spriteName : $"{spriteName}_minified", "item", "Atlas");
    }


    public enum ItemType
    {
        UNIT,
        CARD
    }
}