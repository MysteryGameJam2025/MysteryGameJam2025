using FriedSynapse.FlowEnt;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance => _instance;

    private float masterVolume = -1f;
    public float MasterVolume
    {
        get
        {
            if(masterVolume == -1f)
            {
                masterVolume = PlayerPrefs.GetFloat("masterVol", 0.5f);
                SetOnMixer("masterVol", masterVolume);
            }

            return masterVolume;
        }

        set
        {
            masterVolume = value;
            SetOnMixer("masterVol", masterVolume);
            PlayerPrefs.SetFloat("masterVol", masterVolume);
        }
    }

    private float musicVolume = -1f;
    public float MusicVolume
    {
        get
        {
            if (musicVolume == -1f)
            {
                musicVolume = PlayerPrefs.GetFloat("musicVol", 0.5f);
                SetOnMixer("musicVol", musicVolume);
            }

            return musicVolume;
        }

        set
        {
            musicVolume = value;
            SetOnMixer("musicVol", musicVolume);
            PlayerPrefs.SetFloat("musicVol", musicVolume);
        }
    }

    private float sfxVolume = -1f;
    public float SfxVolume
    {
        get
        {
            if (sfxVolume == -1f)
            {
                sfxVolume = PlayerPrefs.GetFloat("sfxVol", 0.5f);
                SetOnMixer("sfxVol", sfxVolume);
            }

            return sfxVolume;
        }

        set
        {
            sfxVolume = value;
            SetOnMixer("sfxVol", sfxVolume);
            PlayerPrefs.SetFloat("sfxVol", sfxVolume);
        }
    }

    [SerializeField]
    private AudioMixer mixer;
    public AudioMixer Mixer => mixer;

    [SerializeField]
    private AudioSource backgroundMusicSource;
    private AudioSource BackgroundMusicSource => backgroundMusicSource;

    private Tween SoundFadeTween;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        FadeInSource(BackgroundMusicSource, 0.5f);
    }

    private void SetOnMixer(string name, float value)
    {
        float dB;
        if (value != 0)
            dB = 20.0f * Mathf.Log10(value);
        else
            dB = -144.0f;

        Mixer.SetFloat(name, dB);
    }

    public void FadeInSource(AudioSource source, float timeOfFade = 1f)
    {
        SoundFadeTween?.Stop();

        if (!source.isPlaying)
            source.Play();

        SoundFadeTween = new Tween(timeOfFade)
            .OnStarted(() =>
            {
                if (!source.isPlaying)
                    source.Play();
            })
            .For(source)
                .VolumeTo(1)
            .Start();
    }

    public void FadeOutSource(AudioSource source, float timeOfFade = 1f)
    {
        SoundFadeTween?.Stop();
        if (!source.isPlaying)
            source.Play();

        SoundFadeTween = new Tween(timeOfFade)
            .For(source)
                .VolumeTo(0)
            .OnCompleted(() => source.Stop())
            .Start();
    }
}
