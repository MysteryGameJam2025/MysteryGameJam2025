using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TMP_CharacterQueue
{
    private class TMP_CharacterQueueItem
    {
        public string Text { get; set; }
        public bool IsTag { get; set; }
    }

    private Queue<TMP_CharacterQueueItem> DialogueCharacters { get; set; }

    public TMP_CharacterQueue(string text)
    {
        DialogueCharacters = new Queue<TMP_CharacterQueueItem>();
        Regex regex = new Regex(@"<.*?>|.[^<]+");
        var matches = regex.Matches(text);

        foreach (Match match in matches)
        {
            string substring = match.Value;
            if (substring.Length == 0)
            {
                continue;
            }
            bool isTag = substring[0] == '<';
            if (isTag)
            {
                TMP_CharacterQueueItem dialogueCharacter = new TMP_CharacterQueueItem()
                {
                    Text = substring,
                    IsTag = true
                };
                DialogueCharacters.Enqueue(dialogueCharacter);
            }
            else
            {
                foreach (char character in substring)
                {
                    TMP_CharacterQueueItem characterQueueItem = new TMP_CharacterQueueItem()
                    {
                        Text = character.ToString(),
                        IsTag = false
                    };
                    DialogueCharacters.Enqueue(characterQueueItem);
                }
            }

        }
    }

    public bool TryGetNextCharacter(out string characterString)
    {
        if (DialogueCharacters.Count == 0)
        {
            characterString = string.Empty;
            return false;
        }

        characterString = DequeueCharactersUntilNonTag();
        return true;
    }

    string DequeueCharactersUntilNonTag()
    {
        string characterString = string.Empty;
        NextCharacter(ref characterString);
        return characterString;
    }

    void NextCharacter(ref string characterString)
    {
        if (DialogueCharacters.Count == 0)
        {
            return;
        }
        TMP_CharacterQueueItem dialogueCharacter = DialogueCharacters.Dequeue();
        characterString += dialogueCharacter.Text;
        if (dialogueCharacter.IsTag)
        {
            NextCharacter(ref characterString);
        }

    }
}