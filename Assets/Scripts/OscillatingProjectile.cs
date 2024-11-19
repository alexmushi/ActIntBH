using UnityEngine;

public class OscillatingProjectile : MonoBehaviour
{
    public float oscillationAmplitude = 1f;  // Amplitude of oscillation
    public float oscillationFrequency = 2f;  // Frequency of oscillation
    private Vector3 direction;               // Direction the projectile will move in (includes speed)
    private Vector3 startPosition;

    private float startTime;                 // Time at which this bullet was created

    public static int bulletsOnScreen = 0;

    void Start()
    {
        bulletsOnScreen++;
        // Store the starting position and creation time for synchronized oscillation
        startPosition = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        // Move the projectile in the main direction with applied speed
        transform.position += direction * Time.deltaTime;

        // Calculate the oscillation based on a shared sine wave, with a time offset for delay
        float oscillationOffset = Mathf.Sin((Time.time - startTime) * oscillationFrequency) * oscillationAmplitude;
        
        // Apply oscillation only on the x-axis, maintaining the forward movement speed
        transform.position = new Vector3(startPosition.x + oscillationOffset, transform.position.y, transform.position.z);
    }

    // Method to set the initial direction of the projectile, including speed
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection; // This should include the speed factor (e.g., -transform.forward * speed)
    }

    void OnBecameInvisible()
    {
        DestroyProjectile(); // Destroy the projectile when it leaves the camera's view
    }

    private void DestroyProjectile()
    {
        bulletsOnScreen--;
        Destroy(gameObject);
    }
}
