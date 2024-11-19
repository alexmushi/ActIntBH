using UnityEngine;
using System.Collections;

public class MoverCubo : MonoBehaviour
{
    public float moveDistance = 20f;         // Distance to move to the right
    public float moveSpeed = 50f;            // Speed at which the object moves to the target position
    public float[] triggerTimes;             // Array of times to trigger movement

    private Vector3 initialPosition;         // The initial position of the object
    private Vector3 targetPosition;          // The target position to move towards
    private AudioManager audioManager;       // Reference to the AudioManager
    private int currentTriggerIndex = 0;     // Keeps track of the next trigger time in the array
    private bool isMoving = false;           // Tracks if the object is currently moving

    private void Start()
    {
        // Get reference to the AudioManager instance
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }

        // Set initial position and target position to start at initial
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        // Check if AudioManager is available and if we still have trigger times left
        if (audioManager == null || currentTriggerIndex >= triggerTimes.Length) return;

        // Get the current playback time from the AudioManager
        float playbackTime = audioManager.GetCurrentPlaybackTime();

        // Check if the playback time has reached or passed the next trigger time
        if (playbackTime >= triggerTimes[currentTriggerIndex] && !isMoving)
        {
            StartCoroutine(MoveRight());
            currentTriggerIndex++;
        }
    }

    private IEnumerator MoveRight()
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.right * moveDistance;

        while (elapsedTime < 2f)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / 2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        isMoving = false;
    }
}