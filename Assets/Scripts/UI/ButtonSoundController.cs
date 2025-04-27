using UnityEngine;

public class ButtonSoundController : MonoBehaviour
{
    public void OnHover()
    {
        AudioController.Instance?.PlayGlobalSound("UiClick");
    }

    public void OnClick()
    {
        AudioController.Instance?.PlayGlobalSound("UiAccept");
    }
}
