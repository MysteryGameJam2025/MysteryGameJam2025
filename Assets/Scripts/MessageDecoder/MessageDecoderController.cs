using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MessageDecoderController : MonoBehaviour
{
    [SerializeField]
    private Canvas currentCanvas;
    private Canvas CurrentCanvas => currentCanvas;

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

    private List<DragAndDropTextController> remainingOptions = new List<DragAndDropTextController>();

    private void Start()
    {
        ParseText();
    }

    public void SetUp(MessageData message)
    {
        currentMessage = message;
    }

    public void ParseText()
    {
        string rawData = CurrentMessage.Message.text;
        string parsedString = rawData;

        for (int i = 0; i < CurrentMessage.SymbolsInMessage.Length; i++)
        {
            Regex regex = new Regex($"<{CurrentMessage.SymbolsInMessage[i].SymbolName}>");
            MatchCollection symbols = regex.Matches(rawData);
            parsedString = regex.Replace(parsedString, $"<sprite name={CurrentMessage.SymbolsInMessage[i].SymbolSprite.name}>");
            CreateNewTextOption(CurrentMessage.SymbolsInMessage[i].SymbolSprite.name);
        }

        for (int i = 0; i < CurrentMessage.AlternatePossibilites.Length; i++)
        {
            CreateNewTextOption(CurrentMessage.AlternatePossibilites[i]);
        }

        TextField.text = parsedString;
        TextField.ForceMeshUpdate();


        foreach(TMP_CharacterInfo info in TextField.textInfo.characterInfo)
        {
            if(info.character == 57344)
            {
                DragAndDropTargetController newDragAndDrop = Instantiate(DragAndDropTarget, TextField.transform);
                newDragAndDrop.transform.position = TextField.transform.TransformPoint(info.bottomLeft);
            }
        }

        RandomiseRemainingTextOptions();
    }

    private void CreateNewTextOption(string text)
    {
        DragAndDropTextController newText = Instantiate(DragAndDropText, DragAndDropTextHolder);
        newText.SetText(text);
        remainingOptions.Add(newText);
    }

    private void RandomiseRemainingTextOptions()
    {
        remainingOptions.Shuffle();
        for(int i = 0; i < remainingOptions.Count; i++)
        {
            remainingOptions[i].transform.SetSiblingIndex(i);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(DragAndDropTextHolder);
    }
}
