using FriedSynapse.FlowEnt;
using UnityEngine;

public class MessageDecoderController : AbstractMonoBehaviourSingleton<MessageDecoderController>
{
    [SerializeField]
    private MessageDecoder messageDecoder;
    private MessageDecoder MessageDecoder => messageDecoder;

    [SerializeField]
    private CanvasGroup messageCanvasGroup;
    private CanvasGroup MessageCanvasGroup => messageCanvasGroup;

    private Tween ShowMessageTween { get; set; }

    public void OpenMessage(MessageData messageData)
    {
        ShowMessageTween?.Stop();
        MessageDecoder.SetUp(messageData, CloseMessage);
        PlayerController.Instance.LockControls();

        //Play animation or something
        ShowMessageTween = new Tween(0.8f)
            .For(MessageCanvasGroup)
                .AlphaTo(1)
            .OnCompleted(() => 
            {
                MessageCanvasGroup.blocksRaycasts = true;
                MessageCanvasGroup.interactable = true;
            })
            .Start();
    }

    public void CloseMessage()
    {
        ShowMessageTween?.Stop();
        ShowMessageTween = new Tween(0.8f)
            .OnStarted(() =>
            {
                MessageCanvasGroup.blocksRaycasts = false;
                MessageCanvasGroup.interactable = false;
            })
            .For(MessageCanvasGroup)
                .AlphaTo(0)
            .OnCompleted(PlayerController.Instance.UnlockControls)
            .Start();
    }
}
