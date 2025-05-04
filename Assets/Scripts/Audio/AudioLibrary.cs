using System;
using UnityEngine;

[System.Serializable]
public struct AudioAsset
{
    [SerializeField]
    private string assetName;
    public string AssetName => assetName;

    [SerializeField]
    private AudioClip audioClip;
    public AudioClip AudioClip => audioClip;

    [SerializeField]
    private AudioTypes type;
    public AudioTypes Type => type;

    [SerializeField, Range(0, 1)]
    private float volume;
    public float Volume => volume;

    [SerializeField, Range(0, 1.1f)]
    private float reverb;
    public float Reverb => reverb;
}

[CreateAssetMenu]
public class AudioLibrary : ScriptableObject
{
    [SerializeField]
    private AudioAsset[] audioAssets;
    public AudioAsset[] AudioAssets => audioAssets;

    public AudioAsset GetAsset(string name)
    {
        AudioAsset asset;

        asset = Array.Find(AudioAssets, asset => asset.AssetName == name);

        return asset;
    }

    public AudioAsset GetAsset(int id)
    {
        AudioAsset asset = AudioAssets[id];
        return asset;
    }

    public ArraySegment<AudioAsset> GetAssetRange(string name, int numberOfAssets)
    {
        AudioAsset startAsset = GetAsset(name);
        int startIndex = Array.IndexOf(AudioAssets, startAsset);
        ArraySegment<AudioAsset> segment = new ArraySegment<AudioAsset>(AudioAssets, startIndex, numberOfAssets);
        return segment;
    }

    public ArraySegment<AudioAsset> GetAssetRange(int id, int numberOfAssets)
    {
        ArraySegment<AudioAsset> segment = new ArraySegment<AudioAsset>(AudioAssets, id, numberOfAssets);
        return segment;
    }
}
