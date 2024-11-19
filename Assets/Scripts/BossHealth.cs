using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int health = 3;           // Number of hits the boss can take

    private void OnTriggerEnter(Collider other)
    {

        // Reduce boss health
        health--;

        // Destroy the projectile to prevent multiple hits from the same projectile
        Destroy(other.gameObject);

         // Check if boss health has reached zero
        if (health <= 0)
        {
            WinGame();
        }
   
    }

    private void WinGame()
    {
        Debug.Log("You Win!");
        
        // Pause the game
        Time.timeScale = 0f;
    }
}
