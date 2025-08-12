using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    
    [HideInInspector]public Vector3 endPoint;

    [Tooltip("Speed of the cosmestic bullet(400)")]
    public float speed;

    Vector3 direction;

    void Start()
    {
        direction = (endPoint - transform.position).normalized;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, endPoint) < step)
        {
            Destroy(gameObject);
        }
    }
}
