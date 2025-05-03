using FriedSynapse.FlowEnt;
using System;
using UnityEngine;

public class MessageDecoderController : AbstractMonoBehaviourSingleton<MessageDecoderController>
{
    [SerializeField]
    private MessageDecoder messageDecoder;
    private MessageDecoder MessageDecoder => messageDecoder;

    private Tween ShowMessageTween { get; set; }
    private Action OnMessageClosed;

    public void OpenMessage(MessageData messageData, Action onMessageClosed = null)
    {
        DialogueSingleton.Instance.OnSectionCompleted = () => ShowMessage(messageData, onMessageClosed);
        DialogueSingleton.Instance.EnqueueDialogue(messageData.PreSolveDialog);
    }

    private void ShowMessage(MessageData messageData, Action onMessageClosed)
    {
        ShowMessageTween?.Stop();
        MessageDecoder.SetUp(messageData, CloseMessage);
        PlayerController.Instance.LockControls();
        OnMessageClosed = onMessageClosed;

        ShowMessageTween = new Tween(0.8f)
            .For(MessageDecoder.CanvasGroup)
                .AlphaTo(1)
            .OnCompleted(() =>
            {
                MessageDecoder.CanvasGroup.blocksRaycasts = true;
                MessageDecoder.CanvasGroup.interactable = true;
            })
            .Start();
    }

    public void CloseMessage()
    {
        ShowMessageTween?.Stop();
        ShowMessageTween = new Tween(0.8f)
            .OnStarted(() =>
            {
                MessageDecoder.CanvasGroup.blocksRaycasts = false;
                MessageDecoder.CanvasGroup.interactable = false;
            })
            .For(MessageDecoder.CanvasGroup)
                .AlphaTo(0)
            .OnCompleted(() => 
            { 
                PlayerController.Instance.UnlockControls();
                OnMessageClosed?.Invoke();
            })
            .Start();
    }
}
