using UnityEngine;

public class RandomShooter : MonoBehaviour
{
    public GameObject projectilePrefab;       // Prefab for the projectile
    public float projectileSpeed = 10f;       // Speed of the projectile
    public float shootingInterval = 1f;      // Time interval between shots
    public float startTime = 0f;             // Time in music when shooting starts
    public float stopTime = 10f;             // Time in music when shooting stops

    private AudioManager audioManager;       // Reference to the AudioManager
    private float timeSinceLastShot = 0f;    // Timer to track shooting interval

    void Start()
    {
        // Get the AudioManager instance
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }

    void Update()
    {
        // Ensure AudioManager is available
        if (audioManager == null) return;

        // Get the current playback time of the background music
        float playbackTime = audioManager.GetCurrentPlaybackTime();

        // Check if the current playback time is within the shooting time window
        if (playbackTime >= startTime && playbackTime <= stopTime)
        {
            // Update the shooting timer
            timeSinceLastShot += Time.deltaTime;

            // Check if it's time to shoot
            if (timeSinceLastShot >= shootingInterval)
            {
                ShootRandomBackProjectile();
                timeSinceLastShot = 0f; // Reset the timer
            }
        }
    }

    void ShootRandomBackProjectile()
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
        else
        {
            Debug.LogError("The projectile prefab does not have a Bullet component attached!");
        }
    }
}
