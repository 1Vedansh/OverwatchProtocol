using TMPro;
using UnityEngine;

public class updateUI : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text magazineText;
    public Player playerController;

    void LateUpdate()
    {
        Vector2 magazineInfo = playerController.getCurrentWeaponAmmo();
        healthText.text = playerController.playerHealth.ToString();
        magazineText.text = magazineInfo.x.ToString() + "/" + (magazineInfo.y - magazineInfo.x).ToString();
    }
}
