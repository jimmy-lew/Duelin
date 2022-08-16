using System;
using System.Linq;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    SelectionActions _input;
    SelectionActions.InputActions _inputAction;
    Camera _camera;

    GameObject currentSelection;
    GameObject prevSelection;

    GameObject currentHover;
    GameObject prevHover;

    private void Awake()
    {
        _input = new SelectionActions();
        _inputAction = _input.Input;
        _camera = Camera.main;
    }

    private void OnEnable() 
    {
        _input.Enable();
    }

    private void OnDisable() 
    {
        _input.Disable();
    }

    void Start()
    {
        _inputAction.Select.performed += _ => Select();
    }

    void Update() {
        // Hover();
    }

    void Hover() {
        prevHover = currentHover;
        Vector2 mousePos = _inputAction.Pos.ReadValue<Vector2>();
        mousePos = _camera.ScreenToWorldPoint(mousePos);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        bool hitFound = hit.collider != null;
        bool isHoverable(GameObject selection) => selection.GetComponents<Component>().Any(component => component is IHoverable);

        if(!hitFound) return;
        currentHover = hit.collider.gameObject;

        if (!isHoverable(currentHover)) return;
        ((IHoverable)currentHover.GetComponents<Component>().Single(component => component is IHoverable)).OnHover(prevHover);
    }

    void Select()
    {
        prevSelection = currentSelection;
        Vector2 mousePos = _inputAction.Pos.ReadValue<Vector2>();
        mousePos = _camera.ScreenToWorldPoint(mousePos);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        bool hitFound = hit.collider != null;
        bool isClickable(GameObject selection) => selection.GetComponents<Component>().Any(component => component is IClickable);

        if(!hitFound) return;
        currentSelection = hit.collider.gameObject;

        if(!isClickable(currentSelection)) return;
        if (!TurnManager.getInstance().CheckTurn()) return;
        ((IClickable)currentSelection.GetComponents<Component>().Single(component => component is IClickable)).OnClick(prevSelection);
    }
}