using UnityEngine;

public class BulletCounterDisplay : MonoBehaviour
{
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white; 

        // Display the bullet count at the top left of the screen
        GUI.Label(new Rect(10, 10, 200, 20), "Balas: " + (OscillatingProjectile.bulletsOnScreen + Bullet.bulletsOnScreen), guiStyle);
    }
}

