using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;   // The GameObject to spawn
    public float spawnTime = 10f;       // The time on the audio clip when the object should spawn
    private bool hasSpawned = false;   // Track if the object has been spawned

    void Update()
    {
        // Check if AudioManager instance is available
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance not found.");
            return;
        }

        // Get the current playback time from the AudioManager
        float currentPlaybackTime = AudioManager.Instance.GetCurrentPlaybackTime();

        // Spawn the object if the current playback time has reached the spawn time and it hasn't spawned yet
        if (!hasSpawned && currentPlaybackTime >= spawnTime)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
            hasSpawned = true; // Mark as spawned to prevent spawning multiple times
        }
    }
}
