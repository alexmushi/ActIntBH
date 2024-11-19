using UnityEngine;

public class TimedMover : MonoBehaviour
{
    public float moveDistance = 2f;           // Distance to move forward or backward
    public float moveSpeed = 50f;             // Speed at which the object moves to the target position
    public float rightMoveSpeed = 8f;         // Speed for moving slowly to the right
    public float returnTime = 10f;            // Time after which the object returns to the initial position
    public float[] triggerTimes;              // Array of times to trigger movement

    private bool moveToFront = true;          // Determines whether to move to the front or back
    private Vector3 initialPosition;          // The initial position
    private Vector3 frontPosition;            // The front position
    private Vector3 backPosition;             // The back position
    private Vector3 targetPosition;           // The target position to move towards
    private AudioManager audioManager;        // Reference to the AudioManager
    private int currentTriggerIndex = 0;      // Keeps track of the next trigger time in the array
    private float elapsedTime = 0f;           // Tracks the time since movement started
    private bool isReturning = false;         // Tracks if the object is returning to the initial position

    private void Start()
    {
        // Get reference to the AudioManager instance
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }

        // Set initial and calculated positions
        initialPosition = transform.position;
        backPosition = initialPosition;
        frontPosition = initialPosition + new Vector3(0, 0, moveDistance);

        // Set the initial target position
        targetPosition = backPosition;
    }

    private void Update()
    {
        // Check if AudioManager is available and if we still have trigger times left
        if (audioManager == null || currentTriggerIndex >= triggerTimes.Length) return;

        // Get the current playback time from the AudioManager
        float playbackTime = audioManager.GetCurrentPlaybackTime();

        // Check if the playback time has reached or passed the next trigger time
        if (!isReturning && playbackTime >= triggerTimes[currentTriggerIndex])
        {
            // Set the target position based on the moveToFront toggle
            targetPosition = moveToFront ? frontPosition : backPosition;

            // Toggle the direction for the next trigger
            moveToFront = !moveToFront;

            // Move to the next trigger time in the array
            currentTriggerIndex++;

            // Ensure we don’t exceed the array bounds
            if (currentTriggerIndex >= triggerTimes.Length)
            {
                currentTriggerIndex = triggerTimes.Length - 1;
            }
        }

        // Move the object towards the target position on the z-axis
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Slowly move the object to the right if it's not returning
        if (!isReturning)
        {
            transform.position += Vector3.right * rightMoveSpeed * Time.deltaTime;
        }

        // Update elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the object should return to the initial position
        if (elapsedTime >= returnTime && !isReturning)
        {
            // Set the target to the initial position
            targetPosition = initialPosition;
            isReturning = true; // Set return flag
        }

        // Check if the object has reached the initial position after returning
        if (isReturning && transform.position == initialPosition)
        {
            // Reset elapsed time and return flag
            elapsedTime = 0f;
            isReturning = false;
            currentTriggerIndex = 0; // Reset the trigger index if needed to restart the cycle
        }
    }
}
