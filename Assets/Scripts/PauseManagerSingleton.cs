using FriedSynapse.FlowEnt;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManagerSingleton : MonoBehaviour
{
    private const string MainSceneName = "Menu";

    [SerializeField]
    private InputActionReference pauseAction;
    private InputActionReference PauseAction => pauseAction;
    [SerializeField]
    private CanvasGroup pauseMenu;
    private CanvasGroup PauseMenu => pauseMenu;
    [SerializeField]
    private Button resumeButton;
    private Button ResumeButton => resumeButton;
    [SerializeField]
    private Button quitButton;
    private Button QuitButton => quitButton;

    private bool isPaused { get; set; }
    private AbstractAnimation ShowHideAnimation { get; set; }

    public void Start()
    {
        ResumeButton.onClick.AddListener(Resume);
        QuitButton.onClick.AddListener(Quit);
    }

    public void Pause()
    {
        isPaused = true;
        PlayerController.Instance.LockControls();
        Time.timeScale = 0;
        PauseMenu.alpha = 1;
        PauseMenu.interactable = true;
        PauseMenu.blocksRaycasts = true;
    }

    public void Resume()
    {
        isPaused = false;
        PlayerController.Instance.UnlockControls();
        Time.timeScale = 1;
        PauseMenu.alpha = 0;
        PauseMenu.interactable = false;
        PauseMenu.blocksRaycasts = false;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneController.Instance.LoadScene(MainSceneName);
    }

    void Update()
    {
        if (PauseAction.action.WasPressedThisFrame())
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}
