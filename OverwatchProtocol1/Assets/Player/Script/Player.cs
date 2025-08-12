using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public int playerHealth;
    public int totalRifleAmmo;
    public int totalSniperAmmo;

    public Volume volume;

    public GameObject[] weapons;
    public AudioSource takeDamageAudio;

    public float originalVignetteIntensity;
    public float originalVignetteSmoothness;
    public Color originalVignetteColor;

    public float damageVignetteDuration;
    public float targetVignetteIntensity;
    public float targetVignetteSmoothness;
    public Color targetVignetteColor;

    public Vector2Int killHealthReward;
    public Vector2Int killAmmoRewardRifle;
    public Vector2Int killAmmoRewardSniper;

    public PlayerProperties easyPlayer;
    public PlayerProperties mediumPlayer;
    public PlayerProperties hardPlayer;

    public GameObject deathScreen;

    public int gameDifficulty;

    public ProceduralWorldStart proceduralWorldStart;
    Vignette vignette;
    int currentWeapon;

    public void setDifficulty(PlayerProperties playerProperties)
    {
        killHealthReward = playerProperties.killHealthReward;
        killAmmoRewardRifle = playerProperties.killAmmoRewardRifle;
        killAmmoRewardSniper = playerProperties.killAmmoRewardSniper;
    }

    public void Start()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        currentWeapon = 1;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i + 1 != currentWeapon)
            {
                weapons[i].SetActive(false);
            }
            else
            {
                weapons[i].SetActive(true);
            }
        }
        gameDifficulty = proceduralWorldStart.gameDifficulty;

        if (gameDifficulty == 0)
        {
            setDifficulty(easyPlayer);
        }
        else if (gameDifficulty == 1)
        {
            setDifficulty(mediumPlayer);
        }
        else if (gameDifficulty == 2)
        {
            setDifficulty(hardPlayer);
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1) && currentWeapon != 1)
        {
            weapons[currentWeapon - 1].SetActive(false);
            weapons[0].SetActive(true);
            currentWeapon = 1;
        }
        if (Input.GetKey(KeyCode.Alpha2) && currentWeapon != 2)
        {
            weapons[currentWeapon - 1].SetActive(false);
            weapons[1].SetActive(true);
            currentWeapon = 2;
        }
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    public Vector2 getCurrentWeaponAmmo()
    {
        Vector2 info;
        if (currentWeapon == 1)
        {
            info.y = totalRifleAmmo;
        }
        else
        {
            info.y = totalSniperAmmo;
        }
        info.x = weapons[currentWeapon - 1].GetComponent<WeaponManager>().currentAmmo;
        return info;
    }

    public void takeDamage(int damage)
    {
        takeDamageAudio.Play();
        damageVignetteEffect();
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    public void enemyKillReward()
    {
        playerHealth += Random.Range(killHealthReward.x, killHealthReward.y);
        if (currentWeapon == 1)
        {
            totalRifleAmmo += Random.Range(killAmmoRewardRifle.x, killAmmoRewardRifle.y);
        }
        else if (currentWeapon == 2)
        {
            totalSniperAmmo += Random.Range(killAmmoRewardSniper.x, killAmmoRewardSniper.y);
        }
    }

    public void damageVignetteEffect()
    {
        vignette.intensity.value = targetVignetteIntensity;
        vignette.smoothness.value = targetVignetteSmoothness;
        vignette.color.value = targetVignetteColor;

        StopAllCoroutines();
        StartCoroutine(VignetteEffectRoutine(damageVignetteDuration));
    }

    IEnumerator VignetteEffectRoutine(float duration)
    {
        float time = 0f;
        float startIntensity = vignette.intensity.value;
        float startSmoothness = vignette.smoothness.value;

        while (time < duration)
        {
            vignette.intensity.value = Mathf.Lerp(startIntensity, originalVignetteIntensity, time / duration);
            vignette.smoothness.value = Mathf.Lerp(startSmoothness, originalVignetteSmoothness, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        
        vignette.intensity.value = originalVignetteIntensity;
        vignette.smoothness.value = originalVignetteSmoothness;
        vignette.color.value = originalVignetteColor;
    }

    IEnumerator deathVignetteEffectRoutine(float duration)
    {
        float time = 0f;
        vignette.color.value = targetVignetteColor;
        float startIntensity = vignette.intensity.value;
        float startSmoothness = vignette.smoothness.value;

        while (time < duration)
        {
            vignette.intensity.value = Mathf.Lerp(startIntensity, targetVignetteIntensity, time / duration);
            vignette.smoothness.value = Mathf.Lerp(startSmoothness, targetVignetteSmoothness, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void Die()
    {
        StartCoroutine(deathVignetteEffectRoutine(5f));
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void reduceAmmo(string gunName, int amount)
    {
        if (gunName == "FuturisticRifle1")
        {
            totalRifleAmmo -= amount;
        }
        else if (gunName == "FuturisticSniper1")
        {
            totalSniperAmmo -= amount;
        }
    }

    public int returnTotalAmmo(string gunName)
    {
        if (gunName == "FuturisticRifle1")
        {
            return totalRifleAmmo;
        }
        else if (gunName == "FuturisticSniper1")
        {
            return totalSniperAmmo;
        }
        return -1;
    }

    public void OnDestroy()
    {
        Time.timeScale = 1f;        
    }
}

[System.Serializable]
public class PlayerProperties
{
    public Vector2Int killHealthReward;
    public Vector2Int killAmmoRewardRifle;
    public Vector2Int killAmmoRewardSniper;
}