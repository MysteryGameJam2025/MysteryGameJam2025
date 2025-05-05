using UnityEngine;

public class CreditsController : MonoBehaviour
{
    void Start()
    {
        AudioController.Instance.FadeOutBackingTrack();
        AudioController.Instance.StartBackgroundMusic();
    }

    public void BackToMenu()
    {
        SceneController.Instance.LoadScene("Menu");
    }
}
