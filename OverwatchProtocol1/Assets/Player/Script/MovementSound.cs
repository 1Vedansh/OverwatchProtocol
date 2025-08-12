using UnityEngine;

public class MovementSound : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public AudioSource walk;
    public AudioSource sprint;
    
    float x, y;
    bool isMoving;
    bool wasMoving;

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        float threshold = 0.01f;

        isMoving = Mathf.Abs(x) > threshold || Mathf.Abs(y) > threshold;

        if (isMoving && !wasMoving)
        {
            if (Mathf.Approximately(playerMovement.speed, playerMovement.sprintspeed))
            {
                sprint.Play();
            }
            else
            {
                walk.Play();
            }
        }
        else if (!isMoving && wasMoving)
        {
            walk.Stop();
            sprint.Stop();
        }

        if (isMoving && wasMoving)
        {
            if (Mathf.Approximately(playerMovement.speed, playerMovement.sprintspeed))
            {
                if (!sprint.isPlaying)
                {
                    walk.Stop();
                    sprint.Play();
                }
            }
            else
            {
                if (!walk.isPlaying)
                {
                    sprint.Stop();
                    walk.Play();
                }
            }
        }
        wasMoving = isMoving;
    }
}
