using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameSoundManager : MonoBehaviour
{
    public float audioMultiplier;
    
    AudioSource[] audioSources;
    float[] originalVolumes;

    void Start()
    {
        audioMultiplier = PlayerPrefs.GetFloat("Volume")/100f;
        audioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        originalVolumes = new float[audioSources.Length];
        for (int i = 0; i < originalVolumes.Length; i++)
        {
            originalVolumes[i] = audioSources[i].volume;
        }

        ApplyVolume();
    }

    public void ApplyVolume()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = originalVolumes[i] * audioMultiplier;
        }
    }
}
