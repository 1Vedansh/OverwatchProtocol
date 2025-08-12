using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponManager : MonoBehaviour
{
    [Header("Unique Gun Name")]
    [Tooltip("The Unique Gun name to access assets")]
    public string gunName;


    [Header("Gun Specific")]
    public int damage;
    public float range;
    public float force;
    public float fireRate;
    public int maxAmmo;


    [Header("Animator Controller")]
    public Animator animator;
    public RigBuilder rigBuilder;
    public Transform foreArmReference;
    public MultiPositionConstraint foreArmMultiPositionConstraint;
    public MultiRotationConstraint foreArmMultiRotationConstraint;
    [Tooltip("The multi position and rotation constratints will follow this")]
    public Transform recoilGunReference;


    [Header("SFX")]
    public ReloadSounds reloadSounds;
    public SoundManager soundManager;


    [Header("Cosmetic")]
    [Tooltip("Position in front of the gun barrel where bullet is supposed to come out of")]
    public Transform bulletStart;
    [Tooltip("Bullet prefab")]
    public GameObject bullet;
    [Tooltip("Bullet speed (cosmetic/visual only)(10)")]
    public float bulletSpeed;
    [Tooltip("Muzzle flash particle system")]
    public ParticleSystem muzzle;
    [Tooltip("Bullet hole decay time(5)")]
    public float bulletDecay;
    [Tooltip("Bullet Hole Prefab")]
    public GameObject impact;


    [Header("Player Camera")]
    [Tooltip("FPS camera reference for raycast")]
    public Camera fpsCam;


    [Header("Gun Physics")]
    [Tooltip("The reference to the GunPhysics Script")]
    public GunPhysics gunPhysics;
    [Tooltip("Slerp speed for the final position offset(50)")]
    public float positionMultipier;
    [Tooltip("Slerp speed for the final rotation offset(5)")]
    public float rotationMultiplier;
    [Tooltip("Lerp speed for recoil offset(5)")]
    public float recoilPositionSpeed;
    [Tooltip("Randomized bounds for recoil(0.02,0.02,-0.07)")]
    public Vector3 recoilPositionBounds;
    [Tooltip("Lerp speed for recoil offset(5)")]
    public float recoilRotationSpeed;
    [Tooltip("Randomized bounds for recoil(3,5,5)")]
    public Vector3 recoilRotationBounds;
    [Tooltip("How much to multiply the mouse input by(3)")]
    public float swayMultiplier;
    [Tooltip("Multiplier of each axis for view bobbing(0.02,0.02,0)")]
    public Vector3 viewBobbingMultiplier;
    [Tooltip("Sin time speed for view bobbing offset(6)")]
    public float viewBobbingSpeed;
    [Tooltip("Randomized bounds for camera recoil(-1.5,1.5,0.35)")]
    public Vector3 cameraRecoilBounds;
    [Tooltip("Lerp speed for camera recoil(2)")]
    public float cameraRecoilRotationSpeed;
    [Tooltip("Slerp speed for final camera rotation speed(6)")]
    public float cameraRotationMultiplier;
    [Tooltip("Camera Recoil Script reference")]
    public RecoilScript recoilScript;


    [Header("Aim Down Sights")]
    [Tooltip("Is the weapon allowed to ADS")]
    public bool canADS;
    [Tooltip("Reference to the ads script")]
    public AimDownSights aimDownSights;
    [Tooltip("How much to reduce the offsets by for ADS(20)")]
    public float adsFactor;
    [Tooltip("How much to reduce the offsets for camera for ADS(5)")]
    public float cameraADSFactor;
    [Tooltip("Player eye position(center of the screen)")]
    public Transform eyePosition;
    [Tooltip("Position just in front of the weapon scope (this will go to eye position)")]
    public Transform adsPosition;
    [Tooltip("Procedural scope animation speed(10)")]
    public float scopeSpeed;
    [Tooltip("Camera used to render the scope effect")]
    public Camera adsCamera;
    [Tooltip("Object which is used as a screen to render the scope camera")]
    public Transform adsCameraRenderer;
    [Tooltip("Scroll speed for adjusting scope FOV(2.5)")]
    public float adsScrollSpeed;
    [Tooltip("Reference to mouse look script for adjusting sensitivity")]
    public MouseLook mouseLook;
    [Tooltip("Max zoom out(FOV) allowed(60)")]
    public float maxFOV;


    [Header("Shooting Error")]
    public float sprayMultiplier;


    [Header("Misc")]
    public CharacterController character;
    public Player player;
    public PlayerMovement playerMovement;
    public ExitWorld exitWorld;

    [HideInInspector] public float currentAmmo;
    [HideInInspector] public bool adsStatus;

    Vector3 bulletStartPoint;
    Vector3 bulletEndPoint;
    float nextTimeToFire = 0f;
    bool needReload;

    void reloadAmmo()
    {
        if (player.returnTotalAmmo(gunName) / maxAmmo > 0)
        {
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo = player.returnTotalAmmo(gunName);
        }
    }

    void OnEnable()
    {
        animator.enabled = true;
        StartCoroutine(equip());

        reloadSounds.soundManager = soundManager;

        reloadAmmo();
        needReload = false;
        adsStatus = false;

        recoilScript.cameraRecoilBounds = cameraRecoilBounds;
        recoilScript.cameraRecoilRotationSpeed = cameraRecoilRotationSpeed;
        recoilScript.cameraRotationMultiplier = cameraRotationMultiplier;
        recoilScript.weaponManager = this;
    }

    IEnumerator equip()
    {
        animator.Play("equip_" + gunName, -1, 0f);
        soundManager.playSound(SoundType.EQUIP);
        yield return new WaitForSeconds(1f);
        animator.enabled = false;

        recoilGunReference.SetPositionAndRotation(foreArmReference.position, foreArmReference.rotation);

        foreArmMultiPositionConstraint.data.sourceObjects = new WeightedTransformArray { new WeightedTransform(recoilGunReference.transform, 1f) };
        foreArmMultiRotationConstraint.data.sourceObjects = new WeightedTransformArray { new WeightedTransform(recoilGunReference.transform, 1f) };

        rigBuilder.Build();
    }

    IEnumerator reloadAnimation()
    {
        animator.enabled = true;
        animator.Rebind();
        animator.Update(0f);
        animator.Play("reload_" + gunName);
        reloadAmmo();
        yield return new WaitForSeconds(2f);
        animator.enabled = false;
        needReload = false;
    }


    void Update()
    {
        if (!exitWorld.toggle)
        {
            if (Input.GetKey(KeyCode.R) && (currentAmmo < maxAmmo) && (currentAmmo != player.returnTotalAmmo(gunName)))
            {
                needReload = true;
                StartCoroutine(reloadAnimation());
            }

            if (currentAmmo <= 0)
            {
                needReload = true;
            }

            if (canADS)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    aimDownSights.ADSEnable();
                    adsStatus = true;
                }
                else
                {
                    adsStatus = false;
                    aimDownSights.ADSDisable();
                }
            }

            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                if (!needReload)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                    recoilScript.recoilFire();
                    gunPhysics.applyRecoil();
                    soundManager.playSound(SoundType.SHOOT, 0.5f);
                    muzzle.Play();
                    currentAmmo--;
                    player.reduceAmmo(gunName, 1);
                }
                else
                {
                    if (Input.GetButtonDown("Fire1"))
                        soundManager.playSound(SoundType.EMPTY_MAG);
                }
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        bulletStartPoint = bulletStart.position;

        Vector3 randomSpray = fpsCam.transform.forward;

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || !playerMovement.isGrounded)
        {
            randomSpray += (fpsCam.transform.up * Random.Range(-sprayMultiplier, sprayMultiplier)) + (fpsCam.transform.right * Random.Range(-sprayMultiplier,sprayMultiplier));
        }

        if (Physics.Raycast(fpsCam.transform.position, randomSpray, out hit, range) && hit.transform.GetComponent<CharacterController>() != character)
        {

            Debug.Log(hit.transform.name);
            GameObject bulletHole;

            if (hit.transform.GetComponent<Hitbox>() != null)
            {
                hit.transform.GetComponent<Hitbox>().onHit(damage);
                bulletHole = null;
            }
            else
            {
                bulletHole = spawnBulletHole(hit);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);
                if (bulletHole != null)
                {
                    bulletHole.transform.SetParent(hit.transform);
                }
            }

            bulletEndPoint = hit.point;
        }
        else
        {
            bulletEndPoint = bulletStartPoint + fpsCam.transform.forward * range;
        }
        spawnBulletTrail();
    }

    void spawnBulletTrail()
    {
        Vector3 rot = bulletEndPoint - bulletStart.position;
        GameObject bulletTrail = Instantiate(bullet, bulletStart.position, Quaternion.LookRotation(rot));
        bulletTrail.GetComponent<BulletTrail>().endPoint = bulletEndPoint;
        
    }

    GameObject spawnBulletHole(RaycastHit hit)
    {
        GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
        ParticleSystem temp = impactGO.GetComponent<ParticleSystem>();
        impactGO.transform.localRotation *= Quaternion.Euler(0, 0, Random.Range(0, 360));
        Destroy(impactGO, bulletDecay);
        return impactGO;
    }
}
