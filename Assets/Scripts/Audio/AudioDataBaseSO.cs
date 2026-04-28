using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Audio DataBase", fileName = "Audio DataBase")]
public class AudioDataBaseSO : ScriptableObject
{
    public List<AudioClipData> player;
    public List<AudioClipData> ui;

    Dictionary<string, AudioClipData> audioCollections;

    private void OnEnable()
    {
        audioCollections = new Dictionary<string, AudioClipData>();

        AddToCollections(player);
        AddToCollections(ui);
    }

    public AudioClipData GetAudioClipData(string groupName)
    {
        return audioCollections.TryGetValue(groupName, out var audioClipData) ? audioClipData : null;
    }

    public void AddToCollections(List<AudioClipData> list)
    {
        foreach (var audioClipData in list)
        {
            if (audioClipData != null && !audioCollections.ContainsKey(audioClipData.clipName))
                audioCollections.Add(audioClipData.clipName, audioClipData);
        }
    }
}

[System.Serializable]
public class AudioClipData
{
    public string clipName;
    public List<AudioClip> clipList;
    [Range(0, 1)] public float clipVolume = 1;

    public AudioClip GetRandomClip()
    {
        return clipList[Random.Range(0, clipList.Count)];
    }
}
