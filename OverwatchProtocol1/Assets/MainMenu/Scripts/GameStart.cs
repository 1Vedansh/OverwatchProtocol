using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Slider volume;
    public TMP_InputField sensitivity;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 100f);
        }
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 300f);
        }
        PlayerPrefs.Save();

        volume.value = PlayerPrefs.GetFloat("Volume");
        sensitivity.text = PlayerPrefs.GetFloat("Sensitivity").ToString("F2");
    }
}
