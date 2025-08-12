using UnityEngine;
using UnityEngine.SceneManagement;

public class ValueChanged : MonoBehaviour
{
    public string propertyName;

    public void changeFloat(float value)
    {
        valueChanger(value, propertyName);
    }

    public void changeString(string value)
    {
        float temp;
        if (float.TryParse(value, out temp))
        {
            Debug.Log("Parsed float: " + temp);
            valueChanger(temp, propertyName);
        }
        else
        {
            Debug.Log("Invalid input");
        }
    }

    public void valueChanger(float value, string propertyName)
    {
        PlayerPrefs.SetFloat(propertyName, value);
    }

}
