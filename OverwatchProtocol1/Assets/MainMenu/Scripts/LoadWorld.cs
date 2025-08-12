using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadWorld : MonoBehaviour
{
    public void onClick()
    {
        PlayerPrefs.SetString("CurrentWorld", transform.GetChild(0).GetComponent<TMP_Text>().text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("ProceduralWorld", LoadSceneMode.Single);
    }
}
