using System;
using UnityEngine;
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
    }

    public void CloseSettings()
    {
        OnClose?.Invoke();

        container.SetActive(false);

        OnClose = null;
    }

    private void Bind()
    {
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
        throw new NotImplementedException();
    }

    private void OnMusicChanged(float value)
    {
        throw new NotImplementedException();
    }

    private void OnMasterChanged(float value)
    {
        throw new NotImplementedException();
    }
}
