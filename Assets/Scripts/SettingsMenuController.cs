using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private Slider masterVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Button backButton;
    [SerializeField]
    private EventSystem eventSystem;
    private EventSystem EventSystem => eventSystem;

    public Action OnClose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bind();
    }

    public void OpenSettings(Action onClose = null)
    {
        OnClose = onClose;
        container.SetActive(true);
        EventSystem.SetSelectedGameObject(masterVolumeSlider.gameObject);
    }

    public void CloseSettings()
    {
        OnClose?.Invoke();

        container.SetActive(false);

        OnClose = null;
    }

    private void Bind()
    {
        masterVolumeSlider.value = AudioController.Instance.MasterVolume;
        musicVolumeSlider.value = AudioController.Instance.MusicVolume;
        sfxVolumeSlider.value = AudioController.Instance.SfxVolume;

        masterVolumeSlider.onValueChanged.AddListener(OnMasterChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxChanged);

        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        CloseSettings();
    }

    private void OnSfxChanged(float value)
    {
        AudioController.Instance.MusicVolume = value;
    }

    private void OnMusicChanged(float value)
    {
        AudioController.Instance.MusicVolume = value;
    }

    private void OnMasterChanged(float value)
    {
        AudioController.Instance.MasterVolume = value;
    }
}
