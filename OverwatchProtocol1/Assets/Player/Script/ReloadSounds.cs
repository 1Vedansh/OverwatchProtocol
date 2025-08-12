using UnityEngine;

public class ReloadSounds : MonoBehaviour
{
    public SoundManager soundManager;

    public void removeMag()
    {
        soundManager.playSound(SoundType.REMOVE_MAGAZINE, 1f);
    }

    public void insertMag()
    {
        soundManager.playSound(SoundType.INSERT_MAGAZINE, 1f);
    }
}
