using UnityEngine;

public class ShopButton : UIElement, IClickable
{
    private bool _enabled;

    private void Start() {
        _enabled = GameObject.FindWithTag("ShopPanel").GetComponent<SpriteRenderer>().enabled;
    }

    public void OnClick(GameObject prevSelection = null)
    {
        bool isSelectionCard = prevSelection != null && prevSelection.GetComponent<Card>() != null;
        if (isSelectionCard) prevSelection.GetComponent<Card>().Deselect();
        _enabled = !_enabled;
        GameObject.FindWithTag("ShopPanel").GetComponent<SpriteRenderer>().enabled = _enabled;
        ShopManager.getInstance().RenderItemsForSale(_enabled);
    }
}
