using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;               // Initial speed of the bullet
    public float growthRate = 1.05f;        // Exponential growth rate for speed and scale
    public bool enableGrowth = false;       // Toggle to enable or disable growth
    public bool enableSpin = false;         // Toggle to enable or disable spinning
    public float spinSpeed = 50f;           // Speed of the Y-axis spin

    private Vector3 direction;              // Movement direction of the bullet
    public static int bulletsOnScreen = 0;

    void Start()
    {
        bulletsOnScreen++;
    }

    void Update()
    {
        if (enableGrowth)
        {
            // Exponentially increase speed
            speed *= Mathf.Pow(growthRate, Time.deltaTime);

            // Exponentially grow the bullet's scale
            float scaleGrowth = Mathf.Pow(growthRate, Time.deltaTime);
            transform.localScale *= scaleGrowth;
        }

        if (enableSpin)
        {
            // Rotate the bullet around the Y-axis
            transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        }

        // Move the bullet in the assigned direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Destroy the bullet when it leaves the camera's view
        bulletsOnScreen--;
    }
}
