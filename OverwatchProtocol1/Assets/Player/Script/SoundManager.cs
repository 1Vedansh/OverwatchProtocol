using System;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundType
{
    EQUIP,
    REMOVE_MAGAZINE,
    INSERT_MAGAZINE,
    SHOOT,
    EMPTY_MAG
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    public sound[] soundList;
    public AudioSource audioSource;
    public GameSoundManager gameSoundManager;
    
    void OnEnable()
    {
        audioSource = transform.GetComponent<AudioSource>();
        
        #if UNITY_EDITOR
            string[] names = Enum.GetNames(typeof(SoundType));
            Array.Resize(ref soundList, names.Length);
            for (int i = 0; i < names.Length; i++)
            {
                soundList[i].name = names[i];
            }
        #endif
    }

    public void playSound(SoundType sound, float volume = 1f)
    {
        if (sound == SoundType.SHOOT)
        {
            audioSource.clip = soundList[(int)sound].soundEffect;
            audioSource.volume = gameSoundManager.audioMultiplier * volume;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(soundList[(int)sound].soundEffect, volume * gameSoundManager.audioMultiplier);
        }
        
    }
}

[Serializable]
public struct sound
{
    [HideInInspector] public string name;
    public AudioClip soundEffect;
}