using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
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
    private MessageData currentMessage;
    private MessageData CurrentMessage => currentMessage;

    [SerializeField]
    private DragAndDropController dragAndDropBox;
    private DragAndDropController DragAndDropBox => dragAndDropBox;

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
        Regex symbolRegex = new Regex("<[A-Za-z]*>");

        MatchCollection symbols = symbolRegex.Matches(rawData);
        for (int i = 0; i < symbols.Count; i++)
        {
            parsedString = symbolRegex.Replace(parsedString, $"<sprite name={CurrentMessage.SymbolsInMessage[i].SymbolSprite.name}>", 1, i);
        }

        TextField.text = parsedString;
        TextField.ForceMeshUpdate();


        foreach(TMP_CharacterInfo info in TextField.textInfo.characterInfo)
        {
            if(info.character == 57344)
            {
                DragAndDropController newDragAndDrop = Instantiate(DragAndDropBox, TextField.transform);
                newDragAndDrop.transform.position = TextField.transform.TransformPoint(info.bottomLeft);
            }
        }
    }
}
