using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Enemy enemy;
    public void onHit(int damage)
    {
        enemy.takeDamage(damage);
    }

}
