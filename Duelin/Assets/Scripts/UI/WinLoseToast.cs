using System.Collections;
using System.Linq;
using UnityEngine;

public class WinLoseToast : MonoBehaviour
{
    int lifetime = 2;

    public void Render(bool isWin = true)
    {
        if (!isWin) GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("UI_Atlas").Single(sprite => sprite.name.Equals("lose_toast"));
        GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(WaitThenDie());
    }

    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(lifetime);
        LocalInventory.getInstance().ReturnToMain();
    }
}
