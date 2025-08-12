using UnityEngine;

public class UpdateVolume : MonoBehaviour
{
    GameSoundManager gameSoundManager;

    void Start()
    {
        gameSoundManager = FindAnyObjectByType<GameSoundManager>();
        transform.GetComponent<AudioSource>().volume = gameSoundManager.audioMultiplier;
    }
}
