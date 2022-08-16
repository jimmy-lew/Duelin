using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager getInstance() => GameObject.FindWithTag("GameManager").GetComponent<ShopManager>();
    private List<GameObject> AllUnits;
    private List<GameObject> AllCards;
    private List<GameObject> UnitsForSale = new List<GameObject>();
    private List<GameObject> CardsForSale = new List<GameObject>();
    private System.Random random = new System.Random();

    private void Start() {
        AllUnits = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Units"));
        AllCards = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Cards"));

        for(int i = 0; i < 5; i++, generateUnit());
        for(int i = 0; i < 3; i++, generateCard());
    }

    private void generateUnit() => UnitsForSale.Add(AllUnits[random.Next(AllUnits.Count)]);
    private void generateCard() => CardsForSale.Add(AllCards[random.Next(AllCards.Count)]);

    // TODO: Sync across clients
    public GameObject Purchase(GameObject selectedItem)
    {
        int MAX_UNITS = 5;
        int MAX_CARDS = 4;

        int itemCost = selectedItem.transform.Find("entity")?.GetComponent<BaseGoblin>().Cost() ?? selectedItem.transform.Find("card_modal").GetComponent<Card>().Cost();

        // TODO: UI Hint
        if (itemCost > LocalInventory.getInstance().GetGold()) return null;
        if (!SpawnManager.getInstance().HasSpawnPoints()) return null;

        if(UnitsForSale.Contains(selectedItem))
        {
            if (LocalInventory.getInstance().GetEntityListSize() >= MAX_UNITS) return null;
            UnitsForSale.Remove(selectedItem);
            GameManager.getInstance().AudioPlayer("SFXVol", "Purchase");
            generateUnit();
        }
        if(CardsForSale.Contains(selectedItem))
        {
            if (LocalInventory.getInstance().GetCardListSize() >= MAX_CARDS) return null;
            CardsForSale.Remove(selectedItem);
            GameManager.getInstance().AudioPlayer("SFXVol", "Purchase");
            generateCard();
        }

        LocalInventory.getInstance().RemoveGold(itemCost);
        TurnManager.getInstance().HandleTurnAction(TurnManager.ACTION.PURCHASE); 

        return selectedItem;
    }

    public void RenderItemsForSale(bool _enabled = true)
    {
        GameObject.FindGameObjectsWithTag("ShopItem").ForEach(item => {
            item.GetComponent<ItemCard>().Deselect();
            Destroy(item);
        });

        if (!_enabled) return;
        for (int i = 0; i < 8; i++)
        {
            GameObject itemForSale = i >= 5 ? CardsForSale[i-5] : UnitsForSale[i];
            Vector3 displayPos = new Vector3(1.72f + (i % 2 * 0.31f), -0.06f + ((float) Math.Floor((double)(i / 2)) * -0.28f), 0f);
            GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/UI/item_card");
            GameObject itemPrefabInstance = Instantiate(itemPrefab, displayPos, Quaternion.identity);

            itemPrefabInstance.GetComponent<ItemCard>().RenderCard(itemForSale, i >= 5 ? ItemCard.ItemType.CARD : ItemCard.ItemType.UNIT);
        }
    }

    public bool CanAffordAny()
    {
        bool canAffordAnyUnit = UnitsForSale.Any(unit => unit.transform.Find("entity").GetComponent<BaseGoblin>().Cost() <= LocalInventory.getInstance().GetGold());
        bool canAffordAnyCard = CardsForSale.Any(card => card.transform.Find("card_modal").GetComponent<Card>().Cost() <= LocalInventory.getInstance().GetGold());
        return canAffordAnyUnit || canAffordAnyCard;
    }
}