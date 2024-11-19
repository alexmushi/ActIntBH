using UnityEngine;
using System.Collections;

public class SmallShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;  // Duration of the shake
    public float shakeMagnitude = 0.9f; // Magnitude of the shake

    private Vector3 originalPosition;

    private void Awake()
    {
        // Save the original camera position
        originalPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        // Start the shake coroutine
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Generate a random offset within the shake magnitude
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            // Apply the offset to the camera position
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Reset the camera to its original position after the shake
        transform.localPosition = originalPosition;
    }
}
