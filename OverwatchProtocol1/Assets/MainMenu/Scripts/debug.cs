using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class debug : MonoBehaviour
{
    public string deleteWorldName;
    public void clicked()
    {
        PlayerPrefs.DeleteAll();
        // WorldSavesList temp = JsonUtility.FromJson<WorldSavesList>(PlayerPrefs.GetString("WorldNames"));

        // for (int i = temp.worldNamesList.Count - 1; i >= 0; i--)
        // {
        //     if (temp.worldNamesList[i] == deleteWorldName)
        //     {
        //         temp.worldNamesList.RemoveAt(i);
        //         PlayerPrefs.DeleteKey(deleteWorldName + "_Data");
        //         PlayerPrefs.SetString("WorldNames", JsonUtility.ToJson(temp));
        //     }
        // }

        // Debug.Log(PlayerPrefs.GetFloat("Volume"));
        // Debug.Log(PlayerPrefs.GetFloat("Sensitivity"));
    }
}
