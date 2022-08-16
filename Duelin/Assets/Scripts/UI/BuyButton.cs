using UnityEngine;

public class BuyButton : UIElement, IClickable
{
    public void OnClick(GameObject prevSelection = null)
    {
        bool isItemCard = prevSelection != null && prevSelection.GetComponent<ItemCard>() != null;
        bool isCardItemCard = isItemCard && prevSelection.GetComponent<ItemCard>().type == ItemCard.ItemType.CARD;
        if (!isItemCard || !isCardItemCard) return;

        ItemCard itemCard = prevSelection.GetComponent<ItemCard>();

        itemCard.Deselect();
        GameObject item = ShopManager.getInstance().Purchase(itemCard.item);
        if (item == null) return;

        LocalInventory.getInstance().AddCard(item);
        Destroy(itemCard);
        ShopManager.getInstance().RenderItemsForSale();
    }
}