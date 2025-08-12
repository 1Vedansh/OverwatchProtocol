using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitWorld : MonoBehaviour
{
    public bool toggle;
    public GameObject exitButton;
    public Player player;

    void Start()
    {
        toggle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (toggle)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
            toggle = !toggle;
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        exitButton.SetActive(true);
    }

    void UnpauseGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        exitButton.SetActive(false);
        Cursor.visible = false;
    }

    public void exitClick()
    {
        WorldData worldData = new WorldData();

        worldData.playerHealth = player.playerHealth;
        worldData.totalRifleAmmo = player.totalRifleAmmo;
        worldData.totalSniperAmmo = player.totalSniperAmmo;
        worldData.gameDifficulty = player.gameDifficulty;

        string worldName = PlayerPrefs.GetString("CurrentWorld");
        PlayerPrefs.SetString(worldName + "_Data", JsonUtility.ToJson(worldData));
        PlayerPrefs.Save();

        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
    }
}
