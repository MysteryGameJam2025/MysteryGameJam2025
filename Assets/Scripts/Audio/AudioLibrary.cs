using System;
using UnityEditor.VersionControl;
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
