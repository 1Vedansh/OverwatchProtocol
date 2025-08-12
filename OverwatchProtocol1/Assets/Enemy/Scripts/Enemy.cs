using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public Image image;
    public Player playerController;
    public bool isAlive;

    RagDoll ragdoll;
    Rigidbody[] rigidbodies;
    int currentHealth;

    void Start()
    {
        ragdoll = transform.GetComponent<RagDoll>();
        isAlive = true;
        currentHealth = maxHealth;
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            Hitbox hitbox = rigidbody.AddComponent<Hitbox>();
            hitbox.enemy = transform.GetComponent<Enemy>();
        }
    }

    // Function to give enemy damage
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        image.fillAmount = currentHealth / (float)maxHealth;
        if (currentHealth <= 0 && isAlive)
        {
            Die();
            isAlive = false;
        }
    }

    // Enemy dies, ragdoll activates
    public void Die()
    {
        transform.GetComponent<EnemyMove>().enabled = false;
        transform.GetComponent<NavMeshAgent>().enabled = false;
        ragdoll.ActivateRagDoll();
        playerController.enemyKillReward();
    }
}
