using FriedSynapse.FlowEnt;
using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private string sceneToLoadOnPlay;

    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private SettingsMenuController settingsMenu;

    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private EventSystem eventSystem;
    private EventSystem EventSystem => eventSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bind();
    }

    private void Bind()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnSettingsButtonClicked()
    {
        SetMenuUIVisibility(false);
        settingsMenu.OpenSettings(onClose: () => SetMenuUIVisibility(true));
    }

    private void OnPlayButtonClicked()
    {
        AudioController.Instance?.FadeBackgroundMusic(false, 0.5f);
        new Tween(0.75f).OnCompleted(() =>
        {
            SceneController.Instance.LoadScene(sceneToLoadOnPlay);
            AudioController.Instance?.FadeBackgroundMusic(true, 0.5f);
        }).Start();
    }

    private void SetMenuUIVisibility(bool isVisible)
    {
        container.gameObject.SetActive(isVisible);
        if (isVisible)
        {
            EventSystem.SetSelectedGameObject(playButton.gameObject);
        }

    }

    private void Quit()
    {
        Application.Quit();
    }

}
