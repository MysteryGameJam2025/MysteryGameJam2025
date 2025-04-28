using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MessageDecoder : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textField;
    private TMP_Text TextField => textField;

    [SerializeField]
    private RectTransform dragAndDropTextHolder;
    private RectTransform DragAndDropTextHolder => dragAndDropTextHolder;

    [SerializeField]
    private MessageData currentMessage;
    private MessageData CurrentMessage => currentMessage;

    [SerializeField]
    private DragAndDropTargetController dragAndDropTarget;
    private DragAndDropTargetController DragAndDropTarget => dragAndDropTarget;

    [SerializeField]
    private DragAndDropTextController dragAndDropText;
    private DragAndDropTextController DragAndDropText => dragAndDropText;

    private List<DragAndDropTargetController> dragAndDropTargets = new List<DragAndDropTargetController>();
    private List<DragAndDropTextController> remainingOptions = new List<DragAndDropTextController>();

    private int dragAndDropTargetsRemaining = 0;

    private Action OnCompleted;

    public void SetUp(MessageData message, Action onCompleted)
    {
        currentMessage = message;
        OnCompleted = onCompleted;
        ParseText();
    }

    private void ParseText()
    {
        string rawData = CurrentMessage.Message.text;
        string parsedString = rawData;

        for (int i = 0; i < CurrentMessage.SymbolsInMessage.Length; i++)
        {
            string symbolName = CurrentMessage.SymbolsInMessage[i].SymbolSprite.name;
            Regex regex = new Regex($"<{symbolName}>");
            MatchCollection symbols = regex.Matches(rawData);
            parsedString = regex.Replace(parsedString, $"<sprite name={symbolName}>");
            CreateNewTextOption(symbolName);
            for(int j = 0; j < symbols.Count; j++)
                CreateNewDragAndDropTarget(symbolName);
        }

        for (int i = 0; i < CurrentMessage.AlternatePossibilites.Length; i++)
        {
            CreateNewTextOption(CurrentMessage.AlternatePossibilites[i]);
        }

        TextField.text = parsedString;
        TextField.ForceMeshUpdate();

        int index = 0;
        foreach(TMP_CharacterInfo info in TextField.textInfo.characterInfo)
        {
            if(info.character == 57344)
            {
                dragAndDropTargets[index].transform.position = TextField.transform.TransformPoint(info.bottomLeft);
                index++;
            }
        }

        RandomiseRemainingTextOptions();
    }

    private void CreateNewDragAndDropTarget(string text)
    {
        DragAndDropTargetController newDragAndDrop = Instantiate(DragAndDropTarget, TextField.transform);
        newDragAndDrop.TextToMatch = text;
        dragAndDropTargetsRemaining++;
        dragAndDropTargets.Add(newDragAndDrop);
    }

    private void CreateNewTextOption(string text)
    {
        DragAndDropTextController newText = Instantiate(DragAndDropText, DragAndDropTextHolder);
        newText.Init(text);
        newText.OnPickedUp += PlayerPickedUpOption;
        newText.OnDropped += PlayerDroppedOption;
        remainingOptions.Add(newText);
    }

    private void PlayerPickedUpOption(DragAndDropTextController controller)
    {
        controller.transform.SetParent(transform);
        controller.transform.SetAsLastSibling();
        remainingOptions.Remove(controller);

        LayoutRemainingOptions();
    }

    private void PlayerDroppedOption(DragAndDropTextController controller)
    {
        for(int i = 0; i < dragAndDropTargets.Count; i++)
        {
            DragAndDropTargetController target = dragAndDropTargets[i];
            if ((controller.transform as RectTransform).GetWorldSapceRect().Overlaps((target.transform as RectTransform).GetWorldSapceRect()))
                if (controller.DoesMatchText(target.TextToMatch))
                {
                    remainingOptions.Remove(controller);
                    dragAndDropTargets.Remove(target);
                    Destroy(controller.gameObject);
                    Destroy(target.gameObject);
                    dragAndDropTargetsRemaining--;

                    if (dragAndDropTargetsRemaining <= 0)
                        OnCompleted?.Invoke();

                    return;
                }
        }

        controller.transform.SetParent(DragAndDropTextHolder);
        remainingOptions.Add(controller);

        LayoutRemainingOptions();
    }

    private void RandomiseRemainingTextOptions()
    {
        remainingOptions.Shuffle();
        LayoutRemainingOptions();
    }

    private void LayoutRemainingOptions()
    {
        for (int i = 0; i < remainingOptions.Count; i++)
        {
            remainingOptions[i].transform.SetSiblingIndex(i);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(DragAndDropTextHolder);
    }
}
