using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f;            // Regular movement speed
    public float slowMoveSpeed = 2f;        // Slow movement speed
    public float speed = 5f;                // Base move speed
    public float dashSpeed = 15f;           // Speed during dash
    public float dashDuration = 0.3f;       // Duration of the dash
    public GameObject projectilePrefab;         // Projectile prefab for shooting
    public float projectileSpeed = 10f;         // Speed of the projectile
    public KeyCode dashKey = KeyCode.Space; // Key to initiate dash
    public KeyCode slowKey = KeyCode.RightShift; // Key to slow down
    public KeyCode shootKey = KeyCode.F;        // Key to shoot
    public AudioClip shootSound;             // Sound clip for shooting


    private bool isDashing = false;         // Tracks if the player is currently dashing
    private bool isInvulnerable = false;    // Tracks if the player is invulnerable during dash
    private TrailRenderer trailRenderer;     // Reference to the Trail Renderer
    
    
    void Start()
    {
        // Get the Trail Renderer component
        trailRenderer = GetComponent<TrailRenderer>();

        // Ensure the trail is initially disabled
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Check for dash input
        if (Input.GetKeyDown(dashKey) && !isDashing && movement != Vector3.zero)
        {
            StartCoroutine(Dash(movement));
        }

        // Check for slow key and adjust move speed accordingly
        if (Input.GetKey(slowKey) && !isDashing)
        {
            moveSpeed = slowMoveSpeed;
        }
        else
        {
            moveSpeed = speed;
        }

        // Check for shoot input
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }

        // Determine current speed and move the player
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        transform.Translate(movement * currentSpeed * Time.deltaTime, Space.World);
    }

    private IEnumerator Dash(Vector3 dashDirection)
    {
        isDashing = true;
        isInvulnerable = true;

        // Enable the trail renderer during the dash
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }

        // Move in dash direction for the specified dash duration
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        // Disable the trail renderer after the dash
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        // End dash and disable invulnerability
        isDashing = false;
        isInvulnerable = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is invulnerable
        if (isInvulnerable) return;

        // Trigger game over when the player collides with any trigger object
        GameOver();
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        moveSpeed = 0f;
    }

    void Shoot()
    {
        // Define an offset distance in front of the player
        float spawnDistance = 0.7f;

        // Calculate the spawn position in front of the player
        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

        // Instantiate a single projectile and set it to move forward
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);

        // Set the direction of the projectile to move forward
        projectile.GetComponent<Bullet>().SetDirection(transform.forward * projectileSpeed);

        // SFX
        AudioManager.Instance.PlaySFX(shootSound);
    }

}
