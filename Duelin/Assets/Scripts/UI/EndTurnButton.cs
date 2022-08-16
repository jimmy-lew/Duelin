using System.Linq;
using UnityEngine;

public class EndTurnButton : UIElement, IClickable
{
    public static EndTurnButton getInstance() => GameObject.FindWithTag("EndTurnButton").GetComponent<EndTurnButton>();
    public void OnClick(GameObject prevSelection = null)
    {
        if(LocalInventory.getInstance().GetGoblins().Count <= 0) return;
        LocalInventory.getInstance().GetGoblins().ForEach(entity => entity.actionManager.Deselect());
        TurnManager.getInstance().HandleTurnControl(TurnManager.TURN_CONTROL.END);
    }

    public void RenderButton(bool _active)
    {
        Sprite sprite = Resources.LoadAll<Sprite>("UI_Atlas").Single(sprite => sprite.name.Equals($"end_turn_{_active}"));
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}