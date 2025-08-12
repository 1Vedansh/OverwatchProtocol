using UnityEngine;

public class ProceduralWorldStart : MonoBehaviour
{
    public GameObject proceduralWorldGenerator;
    public Player playerController;
    public string worldName;
    public int gameDifficulty;

    public void OnEnable()
    {
        worldName = PlayerPrefs.GetString("CurrentWorld");
        // Debug.Log(worldName);
        WorldData worldData = JsonUtility.FromJson<WorldData>(PlayerPrefs.GetString(worldName + "_Data"));
        // Debug.Log(PlayerPrefs.GetString(worldName + "_Data"));
        playerController.playerHealth = worldData.playerHealth;
        playerController.totalSniperAmmo = worldData.totalSniperAmmo;
        playerController.totalRifleAmmo = worldData.totalRifleAmmo;
        gameDifficulty = worldData.gameDifficulty;
        proceduralWorldGenerator.GetComponent<Parameters>().seed = Random.Range(1, 1000000);
        proceduralWorldGenerator.SetActive(true);
    }
}
