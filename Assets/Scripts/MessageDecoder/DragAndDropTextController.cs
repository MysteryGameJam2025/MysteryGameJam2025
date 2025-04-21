using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class DragAndDropTextController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textElement;
    private TMP_Text TextElement => textElement;

    [SerializeField]
    private InputActionReference selectAction;
    private InputActionReference SelectAction => selectAction;

    [SerializeField]
    private InputActionReference pointerLocationAction;
    private InputActionReference PointerLocationAction => pointerLocationAction;

    public Action OnPickedUp;

    public Action OnDropped;

    private bool isHovered;

    public void SetText(string text)
    {
        TextElement.text = text;
    }

    private void OnMouseEnter()
    {
        isHovered = true;
    }

    private void OnMouseExit()
    {
        isHovered = false;
    }

    private void Update()
    {
        if(isHovered && SelectAction.action.IsInProgress())
        {
            OnPickedUp?.Invoke();
            transform.position = PointerLocationAction.action.ReadValue<Vector2>();
        }
    }
}
