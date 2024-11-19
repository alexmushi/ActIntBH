using UnityEngine;
using System.Collections.Generic;

public class Sprayer : MonoBehaviour
{
    public GameObject projectilePrefab;       // Prefab for the projectile
    public float projectileSpeed = 10f;       // Speed of the projectile
    public int numberOfProjectiles = 32;      // Number of projectiles in the spray
    public float[] triggerTimes;              // Array of times to trigger the spray attack
    private HashSet<int> triggeredAttacks;    // Tracks which attacks have already been triggered

    private AudioManager audioManager;        // Reference to the AudioManager
    private ScreenShake screenShake;          // Reference to ScreenShake for screen shake effect

    private void Start()
    {
        // Initialize the set of triggered attacks
        triggeredAttacks = new HashSet<int>();

        // Get reference to the AudioManager instance
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }

        // Get reference to the ScreenShake component
        screenShake = Camera.main.GetComponent<ScreenShake>();
        if (screenShake == null)
        {
            Debug.LogError("ScreenShake component not found on the main camera.");
        }
    }

    private void Update()
    {
        if (audioManager == null) return;

        // Get the current playback time from the AudioManager
        float currentPlaybackTime = audioManager.GetCurrentPlaybackTime();

        // Check each trigger time
        for (int i = 0; i < triggerTimes.Length; i++)
        {
            // If the playback time has reached the trigger time and the spray hasn't been triggered yet
            if (currentPlaybackTime >= triggerTimes[i] && !triggeredAttacks.Contains(i))
            {
                // Trigger the spray attack
                SprayAttack();
                triggeredAttacks.Add(i); // Mark this spray as triggered
            }
        }
    }

    private void SprayAttack()
    {
        float angleStep = 360f / numberOfProjectiles; // Angle between each projectile

        // Trigger the screen shake effect
        if (screenShake != null)
        {
            screenShake.TriggerShake();
        }

        // Loop to create projectiles in a circular spray pattern
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the angle for this projectile
            float angle = i * angleStep;

            // Convert the angle to a direction vector
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            // Instantiate the projectile and set its direction
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Bullet>().SetDirection(direction * projectileSpeed);
        }
    }
}
