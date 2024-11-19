using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;       // The prefab for the oscillating projectile
    public GameObject firstShotPrefab;
    public int bulletsPerBurst = 5;           // Number of bullets per burst
    public float bulletDelay = 0.2f;          // Delay between each bullet in a burst
    public float projectileSpeed = 15f;       // Speed of the projectile
    public float[] triggerTimes;              // Array of times on the music to trigger the burst

    private bool isShooting = false;
    private HashSet<int> triggeredBursts = new HashSet<int>();  // Keeps track of which bursts have been triggered
    private AudioManager audioManager;        // Reference to the AudioManager

    private void Start()
    {
        // Get reference to the AudioManager instance
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }

    private void Update()
    {
        // Check if the AudioManager is available
        if (audioManager == null)
        {
            return;
        }

        // Get the current playback time
        float currentPlaybackTime = audioManager.GetCurrentPlaybackTime();

        // Loop through each trigger time to check if it should activate the burst
        for (int i = 0; i < triggerTimes.Length; i++)
        {
            // If the playback time has reached the trigger time and it hasn't been triggered yet
            if (currentPlaybackTime >= triggerTimes[i] && !triggeredBursts.Contains(i))
            {
                TriggerBurst();
                triggeredBursts.Add(i); // Mark this burst as triggered
            }
        }
    }

    public void TriggerBurst()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootBurst());
        }
    }

    IEnumerator ShootBurst()
    {
        isShooting = true;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            if (i == 0)
            {
                // Use the firstShotPrefab for the first shot in the burst
                Shoot(firstShotPrefab);
            }
            else
            {
                // Use the regular projectilePrefab for the remaining shots
                Shoot(projectilePrefab);
            }
            yield return new WaitForSeconds(bulletDelay);
        }

        isShooting = false;
    }

    void Shoot(GameObject prefab)
    {
        // Instantiate a projectile at the shooter's position
        GameObject projectile = Instantiate(prefab, transform.position, transform.rotation);

        // Set the projectile to move backward
        projectile.GetComponent<OscillatingProjectile>().SetDirection(transform.forward * projectileSpeed);
    }
}
