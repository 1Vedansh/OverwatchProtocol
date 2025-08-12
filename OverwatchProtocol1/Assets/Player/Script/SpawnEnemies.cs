using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : MonoBehaviour
{
    public int maxAllowedEnemies;

    public float despawnDistance;
    public float maxSpawnDistance;
    public float timeUntilNextSpawn;

    public List<GameObject> enemies;
    public GameObject enemyPrefab;

    public Transform playerEyeSight;
    public ProceduralWorldStart proceduralWorldStart;

    int gameDifficulty;
    public EnemyProperties easyEnemy;
    public EnemyProperties mediumEnemy;
    public EnemyProperties hardEnemy;

    Player playerController;
    float nextSpawnTime;
    int spawnAttempts = 10;

    void Start()
    {
        playerController = transform.GetComponent<Player>();
        gameDifficulty = proceduralWorldStart.gameDifficulty;
        nextSpawnTime = Time.time;
    }

    void setEnemyProperties(Enemy enemy, EnemyMove enemyMove, EnemyProperties enemyProperties)
    {
        enemy.maxHealth = enemyProperties.health;
        enemyMove.hitChance = enemyProperties.hitChance;
        enemyMove.damage = enemyProperties.damage;
        enemyMove.fireRate = enemyProperties.fireRate;
        enemyMove.walkPointRange = enemyProperties.walkPointRange;
        enemyMove.enemySightRange = enemyProperties.enemySightRange;
        enemyMove.enemyShootRange = enemyProperties.enemyShootRange;
        enemyMove.walkSpeed = enemyProperties.walkSpeed;
        enemyMove.runSpeed = enemyProperties.runSpeed;
    }

    void spawnEnemy(RaycastHit hit)
    {
        GameObject enemy = Instantiate(enemyPrefab, hit.point, Quaternion.identity);

        Enemy enemyComp = enemy.GetComponent<Enemy>();
        EnemyMove moveComp = enemy.GetComponent<EnemyMove>();

        enemyComp.playerController = playerController;
        moveComp.playerController = playerController;
        moveComp.playerEyeSight = playerEyeSight;

        if (gameDifficulty == 0)
        {
            setEnemyProperties(enemyComp, moveComp, easyEnemy);
            maxAllowedEnemies = 30;
        }
        else if (gameDifficulty == 1)
        {
            setEnemyProperties(enemyComp, moveComp, mediumEnemy);
            maxAllowedEnemies = 30;
        }
        else if (gameDifficulty == 2)
        {
            setEnemyProperties(enemyComp, moveComp, hardEnemy);
            maxAllowedEnemies = 30;
        }

        enemies.Add(enemy);
    }

    void Update()
    {
        RaycastHit hit;
        int attempts = spawnAttempts;
        Vector3 playerPosition = transform.position;

        // Destroy enemies outside of despawn radius
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(enemies[i].transform.position, transform.position) >= despawnDistance || (enemies[i].GetComponent<Enemy>().isAlive == false))
            {
                Destroy(enemies[i], 10f);
                enemies.RemoveAt(i);
            }
        }

        // Spawn enemies if allowed
        if (enemies.Count < maxAllowedEnemies && Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + timeUntilNextSpawn;
            do
            {
                float randX = Random.Range(-maxSpawnDistance, maxSpawnDistance);
                float randZ = Random.Range(-maxSpawnDistance, maxSpawnDistance);
                Vector3 raycastPosition = new Vector3(playerPosition.x + randX, 1000, playerPosition.z + randZ);
                if (Physics.Raycast(raycastPosition, Vector3.down, out hit, Mathf.Infinity))
                {
                    if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "ground")
                    {
                        NavMeshHit navHit;
                        if (NavMesh.SamplePosition(hit.point, out navHit, 3f, NavMesh.AllAreas))
                        {
                            spawnEnemy(hit);
                            break;
                        }
                    }
                }
                else
                {
                    Debug.Log("Missed All layers");
                }
                attempts--;
            } while (attempts > 0);
        }
    }
}

[System.Serializable]
public class EnemyProperties
{
    public int health;
    public int hitChance;
    public int damage;
    public float fireRate;
    public float walkPointRange;
    public float enemySightRange;
    public float enemyShootRange;
    public float walkSpeed;
    public float runSpeed;
}