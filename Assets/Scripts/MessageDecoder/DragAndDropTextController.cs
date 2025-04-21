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

    public Action OnPickedUp;

    public Action OnDropped;

    private bool isHovered;

    public void SetText(string text)
    {
        TextElement.text = text;
    }

    public void SetHovered(bool hovered)
    {
        isHovered = hovered;
    }

    private void Update()
    {
        //Debug.Log($"Pointer location: {PointerLocationAction.action.ReadValue<Vector2>()}");
        Debug.Log($"Object location: {gameObject.transform.position}");

        if (isHovered && SelectAction.action.IsInProgress())
        {
            OnPickedUp?.Invoke();
            transform.position = PointerLocationAction.action.ReadValue<Vector2>();
        }
    }
}
