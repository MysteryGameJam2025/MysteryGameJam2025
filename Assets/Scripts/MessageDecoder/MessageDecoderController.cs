using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class MessageDecoderController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textField;
    private TMP_Text TextField => textField;

    [SerializeField]
    private MessageData currentMessage;
    private MessageData CurrentMessage => currentMessage;

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
    }
}
