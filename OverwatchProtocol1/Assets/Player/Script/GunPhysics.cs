using System.Collections;
using UnityEngine;

public class GunPhysics : MonoBehaviour
{
    // Final transform multiplier
    float positionMultipier;
    float rotationMultiplier;


    // Recoil variables
    float recoilPositionSpeed;
    Vector3 recoilPositionBounds;
    Vector3 recoilPositionOffset;

    float recoilRotationSpeed;
    Vector3 recoilRotationBounds;
    Quaternion recoilRotationOffset;


    // Sway variables
    float swayMultiplier;
    Quaternion swayRotationOffset;


    // View Bobbing variables
    Vector3 viewBobbingMultiplier;
    float viewBobbingSpeed;
    float viewBobbingSinTime;
    Vector3 viewBobbingPositionOffset;


    // Reference variables
    Vector3 originalPosition;
    Vector3 currentPosition;
    Vector3 finalTargetPosition;

    Quaternion originalRotation;
    Quaternion currentRotation;
    Quaternion finalTargetRotation;


    // Gun manager
    WeaponManager weaponManager;


    // ADS
    float adsFactor;
    AimDownSights aimDownSights;

    bool wait = false;

    void OnEnable()
    {
        weaponManager = transform.GetComponent<WeaponManager>();
        aimDownSights = transform.GetComponent<AimDownSights>();

        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        positionMultipier = weaponManager.positionMultipier;
        rotationMultiplier = weaponManager.rotationMultiplier;
        recoilPositionSpeed = weaponManager.recoilPositionSpeed;
        recoilPositionBounds = weaponManager.recoilPositionBounds;
        recoilRotationSpeed = weaponManager.recoilRotationSpeed;
        recoilRotationBounds = weaponManager.recoilRotationBounds;
        swayMultiplier = weaponManager.swayMultiplier;
        viewBobbingMultiplier = weaponManager.viewBobbingMultiplier;
        viewBobbingSpeed = weaponManager.viewBobbingSpeed;
        adsFactor = weaponManager.adsFactor;

        StartCoroutine(waitForAnimation());
    }

    // dont want to start procedural motions before the animaion finishes or it will look bad
    IEnumerator waitForAnimation()
    {
        yield return new WaitForSeconds(2f);
        wait = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            calculateRecoilOffset();
            calculateSwayOffset();
            calculateViewBobbingOffset();

            if (weaponManager.adsStatus)
            {
                finalTargetRotation = swayRotationOffset * originalRotation * recoilRotationOffset;
                finalTargetRotation = Quaternion.Slerp(Quaternion.identity, finalTargetRotation, 1 / adsFactor);
                finalTargetPosition = originalPosition + aimDownSights.adsOffset + (recoilPositionOffset + viewBobbingPositionOffset) / adsFactor;
            }
            else
            {
                finalTargetRotation = swayRotationOffset * originalRotation * recoilRotationOffset;
                finalTargetPosition = originalPosition + aimDownSights.adsOffset + recoilPositionOffset + viewBobbingPositionOffset;
            }

            currentPosition = Vector3.Slerp(currentPosition, finalTargetPosition, positionMultipier * Time.deltaTime);
            currentRotation = Quaternion.Slerp(currentRotation, finalTargetRotation, rotationMultiplier * Time.deltaTime);

            transform.SetLocalPositionAndRotation(currentPosition, currentRotation);
        }
    }

    public void applyRecoil()
    {
        recoilPositionOffset += new Vector3(Random.Range(-recoilPositionBounds.x, recoilPositionBounds.x), recoilPositionBounds.y, Random.Range(recoilPositionBounds.z / 2, recoilPositionBounds.z));
        recoilRotationOffset *= Quaternion.Euler(Random.Range(-recoilRotationBounds.x, 0), Random.Range(-recoilRotationBounds.y, recoilRotationBounds.y), Random.Range(-recoilRotationBounds.z, recoilRotationBounds.z));
    }

    void calculateRecoilOffset()
    {
        recoilPositionOffset = Vector3.Lerp(recoilPositionOffset, Vector3.zero, recoilPositionSpeed * Time.deltaTime);
        recoilRotationOffset = Quaternion.Lerp(recoilRotationOffset, Quaternion.identity, recoilRotationSpeed * Time.deltaTime);
    }

    void calculateSwayOffset()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;

        Quaternion swayX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion swayY = Quaternion.AngleAxis(mouseX, Vector3.up);
        swayRotationOffset = swayX * swayY;
    }

    void calculateViewBobbingOffset()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        if (inputVector.magnitude > 0f)
        {
            viewBobbingSinTime += Time.deltaTime * viewBobbingSpeed;
        }
        else
        {
            viewBobbingSinTime = 0f;
        }

        float xOffset = Mathf.Sin(viewBobbingSinTime) * viewBobbingMultiplier.x;
        float yOffset = Mathf.Abs(viewBobbingMultiplier.y * Mathf.Sin(viewBobbingSinTime));
        float zOffset = Mathf.Sin(viewBobbingSinTime) * viewBobbingMultiplier.z;

        viewBobbingPositionOffset = new Vector3(xOffset, yOffset, zOffset);
    }
}
