using UnityEngine;

public class Card : MonoBehaviour, IClickable
{
    public virtual int Cost() => 0;
    private bool isSelected = false;

    public virtual void OnClick(GameObject prevSelection = null) => isSelected = isSelected ? Deselect() : Select();

    public virtual bool Select()
    {
        GameManager.getInstance().AudioPlayer("SFXVol", "Card Pick Up");
        transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, -1.05f, transform.parent.transform.position.z);
        return true;
    }

    public virtual bool Deselect()
    {
        transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, -1.305f, transform.parent.transform.position.z);
        isSelected = false;
        return false;
    }

    public void UseEffect(GameObject target = null){
        Debug.Log("Effect being used");
        Deselect();
        
        GameManager.getInstance().AudioPlayer("SFXVol", "Card Drop");

        if(TurnManager.getInstance().bonusActionTaken) return;
        HandleEffect(target);
        TurnManager.getInstance().HandleTurnAction(TurnManager.ACTION.BONUS_ACTION);
        LocalInventory.getInstance().UpdateCardListItem(transform.parent.gameObject, LocalInventory.getInstance().GetPositionOfCard(transform.parent.gameObject));
        Destroy(transform.parent.gameObject);
    }

    protected virtual void HandleEffect(GameObject target = null){}

    protected void Start() {}
}