using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LocalInventory
{
    private static LocalInventory _instance = null;
    public static LocalInventory getInstance() => _instance == null ? _instance = new LocalInventory() : _instance;
    private int Gold;
    private List<GameObject> Entities = new List<GameObject>();
    private List<GameObject> Cards = new List<GameObject>();
    private Tilemap mineTilemap;
    
    private List<ArrayList> GAME_STATEs = new List<ArrayList>();

    LocalInventory()
    {
        Gold = 5;
        mineTilemap = TilemapRepository.getInstance().GetTilemap(TilemapRepository.MINE_MAP);
    }

    public void UpdateEntityListItem(GameObject entity, int index) 
    {
        if (Entities.Count < index + 1) Entities.Add(entity);
        Entities[index] = entity;
    }

    public void UpdateCardListItem(GameObject card, int index)
    {
        if (Cards.Count < index + 1) Cards.Add(card);
        Cards[index] = card;
    }

    public LocalInventory UpdateGameState()
    {
        GAME_STATEs.ForEach(state => {
            int index = GAME_STATEs.FindIndex(s => (GAME_STATE) s[0] == (GAME_STATE) state[0]);
            GAME_STATEs[index][1] = ((int) GAME_STATEs[index][1]) - 1; 
        });
        GAME_STATEs.RemoveAll(state => ((int) state[1]) <= 0);
        return this;
    }

    public LocalInventory UpdateGoldAmount(){
        GetGoblins().ForEach(entity => {
            Vector3Int currentCellPos = mineTilemap.WorldToCell(entity.GetCurrentPos());
            bool isOnMine = mineTilemap.HasTile(new Vector3Int(currentCellPos.x, currentCellPos.y, 2));
            bool hasCaptured = entity.occupationState == BaseGoblin.OCCUPATION_STATE.CAPTURED;

            if(isOnMine && !hasCaptured) entity.occupationState = (BaseGoblin.OCCUPATION_STATE) ((int) entity.occupationState + 1);
            if(!isOnMine) entity.occupationState = BaseGoblin.OCCUPATION_STATE.FREE;
            if(!IsHarvest() && IsPlagued()) return;
            if(isOnMine && hasCaptured) AddGold(2 * (entity is Miner ? 2 : 1) * (!IsPlagued() && IsHarvest() ? 2 : 1));
        });
        if(SpawnManager.getInstance().IsSpawnOccupied()) AddGold(1);
        GameObject.FindWithTag("GoldAmount").GetComponent<GoldAmount>().RenderAmount();
        return this;
    }

    public void DestroyGoblin(GameObject entity)
    {
        int entityIndex = GetPositionOfEntity(entity);
        for (int i = entityIndex + 1; i < Entities.Count; i++)
        {
            GameObject targetEntity = Entities[i];
            GameObject targetEntityUnitCard = targetEntity.transform.Find("entity").GetComponent<BaseGoblin>().unit_card;
            Vector3 targetEntityUnitCardPos = targetEntityUnitCard.transform.position;
            targetEntityUnitCard.transform.position = new Vector3(targetEntityUnitCardPos.x, targetEntityUnitCardPos.y += 0.28f, targetEntityUnitCardPos.z); 
        }
        Entities.RemoveAt(entityIndex);
        if (GetEntityListSize() <= 0) GameObject.FindWithTag("WinLoseToast").GetComponent<WinLoseToast>().Render(false);
        GameManager.getInstance().StartCoroutine(isLastPlayer());
    }

    IEnumerator isLastPlayer()
    {
        yield return new WaitForSeconds(2.5f);
        if (PhotonNetwork.CountOfPlayersInRooms == 1) GameObject.FindWithTag("WinLoseToast").GetComponent<WinLoseToast>().Render();
    }
    
    public int GetPositionOfEntity(GameObject entity) => Entities.FindIndex(e => e == entity);
    public int GetPositionOfCard(GameObject card) => Cards.FindIndex(c => c == card);
    public int GetEntityListSize() => Entities.Count; 
    public int GetCardListSize() => Cards.Count; 
    public List<BaseGoblin> GetGoblins() => Entities.Select(entity => entity.transform.Find("entity").GetComponent<BaseGoblin>()).ToList();
    public BaseGoblin GetGoblin(int index) => Entities[index].transform.Find("entity").GetComponent<BaseGoblin>();
    public Card GetCard(int index) => Cards[index].transform.Find("card_modal").GetComponent<Card>();
    private bool IsPlagued() => GAME_STATEs.Any(state => (GAME_STATE) state[0] == GAME_STATE.PLAGUED);
    private bool IsHarvest() => GAME_STATEs.Any(state => (GAME_STATE) state[0] == GAME_STATE.HARVEST);

    // --------------- Other Inventory Functions ---------------

    // ==== Cards ====
    ///<summary>
    ///Adds a card to the player's inventory (e.g: when purchased from the shop)
    ///</summary>
    public void AddCard(GameObject card){
        Cards.Add(card);
        GameObject newCardInstance = null;
        int cardListSize = GetCardListSize();
        float startingXPosition = (cardListSize - 1) * -0.28f;

        Cards.ForEach(card => {
            Vector3 cardPos = new Vector3(startingXPosition + GetPositionOfCard(card) * 0.56f, -1.305f, 0f);
            if(GetPositionOfCard(card) == cardListSize - 1) {
                newCardInstance = GameObject.Instantiate(card, cardPos, Quaternion.identity);
                return;
            }
            card.transform.position = cardPos;
        });
        UpdateCardListItem(newCardInstance, GetPositionOfCard(card));
    }

    public void AddGameState(GAME_STATE state, int duration)
    {
        GAME_STATEs.Add(new ArrayList(){
            state,
            duration
        });
    }

    ///<summary>
    ///Remove a card to the player's inventory (e.g: when Used)
    ///</summary>
    public void RemoveCard(GameObject card){
        Cards.Remove(card);
    }

    ///<summary>
    ///Gets the player's cards
    ///</summary>
    public List<GameObject> GetCards(){
        return Cards;
    }

    // ==== Currency ====
    public int GetGold(){
        return Gold;
    }

    ///<summary>
    ///Adds currency to the player's inventory (e.g: When farm captured / when enemy killed)
    ///</summary>
    public void AddGold(int value){
        Gold += value;
        if (!(Gold >= GameManager.getInstance().amountToWin)) GameObject.FindWithTag("GoldAmount").GetComponent<GoldAmount>().RenderAmount();
        else GameManager.getInstance().HandleWin();
    }

    ///<summary>
    ///Remove currency to the player's inventory (e.g: When Card/Entities purchased from shop)
    ///</summary>
    public void RemoveGold(int amount){
        Gold -= amount;
        GameObject.FindWithTag("GoldAmount").GetComponent<GoldAmount>().RenderAmount();
    }

    public enum GAME_STATE
    {
        PLAGUED,
        HARVEST,
    }

    public void ReturnToMain(){
        Entities = new List<GameObject>();
        Cards = new List<GameObject>();
        Gold = 5;

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
            
        // Trigger OST Again
        PreserveSound.Instance.gameObject.GetComponent<AudioSource>().time = 0;
        PreserveSound.Instance.gameObject.GetComponent<AudioSource>().Play();
    }
}