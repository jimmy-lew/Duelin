using System;
using UnityEngine;

public class UnitCard : MonoBehaviour, IClickable
{
    public void OnClick(GameObject prevSelection = null)
    {
        bool isSelectionCard = prevSelection != null && prevSelection.GetComponent<Card>() != null;
        if (isSelectionCard) prevSelection.GetComponent<Card>().Deselect();
        bool isEntityCard = prevSelection != null && prevSelection.GetComponent<UnitCard>();
        int unitIndex = (int) Math.Ceiling((0.89f - gameObject.transform.position.y) / 0.28f);
        BaseGoblin goblin = LocalInventory.getInstance().GetGoblin(unitIndex);
        if(!isEntityCard) goblin.OnClick(prevSelection);
        int prevEntityIndex = (int) Math.Ceiling((0.89f - prevSelection.transform.position.y) / 0.28f);
        BaseGoblin prevGoblin = LocalInventory.getInstance().GetGoblin(prevEntityIndex);
        goblin.OnClick(prevGoblin.gameObject);
    }

    public void RenderCard(BaseGoblin entity, bool isSelected = false)
    {
        Utility.RenderSprite(gameObject.transform,$"{(entity.photonView.IsMine ? "friendly" : "enemy")}_healthbar_{entity.Health}" ,"healthbar");
        Utility.RenderSprite(gameObject.transform, entity.Cooldown > 0 ? $"cool_down_bar_{entity.Cooldown}" : "empty_bar", "cooldown");
        Utility.RenderSprite(gameObject.transform, $"{entity.Damage}", "damage");
        Utility.RenderSprite(gameObject.transform, $"unit_card{(isSelected ? "_selected" : "")}", "card");

        Sprite unitSprite = entity.gameObject.GetComponent<SpriteRenderer>().sprite;
        transform.Find("unit").GetComponent<SpriteRenderer>().sprite = unitSprite;
    }
}