using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // amount of time to wait before moving
    public float pause;

    // Stay in shooting mode for this time (To avoid constant navmesh agent recalculation)
    public float shootCommitTime;

    public Transform player;
    public Transform enemyEyeSight;
    public Transform playerEyeSight;
    public LayerMask whatIsGround;
    public float walkPointRange;
    public float enemySightRange;
    public float enemyShootRange;
    public float walkSpeed;
    public float runSpeed;
    public float fireRate;
    public int damage;
    public float hitChance;
    public GameObject gun;
    public ParticleSystem muzzle;
    public Player playerController;

    AudioSource audioSource;
    float shootStateEndTime = 0f;
    float currentTime;
    float nextTimeToFire;
    NavMeshAgent agent;
    float errorDistance = 100f;
    Vector3 walkPoint;
    bool walkPointSet;
    string currentAnimation = "";
    Animator animator;
    bool isShooting;

    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
        agent = transform.GetComponent<NavMeshAgent>();
        animator = transform.GetComponent<Animator>();
        player = playerController.transform;
        currentTime = Time.time;
        nextTimeToFire = Time.time;
    }

    // Small animation manager for the enemy
    void playAnimation(string animation, float crossFadeTime = 0.2f)
    {
        if (animation != "shoot")
        {
            if (currentAnimation != animation)
            {
                currentAnimation = animation;
                animator.CrossFade(animation, crossFadeTime);
            }
        }
        else
        {
            currentAnimation = animation;
            animator.CrossFadeInFixedTime(animation, 0.2f, 0, 0f);
        }
    }

    void Update()
    {
        // if (!agent.isOnNavMesh)
        // {
        //     Debug.Log("Not on a NAVMESH\n");
        // }

        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("enemy");
        if (Physics.Raycast(enemyEyeSight.position, (playerEyeSight.position - enemyEyeSight.position).normalized, out hit, enemySightRange, layerMask))
        {
            if (hit.transform.GetComponent<PlayerMovement>() != null)
            {
                Chase();
            }
            else
            {
                gun.SetActive(false);
                // Debug.Log(hit.transform.name);
                Patroling();
                isShooting = false;
            }
        }
        else
        {
            gun.SetActive(false);
            Patroling();
            isShooting = false;
        }
    }

    // Late update as Animation will be run before this
    void LateUpdate()
    {
        if (isShooting)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void Chase()
    {
        agent.speed = runSpeed;
        float distance = Vector3.Distance(transform.position, player.position);

        // if shooting continue till its out of range/sight
        if (isShooting)
        {
            if (Time.time >= shootStateEndTime || distance > enemyShootRange)
            {
                isShooting = false;
            }
            else
            {
                gun.SetActive(true);
                Shoot();
                return;
            }
        }

        // start shooting after coming well under the shoot range so that the player cant just simply take one step backward to reset the chase state
        if (!isShooting && distance <= enemyShootRange)
        {
            float temp = Random.Range(enemyShootRange / 2, enemyShootRange);
            if (distance < temp)
            {
                isShooting = true;
                shootStateEndTime = Time.time + shootCommitTime;
                return;
            }
        }

        // chase
        playAnimation("run", 0.2f);
        if (!agent.hasPath || Vector3.Distance(agent.destination, player.position) > 5f)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    // muzzle fire particle system
    public void muzzleFire()
    {
        muzzle.Play();
    }

    // Shoot calculations
    void Shoot()
    {
        agent.ResetPath();
        if (Time.time >= nextTimeToFire)
        {
            playAnimation("shoot");
            audioSource.Play();
            nextTimeToFire = Time.time + 1f / fireRate;
            float temp = Random.Range(0f, 100f);
            if (temp <= hitChance)
            {
                playerController.takeDamage(damage);
            }
        }
    }

    // Random patrol
    private void Patroling()
    {
        agent.speed = walkSpeed;
        if (!walkPointSet && Time.time >= currentTime + pause)
        {
            SearchWalkPoint();
            agent.SetDestination(walkPoint);
        }
        if (walkPointSet)
        {
            playAnimation("walk", 0.2f);
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance + 0.1f)
            {
                currentTime = Time.time;
                playAnimation("idle", 0.2f);
                walkPointSet = false;
            }
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + errorDistance, transform.position.z + randomZ);

        RaycastHit hit;

        if (Physics.Raycast(walkPoint, -transform.up, out hit, 2 * errorDistance, whatIsGround))
        {
            walkPointSet = true;
            walkPoint = hit.point;
        }
    }
}
