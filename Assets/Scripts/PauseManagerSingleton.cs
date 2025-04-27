using UnityEngine;

public class PauseManagerSingleton : MonoBehaviour
{

    public void Pause()
    {
        PlayerController.Instance.LockControls();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PlayerController.Instance.UnlockControls();
        Time.timeScale = 1;
    }
}
