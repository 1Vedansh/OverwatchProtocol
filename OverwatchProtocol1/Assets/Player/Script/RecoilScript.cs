using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    public Vector3 cameraRecoilBounds;
    public float cameraRotationMultiplier;
    public float cameraRecoilRotationSpeed;
    
    public WeaponManager weaponManager;

    Quaternion originalRotation;
    Quaternion targetRotation;
    Quaternion currentRotation;

    Quaternion finalTargetRotation;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // if (weaponManager.cameraADSFactor <= 0f) return;
        
        finalTargetRotation = originalRotation * targetRotation;
        if (weaponManager.adsStatus)
        {
            finalTargetRotation = Quaternion.Lerp(Quaternion.identity, finalTargetRotation, 1 / weaponManager.cameraADSFactor);
        }

        targetRotation = Quaternion.Lerp(targetRotation, Quaternion.identity, cameraRecoilRotationSpeed * Time.deltaTime);
        currentRotation = Quaternion.Slerp(currentRotation, finalTargetRotation, cameraRotationMultiplier * Time.deltaTime);
        transform.localRotation = currentRotation;
    }

    public void recoilFire()
    {
        targetRotation *= Quaternion.Euler(cameraRecoilBounds.x, Random.Range(-cameraRecoilBounds.y, cameraRecoilBounds.y), Random.Range(-cameraRecoilBounds.z, cameraRecoilBounds.z));
    }
}
