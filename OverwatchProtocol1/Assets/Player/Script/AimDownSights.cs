using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    WeaponManager weaponManager;
    Transform eyePosition;
    Transform adsPosition;
    float scopeSpeed;
    Camera adsCamera;
    float adsScrollSpeed;
    MouseLook mouseLook;
    float maxFOV;
    MeshRenderer adsCameraRenderer;


    [HideInInspector]
    public Vector3 adsOffset;

    Vector3 originalPosition;
    Quaternion originalRotation;
    bool isScoping;
    Vector3 offset;
    float originalSensitivity;

    void Start()
    {
        weaponManager = transform.GetComponent<WeaponManager>();

        isScoping = false;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        eyePosition = weaponManager.eyePosition;
        adsPosition = weaponManager.adsPosition;
        scopeSpeed = weaponManager.scopeSpeed;
        adsCamera = weaponManager.adsCamera;
        adsScrollSpeed = weaponManager.adsScrollSpeed;
        mouseLook = weaponManager.mouseLook;
        maxFOV = weaponManager.maxFOV;
        originalSensitivity = mouseLook.mouseSensitivity;
        adsCameraRenderer = weaponManager.adsCameraRenderer.GetComponent<MeshRenderer>();
    }

    public void ADSEnable()
    {
        adsCamera.enabled = true;
        adsCameraRenderer.enabled = true;
        adsCamera.fieldOfView -= Input.mouseScrollDelta.y * adsScrollSpeed;
        adsCamera.fieldOfView = Mathf.Clamp(adsCamera.fieldOfView, 0, maxFOV);

        mouseLook.mouseSensitivity = originalSensitivity/2;

        if (!isScoping)
        {
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;

            // procedural animation for bringing the scope near the eye
            offset = transform.parent.InverseTransformDirection(eyePosition.position - adsPosition.position);
            isScoping = true;
        }
        else
        {
            adsOffset = Vector3.Lerp(adsOffset, offset, scopeSpeed * Time.deltaTime);
        }
    }

    public void ADSDisable()
    {
        adsCamera.enabled = false;
        adsCameraRenderer.enabled = false;
        mouseLook.mouseSensitivity = originalSensitivity;
        isScoping = false;
        adsOffset = Vector3.zero;
    }
}
