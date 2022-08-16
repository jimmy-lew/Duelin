using System.Linq;
using UnityEngine;

public class GoldBar : MonoBehaviour
{
    public void RenderBar()
    {
        Sprite goldBarSprite = Resources.LoadAll<Sprite>("UI_Atlas").Single(sprite => sprite.name.Equals($"gold_bar_{GameManager.getInstance().amountToWin}"));
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = goldBarSprite;
    }
}