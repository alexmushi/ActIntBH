using UnityEngine;

public class MoveBoss : MonoBehaviour
{
    public int sideSpeed = 5;                // Side-to-side movement speed
    public float directionChangeInterval = 2f; // Time in seconds to switch side-to-side direction
    public float frontBackSpeed = 2f;        // Speed of front-back oscillation
    public float frontBackDistance = 2f;     // Distance of front-back oscillation
    public GameObject projectilePrefab;      // Prefab of the projectile to shoot
    public float shootingInterval = 1f;      // Interval between each shot
    public float projectileSpeed = 10f;      // Speed of the projectile
    public float sprayInterval = 3f;         // Interval between each spray attack
    public float attackSwitchInterval = 10f; // Interval to switch between attacks

    public int bulletHellProjectiles = 20;   // Number of projectiles in bullet hell attack
    public float bulletHellRotationSpeed = 30f; // Speed of bullet rotation in degrees per second

    private Vector3 startPosition;
    private int sideDirection = 1;           // 1 for right, -1 for left
    private float timeSinceDirectionChange;  // Timer to track time since last direction change
    private float timeSinceLastShot;         // Timer for shooting
    private float timeSinceLastSpray;        // Timer for the spray attack
    private float timeSinceLastSwitch;       // Timer to track attack switching
    private int attackMode = 0;              // Tracks the current attack mode (0: normal, 1: random, 2: bullet hell)
    private float bulletHellAngle = 0f;      // Tracks the current angle for bullet hell rotation

    private ScreenShake screenShake;         // Reference to the ScreenShake script

    void Start()
    {
        startPosition = transform.position;
        timeSinceDirectionChange = 0f;
        timeSinceLastShot = 0f;
        timeSinceLastSpray = 0f;
        timeSinceLastSwitch = 0f;
        screenShake = Camera.main.GetComponent<ScreenShake>();
    }

    void Update()
    {
        // Update the timer for direction change
        timeSinceDirectionChange += Time.deltaTime;

        // Change side-to-side direction after the set interval
        if (timeSinceDirectionChange >= directionChangeInterval)
        {
            sideDirection *= -1; // Reverse side-to-side direction
            timeSinceDirectionChange = 0f; // Reset the timer
        }

        // Side-to-side movement
        transform.Translate(Vector3.right * sideDirection * sideSpeed * Time.deltaTime);

        // Front-back oscillation using a sine wave
        float frontBackOffset = Mathf.Sin(Time.time * frontBackSpeed + Mathf.PI / 2) * frontBackDistance;
        transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z + frontBackOffset);

        // Update the timer for switching attacks
        timeSinceLastSwitch += Time.deltaTime;
        if (timeSinceLastSwitch >= attackSwitchInterval)
        {
            attackMode = (attackMode + 1) % 3; // Cycle through attack modes
            timeSinceLastSwitch = 0f; // Reset the attack switch timer
        }

        // Update the shooting timer and perform the appropriate attack
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootingInterval)
        {
            switch (attackMode)
            {
                case 0:
                    NormalShoot();
                    break;
                case 1:
                    RandomAngleAttack();
                    break;
                case 2:
                    BulletHellAttack();
                    break;
            }
            timeSinceLastShot = 0f;
        }

        // Update the spray attack timer and perform spray if interval has passed
        timeSinceLastSpray += Time.deltaTime;
        if (timeSinceLastSpray >= sprayInterval)
        {
            SprayAttack();
            timeSinceLastSpray = 0f;
        }
    }

    void NormalShoot()
    {
        // Instantiate a single projectile and set it to move forward
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Set the direction of the projectile to move forward
        projectile.GetComponent<Bullet>().SetDirection(-transform.forward * projectileSpeed);
    }

    void RandomAngleAttack()
    {
        // Generate a random angle within the 180-degree arc behind the object
        float randomAngle = Random.Range(-90f, 90f); // -90 to 90 degrees behind

        // Convert the random angle to a direction vector
        Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * -transform.forward;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the projectile's direction
        if (projectile.GetComponent<Bullet>() != null)
        {
            projectile.GetComponent<Bullet>().SetDirection(randomDirection * projectileSpeed);
        }
    }

    void BulletHellAttack()
    {
        // Increment the rotation angle for the bullet hell attack
        bulletHellAngle += bulletHellRotationSpeed * Time.deltaTime;

        for (int i = 0; i < bulletHellProjectiles; i++)
        {
            // Calculate the angle for this projectile
            float angle = bulletHellAngle + (360f / bulletHellProjectiles) * i;

            // Convert the angle to a direction vector
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            // Instantiate the projectile and set its direction
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Bullet>().SetDirection(direction * projectileSpeed);
        }
    }

    void SprayAttack()
    {
        int numberOfProjectiles = 32; // Number of directions to shoot in
        float angleStep = 360f / numberOfProjectiles; // Angle between each projectile

        if (screenShake != null)
        {
            screenShake.TriggerShake();
        }

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the angle for this projectile
            float angle = i * angleStep;

            // Convert the angle to a direction vector
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            // Instantiate and set the projectile's direction
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Bullet>().SetDirection(direction * projectileSpeed);
        }
    }
}
