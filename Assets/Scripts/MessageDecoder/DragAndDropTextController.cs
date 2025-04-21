using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public Action<DragAndDropTextController> OnPickedUp;

    public Action<DragAndDropTextController> OnDropped;

    private bool isHovered;
    private bool isPickedUp;

    public void Init(string text)
    {
        TextElement.text = text;
    }

    public void SetHovered(bool hovered)
    {
        isHovered = hovered;
    }

    public bool DoesMatchText(string textToMatch)
    {
        return TextElement.text == textToMatch;
    }

    private void Update()
    {
        if (isHovered && SelectAction.action.WasPressedThisFrame())
        {
            OnPickedUp?.Invoke(this);
            isPickedUp = true;
        }
        else if (isPickedUp && SelectAction.action.IsPressed())
        {
            transform.position = PointerLocationAction.action.ReadValue<Vector2>();
        }
        else if (isPickedUp && SelectAction.action.WasReleasedThisFrame())
        {
            OnDropped?.Invoke(this);
            isPickedUp = false;
        }
    }
}
