using System;
using System.Xml.Serialization;
using UnityEngine;
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
        SceneController.Instance.LoadScene(sceneToLoadOnPlay);
    }

    private void SetMenuUIVisibility(bool isVisible)
    {
        container.gameObject.SetActive(isVisible);
    }

    private void Quit()
    {
        Application.Quit();
    }

}
