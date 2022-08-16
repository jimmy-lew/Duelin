using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityController : MonoBehaviour
{
    protected readonly string MOUSE_POS = "POS";
    public ACTION ACTION_TYPE;
    public static int clicks;

    public virtual void HandleAction(InputAction.CallbackContext context) {}
    public virtual void Clear() {}

    public enum ACTION
    {
        [Description("LEFT_CLICK")]
        LEFT_CLICK,
        [Description("RIGHT_CLICK")]
        RIGHT_CLICK,
    }
}