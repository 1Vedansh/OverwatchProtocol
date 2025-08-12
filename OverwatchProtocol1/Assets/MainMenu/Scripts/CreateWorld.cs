using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateWorld : MonoBehaviour
{
    public TMP_InputField tMP_InputField;
    public GameObject errorText;
    public GameObject GoBack;
    public ToggleGroup toggleGroup;
    
    WorldSavesList worldSavesList;
    List<string> worldNamesList;

    void OnEnable()
    {
        worldSavesList = JsonUtility.FromJson<WorldSavesList>(PlayerPrefs.GetString("WorldNames"));
        worldNamesList = worldSavesList.worldNamesList;
    }

    public void OnClick()
    {
        string worldName = tMP_InputField.text;
        bool flag = false;
        for (int i = 0; i < worldNamesList.Count; i++)
        {
            if (worldName == worldNamesList[i])
            {
                flag = true;
            }
        }
        if (flag)
        {
            StartCoroutine(displayError());
        }
        else
        {
            Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

            WorldData worldData = new WorldData();
            worldData.playerHealth = 100;
            string gameDifficulty = toggle.GetComponentInChildren<Text>().text.ToString();

            if (gameDifficulty == "Easy")
            {
                worldData.gameDifficulty = 0;
                worldData.totalRifleAmmo = 250;
                worldData.totalSniperAmmo = 50;
            }
            else if (gameDifficulty == "Medium")
            {
                worldData.gameDifficulty = 1;
                worldData.totalRifleAmmo = 150;
                worldData.totalSniperAmmo = 25;
            }
            else if (gameDifficulty == "Hard")
            {
                worldData.gameDifficulty = 2;
                worldData.totalRifleAmmo = 100;
                worldData.totalSniperAmmo = 15;
            }

            worldSavesList.worldNamesList.Add(worldName);

            PlayerPrefs.SetString("WorldNames", JsonUtility.ToJson(worldSavesList));
            PlayerPrefs.SetString(worldName + "_Data", JsonUtility.ToJson(worldData));
            PlayerPrefs.Save();

            GoBack.GetComponent<DestinationButton>().goToDestination();
        }
    }

    IEnumerator displayError()
    {
        errorText.SetActive(true);
        yield return new WaitForSeconds(5f); 
        errorText.SetActive(false);
    }
}
