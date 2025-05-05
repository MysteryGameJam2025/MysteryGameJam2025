using FriedSynapse.FlowEnt;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioTypes
{
    Sfx,
    Music
}

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance => _instance;

    private float masterVolume = -1f;
    public float MasterVolume
    {
        get
        {
            if (masterVolume == -1f)
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
                musicVolume = PlayerPrefs.GetFloat("musicVol", 0.25f);
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
                sfxVolume = PlayerPrefs.GetFloat("sfxVol", 0.25f);
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
    private AudioMixerGroup musicGroup;
    private AudioMixerGroup MusicGroup => musicGroup;

    [SerializeField]
    private AudioMixerGroup sfxGroup;
    private AudioMixerGroup SfxGroup => sfxGroup;

    [SerializeField]
    private AudioSource backgroundMusicSource;
    private AudioSource BackgroundMusicSource => backgroundMusicSource;

    [SerializeField]
    private AudioSource backingTrackSource;
    private AudioSource BackingTrackSource => backingTrackSource;

    [SerializeField]
    private AudioSource loveThemeSource;
    private AudioSource LoveThemeSource => loveThemeSource;

    [SerializeField]
    private AudioSource worldThemeSource;
    private AudioSource WorldThemeSource => worldThemeSource;

    [SerializeField]
    private AudioSource worldThemeIncorrectHarpSource;
    private AudioSource WorldThemeIncorrectHarpSource => worldThemeIncorrectHarpSource;

    [SerializeField]
    private AudioSource worldThemeCorrectHarpSource;
    private AudioSource WorldThemeCorrectHarpSource => worldThemeCorrectHarpSource;

    [SerializeField]
    private AudioSource worldThemeHornSource;
    private AudioSource WorldThemeHornSource => worldThemeHornSource;

    [SerializeField]
    private AudioSource deathThemeSource;
    private AudioSource DeathThemeSource => deathThemeSource;

    [SerializeField]
    private AudioLibrary audioLibrary;
    private AudioLibrary AudioLibrary => audioLibrary;

    [SerializeField]
    private AudioSource globalSourcePrefab;
    private AudioSource GlobalSourcePrefab => globalSourcePrefab;

    [SerializeField]
    private AudioSource localSourcePrefab;
    private AudioSource LocalSourcePrefab => localSourcePrefab;

    private Dictionary<AudioSource, Tween> SoundFadeTweens = new Dictionary<AudioSource, Tween>();

    private Dictionary<string, AudioSource> LoadedGlobalSounds = new Dictionary<string, AudioSource>();
    private Dictionary<(string, GameObject), AudioSource> LoadedLocalSounds = new Dictionary<(string, GameObject), AudioSource>();

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

    private void SetOnMixer(string name, float value)
    {
        float dB;
        if (value != 0)
            dB = 20.0f * Mathf.Log10(value);
        else
            dB = -144.0f;

        Mixer.SetFloat(name, dB);
    }

    public void FadeBackgroundMusic(bool fadeIn, float timeOfFade = 1f)
    {
        if (fadeIn)
            FadeInSource(BackgroundMusicSource, timeOfFade);
        else
            FadeOutSource(BackgroundMusicSource, timeOfFade);
    }

    public void FadeInBackingTrack(float timeOfFade = 1f)
    {
        FadeInSource(BackingTrackSource, timeOfFade);
    }

    public void FadeOutBackingTrack(float timeOfFade = 1f)
    {
        FadeInSource(BackingTrackSource, timeOfFade);
    }

    public void FadeInLoveTheme(float timeOfFade = 1f)
    {
        FadeInSource(LoveThemeSource, timeOfFade);

        FadeOutSource(WorldThemeSource, timeOfFade);
        FadeOutSource(WorldThemeIncorrectHarpSource, timeOfFade);
        FadeOutSource(WorldThemeCorrectHarpSource, timeOfFade);
        FadeOutSource(WorldThemeHornSource, timeOfFade);
        FadeOutSource(DeathThemeSource, timeOfFade);
    }

    public void FadeInWorldTheme(float timeOfFade = 1f)
    {
        FadeInSource(WorldThemeSource, timeOfFade);

        FadeOutSource(LoveThemeSource, timeOfFade);
        FadeOutSource(DeathThemeSource, timeOfFade);
    }

    public void FadeInWorldHorns(float timeOfFade = 1f)
    {
        FadeInSource(WorldThemeHornSource, timeOfFade);
    }

    public void FadeOutWorldHorns(float timeOfFade = 1f)
    {
        FadeOutSource(WorldThemeHornSource, timeOfFade);
    }

    public void FadeInHarpIncorrect(float timeOfFade = 1f)
    {
        FadeInSource(WorldThemeIncorrectHarpSource, timeOfFade);
        FadeOutSource(WorldThemeCorrectHarpSource, timeOfFade);
    }

    public void FadeInHarpCorrect(float timeOfFade = 1f)
    {
        FadeInSource(WorldThemeCorrectHarpSource, timeOfFade);
        FadeOutSource(WorldThemeIncorrectHarpSource, timeOfFade);
    }

    public void FadeInDeathTheme(float timeOfFade = 1f)
    {
        FadeInSource(DeathThemeSource, timeOfFade);

        FadeOutSource(LoveThemeSource, timeOfFade);
        FadeOutSource(WorldThemeSource, timeOfFade);
        FadeOutSource(WorldThemeIncorrectHarpSource, timeOfFade);
        FadeOutSource(WorldThemeCorrectHarpSource, timeOfFade);
        FadeOutSource(WorldThemeHornSource, timeOfFade);
    }

    public void FadeInSource(AudioSource source, float timeOfFade = 1f)
    {
        if (SoundFadeTweens.TryGetValue(source, out Tween tween))
        {
            tween.Stop();
            SoundFadeTweens.Remove(source);
        }

        SoundFadeTweens.Add(source, new Tween(timeOfFade)
            .OnStarted(() =>
            {
                if (!source.isPlaying)
                    source.Play();
            })
            .For(source)
                .VolumeTo(source.volume, 1)
            .OnCompleted(() =>
            {
                SoundFadeTweens.Remove(source);
            })
            .Start()
        );
    }

    public void FadeOutSource(AudioSource source, float timeOfFade = 1f)
    {
        if (SoundFadeTweens.TryGetValue(source, out Tween tween))
        {
            tween.Stop();
            SoundFadeTweens.Remove(source);
        }

        SoundFadeTweens.Add(source, new Tween(timeOfFade)
            .For(source)
                .VolumeTo(source.volume, 0)
            .OnCompleted(() =>
            {
                source.Stop();
                SoundFadeTweens.Remove(source);
            })
            .Start()
        );
    }

    public AudioSource PlayGlobalSound(string name, bool shouldOverride = true, bool shouldPlay = true)
    {
        AudioSource source;

        if (LoadedGlobalSounds.TryGetValue(name, out AudioSource loadedSource))
        {
            source = loadedSource;
            if (source.isPlaying && !shouldOverride)
                return source;
        }
        else
        {
            AudioAsset asset = AudioLibrary.GetAsset(name);
            source = Instantiate(GlobalSourcePrefab, transform);

            source.outputAudioMixerGroup = asset.Type == AudioTypes.Music ? MusicGroup : SfxGroup;
            source.clip = asset.AudioClip;
            source.volume = asset.Volume;
            source.reverbZoneMix = asset.Reverb;
            LoadedGlobalSounds.Add(name, source);
        }

        if (shouldPlay)
            source.Play();
        return source;
    }

    public AudioSource PlayLocalSound(string name, GameObject parent, bool shouldOverride = true, bool shouldPlay = true, bool shouldOverlap = false, bool loop = false)
    {
        AudioSource source;

        if (!shouldOverlap && LoadedLocalSounds.TryGetValue((name, parent), out AudioSource loadedSource))
        {
            source = loadedSource;
            if (source.isPlaying && !shouldOverride)
                return source;
        }
        else
        {
            AudioAsset asset = AudioLibrary.GetAsset(name);
            source = Instantiate(LocalSourcePrefab, parent.transform);

            source.outputAudioMixerGroup = asset.Type == AudioTypes.Music ? MusicGroup : SfxGroup;
            source.clip = asset.AudioClip;
            source.volume = asset.Volume;
            source.reverbZoneMix = asset.Reverb;
            source.loop = loop;
            source.maxDistance = asset.MaxDistance;
            LoadedLocalSounds.TryAdd((name, parent), source);
        }

        if (shouldPlay)
            source.Play();

        return source;
    }

    public AudioSource PlayRandomLocalSound(string name, int numberOfSounds, GameObject parent, bool shouldOverride = true, bool shouldPlay = true)
    {
        AudioSource source;

        if (LoadedLocalSounds.TryGetValue((name, parent), out AudioSource loadedSource))
        {
            source = loadedSource;
            if (source.isPlaying && !shouldOverride)
                return source;
        }
        else
        {
            source = Instantiate(LocalSourcePrefab, parent.transform);
            LoadedLocalSounds.Add((name, parent), source);
        }

        AudioAsset asset = AudioLibrary.GetAssetRange(name, numberOfSounds)[Random.Range(0, numberOfSounds)];
        source.clip = asset.AudioClip;
        source.outputAudioMixerGroup = asset.Type == AudioTypes.Music ? MusicGroup : SfxGroup;
        source.volume = asset.Volume;
        source.reverbZoneMix = asset.Reverb;
        source.maxDistance = asset.MaxDistance;

        if (shouldPlay)
            source.Play();
        return source;
    }

    public void StartBackgroundMusic()
    {
        BackgroundMusicSource.Play();
    }
}
