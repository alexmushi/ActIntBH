using UnityEngine;

public class AimedShooter : MonoBehaviour
{
    public GameObject projectilePrefab;       // Prefab for the projectile
    public float projectileSpeed = 10f;       // Speed of the projectile
    public float[] triggerTimes;              // Array of times to trigger shooting

    private GameObject player;                // Reference to the player
    private AudioManager audioManager;        // Reference to the AudioManager
    private int currentTriggerIndex = 0;      // Tracks the next trigger time in the array
    private SmallShake smallShake;          // Reference to ScreenShake for screen shake effect


    void Start()
    {
        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in the scene. Make sure the player has the 'Player' tag.");
        }

        // Get the AudioManager instance
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
        smallShake = Camera.main.GetComponent<SmallShake>();

        }
    void Update()
    {
        // Check if the player and AudioManager are available and if there are trigger times left
        if (player == null || audioManager == null || currentTriggerIndex >= triggerTimes.Length) return;

        // Get the current playback time from the AudioManager
        float playbackTime = audioManager.GetCurrentPlaybackTime();

        // Process all trigger times that have passed
        while (currentTriggerIndex < triggerTimes.Length && playbackTime >= triggerTimes[currentTriggerIndex])
        {
            ShootAtPlayer();
            currentTriggerIndex++; // Move to the next trigger time
        }
    }


    void ShootAtPlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the direction of the projectile to aim at the player
        projectile.GetComponent<Bullet>().SetDirection(directionToPlayer * projectileSpeed);
        // Trigger the screen shake effect
        if (smallShake != null)
        {
            smallShake.TriggerShake();
        }
    }
}
