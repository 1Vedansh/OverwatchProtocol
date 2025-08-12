using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadWorldSaves : MonoBehaviour
{
    public GameObject worldPrefab;
    public Transform scrollableContent;

    void OnEnable()
    {
        // reset the scrollable viewport everytime or dulicates will show up
        foreach (Transform child in scrollableContent)
        {
            Destroy(child.gameObject);
        }

        // Make key for the first time
        if (!PlayerPrefs.HasKey("WorldNames"))
        {
            WorldSavesList tempList = new WorldSavesList();
            tempList.worldNamesList = new List<string>(0);
            PlayerPrefs.SetString("WorldNames", JsonUtility.ToJson(tempList));
            PlayerPrefs.Save();
        }

        WorldSavesList temp = JsonUtility.FromJson<WorldSavesList>(PlayerPrefs.GetString("WorldNames"));
        List<string> worldNamesList = temp.worldNamesList;

        // Display all the worlds
        for (int i = 0; i < worldNamesList.Count; i++)
        {
            WorldData tempWorldData = JsonUtility.FromJson<WorldData>(PlayerPrefs.GetString(worldNamesList[i] + '_' + "Data"));

            GameObject temporaryGameObject = Instantiate(worldPrefab, scrollableContent);

            RectTransform rectTransform = temporaryGameObject.GetComponent<RectTransform>();
            rectTransform.localPosition += new Vector3(0, -(rectTransform.rect.height + 50) * i, 0);

            TMP_Text worldName = temporaryGameObject.transform.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text difficulty = temporaryGameObject.transform.GetChild(1).GetComponent<TMP_Text>();
            worldName.text = worldNamesList[i];
            int difficultyINT = tempWorldData.gameDifficulty;
            if (difficultyINT == 0)
            {
                difficulty.text = "Easy";
            }
            else if (difficultyINT == 1)
            {
                difficulty.text = "Medium";
            }
            else if (difficultyINT == 2)
            {
                difficulty.text = "Hard";
            }
        }
    }
}

[System.Serializable]
public class WorldSavesList
{
    public List<string> worldNamesList;
}

[System.Serializable]
public class WorldData
{
    public int gameDifficulty;
    public int playerHealth;
    public int totalRifleAmmo;
    public int totalSniperAmmo;
}