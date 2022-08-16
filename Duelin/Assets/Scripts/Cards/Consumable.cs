using UnityEngine;

public class Consumable : Card
{
    public override void OnClick(GameObject prevSelection = null)
    {
        base.OnClick(prevSelection);
        bool isSelectionSelf = prevSelection != null && prevSelection.GetComponent<Card>() == this;
        if (isSelectionSelf) UseEffect();
    }
}